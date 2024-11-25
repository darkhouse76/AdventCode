using System;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace CodeTAF
{
    public class AOC2104 : MonoBehaviour
    {
        [SerializeField]
        private bool partTwo = false;
        [SerializeField]
        private bool useTestInput = true;
        [SerializeField]
        private bool run = false;
        private string input;

        private string inputFolderName = "inputs";


        string rootPath {
            get {
                var g = AssetDatabase.FindAssets($"t:Script {GetType().Name}");
                return AssetDatabase.GUIDToAssetPath(g[0]);
            }
        }
        string inputPath {
            get {
                return $"{rootPath[..^(GetType().Name.Length + 3)]}{inputFolderName}/";
            }
        }
        string day {
            get {
                return GetType().Name[^2..];
            }
        }
        string testInput {
            get {
                string filePath = $"{inputPath}{day}test.txt";

                if (!File.Exists(filePath)) {
                    Debug.LogError($"NO input file found @ {filePath}");
                    return null;
                }
                return File.ReadAllText(filePath);
            }
        }
        string realInput {
            get {
                string filePath = $"{inputPath}{day}real.txt";

                if (!File.Exists(filePath)) {
                    Debug.LogError($"NO input file found @ {filePath}");
                    return null;
                }
                return File.ReadAllText(filePath);
            }
        }

        void part1() {
            string[] lines = input.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Length; i++) { print(lines[i]); }

        }

        void part2() {


        }

        void Update() {
            if (run) {
                run = false;
                Debug.Log("========================================================================");

                input = useTestInput ? testInput : realInput;

                var startTime = System.DateTime.Now;

                if (partTwo) { part2(); }
                else { part1(); }
                print($"Took {System.DateTime.Now - startTime} to complete.");
            }
        }
    }
}

