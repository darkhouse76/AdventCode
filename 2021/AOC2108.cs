using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace CodeTAF
{
    public class AOC2108 : MonoBehaviour
    {
        [SerializeField]
        private bool partTwo = false;
        [SerializeField]
        private bool useTestInput = true;
        [SerializeField]
        private bool run = false;
        private string input;

        private const string inputFolderName = "inputs";


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

        void part1() {
            
            int[] targetSizes = new int[] { 2, 3, 4, 7 };
            int targetsFound = 0;

            string[] displayInfos = input.Split(new string[] { "\r\n", " | " }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 1; i < displayInfos.Length; i += 2) {
                
                string[] patterns = displayInfos[i].Split(" ");
                foreach (string pattern in patterns) {
                    //print(pattern.Count());

                    if (targetSizes.Contains(pattern.Count())) { targetsFound++; }
                }
                //print(patterns.Count());
            }

            print($"1, 4, 7, and 8 appears {targetsFound}x times in output Patterns ");
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

