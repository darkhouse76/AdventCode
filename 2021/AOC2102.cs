using System;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace CodeTAF
{
    public class AOC2102 : MonoBehaviour
    {
        [SerializeField]
        private bool partTwo = false;
        [SerializeField]
        private bool useTestInput = true;
        [SerializeField]
        private bool run = false;
        private string input;

        private readonly string inputFolderName = "inputs";


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

            string[] lines = input.Split(new[] { "\r\n", " " }, StringSplitOptions.None);


            //for (int i = 0; i < lines.Length; i++) {
            //    print(lines[i]);
            //}         


            int curDepth = 0;
            int curHorizontal = 0;

            for (int i = 0; i < lines.Length; i += 2) {
                int dir = 1;
                int units = int.Parse(lines[i + 1]);

                switch (lines[i]) {
                    case "forward":
                        curHorizontal += units;
                        break;

                    case "up":
                        units *= -1;
                        goto case "down";

                    case "down":
                        curDepth += units;
                        break;
                }
            }

            print($"Final position is Horizontal = {curHorizontal} Depth = {curDepth} Total = {curHorizontal * curDepth}");

        }

        void part2() {
            string[] lines = input.Split(new[] { "\r\n", " " }, StringSplitOptions.None);

            int curAim = 0;
            int curDepth = 0;
            int curHorizontal = 0;

            for (int i = 0; i < lines.Length; i += 2) {                
                int units = int.Parse(lines[i + 1]);

                switch (lines[i]) {
                    case "forward":
                        curHorizontal += units;
                        curDepth += (curAim * units);
                        break;

                    case "up":
                        units *= -1;
                        goto case "down";

                    case "down":
                        curAim += units;
                        break;
                }
            }

            print($"Final position is Horizontal = {curHorizontal} Depth = {curDepth} Total = {curHorizontal * curDepth}");

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

