using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace CodeTAF
{
    public class AOC2106 : MonoBehaviour
    {
        [SerializeField]
        private bool partTwo = false;
        [SerializeField]
        private bool useTestInput = true;
        [SerializeField]
        private bool run = false;
        private string input;

        private readonly string inputFolderName = "inputs";


        string RootPath {
            get {
                var g = AssetDatabase.FindAssets($"t:Script {GetType().Name}");
                return AssetDatabase.GUIDToAssetPath(g[0]);
            }
        }
        string InputPath {
            get {
                return $"{RootPath[..^(GetType().Name.Length + 3)]}{inputFolderName}/";
            }
        }
        string Day {
            get {
                return GetType().Name[^2..];
            }
        }
        string TestInput {
            get {
                string filePath = $"{InputPath}{Day}test.txt";

                if (!File.Exists(filePath)) {
                    Debug.LogError($"NO input file found @ {filePath}");
                    return null;
                }
                return File.ReadAllText(filePath);
            }
        }
        string RealInput {
            get {
                string filePath = $"{InputPath}{Day}real.txt";

                if (!File.Exists(filePath)) {
                    Debug.LogError($"NO input file found @ {filePath}");
                    return null;
                }
                return File.ReadAllText(filePath);
            }
        }


        class fish
        {
            int SpawnTimer { get; set; }
            public fish LastChild { get; set; }

            public fish() {
                SpawnTimer = 8;
            }

            public fish(int customStartTimer) {
                SpawnTimer = customStartTimer;
            }

            public bool nextDay() {
                if (SpawnTimer-- == 0) {
                    LastChild = new fish();
                    SpawnTimer = 6;
                    return true;
                }
                return false;
            }
        }

        int[] parseInput() {
            string[] startFishTimers = input.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            return Array.ConvertAll(startFishTimers, int.Parse);

        } 


        void part1() {

            int[] startFishTimers = parseInput();
            List<fish> allFish = new();
            int numDays = 80; //number of days to goto

            for (int i = 0; i < startFishTimers.Length; i++) {
                //print(startFishTimers[i]);
                allFish.Add(new fish(startFishTimers[i]));
            }         

            for (int i = 0; i < numDays; i++) {
                List<fish> newFish = new();
                foreach (fish curFish in allFish) { 
                    if (curFish.nextDay()) { newFish.Add(curFish.LastChild); }
                }
                if (newFish.Count > 0) { allFish.AddRange(newFish); }
            }

            print($"Total number of fish after {numDays} days = {allFish.Count}");
        }

        void part2() {


        }

        void Update() {
            if (run) {
                run = false;
                Debug.Log("========================================================================");

                input = useTestInput ? TestInput : RealInput;

                var startTime = System.DateTime.Now;

                if (partTwo) { part2(); }
                else { part1(); }
                print($"Took {System.DateTime.Now - startTime} to complete.");
            }
        }
    }
}

