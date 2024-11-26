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
            int[] startFishTimers = parseInput();
            //List<fish> allFish = new();
            int numDays = 256; //number of days to goto

            //new
            Int64 totalFish = startFishTimers.Length;
            Int64[] newFishDay = new Int64[7];
            Int64[] increaseNewFish = new Int64[7];
            int dayID;

            //new
            for ( int i = 0; i < 7; i++ )
            {
                newFishDay[i] = 0;
                increaseNewFish[i] = 0;
            }

            foreach (int thisDay in startFishTimers) {
                newFishDay[thisDay]++;
            }        


            //old
            //for (int i = 0; i < startFishTimers.Length; i++) {
            //    //print(startFishTimers[i]);
            //    allFish.Add(new fish(startFishTimers[i]));
            //}
            //old and new
            for (int i = 0; i < numDays; i++) {
                //old
                //List<fish> newFish = new();
                //foreach (fish curFish in allFish) {
                //    if (curFish.nextDay()) { newFish.Add(curFish.LastChild); }
                //}
                //if (newFish.Count > 0) { allFish.AddRange(newFish); }

                //new
                dayID = i % 7;

                //add last weeks fish to the upcoming new fish
                newFishDay[(i + 2) % 7] += increaseNewFish[dayID];

                //born new fish due today
                totalFish += newFishDay[dayID];
                //log the increase of new fish for next week. 
                increaseNewFish[dayID] = newFishDay[dayID];
                //if (i % 7 == 0) { print("==== New Week ======"); }
                //print($"New Fish for the Day = {newFishDay[dayID]}");

                //old
                //if (i % 7 == 0) { print("==== New Week ======"); }
                //print($"New Fish for the Day = {newFish.Count} (OLD)");
            }

            print($"Total number of fish after {numDays} days = {totalFish}");
            //print($"Total number of fish after {numDays} days = {allFish.Count} (OLD)");

            

            //for (int i = 0; i < numDays ; i++) {
            //    dayID = i % 7;

            //    //add last weeks fish to the upcoming new fish
            //    newFishDay[(i + 2) % 7] += increaseNewFish[dayID];

            //    //born new fish due today
            //    totalFish += newFishDay[dayID];
            //    //log the increase of new fish for next week. 
            //    increaseNewFish[dayID] = newFishDay[dayID];
            //    if (i % 7 == 0) { print("==== New Week ======"); }
            //    print($"New Fish for the Day = {newFishDay[dayID]}");
            //}

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

