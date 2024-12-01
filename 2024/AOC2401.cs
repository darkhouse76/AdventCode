using System;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace CodeTAF
{
    public class AOC2401 : MonoBehaviour
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
            string[] lines = input.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            int[] leftList = new int[lines.Length];
            int[] rightList = new int[lines.Length];

            for (int i = 0; i < lines.Length; i++) {
                string[] sides = lines[i].Split("   ");
                leftList[i] = int.Parse(sides[0]);
                rightList[i] = int.Parse(sides[1]);
            }
            
            Array.Sort(leftList);
            Array.Sort(rightList);
            int distanceDiff = 0;

            for (int i = 0;i < leftList.Length;i++) {
                distanceDiff += Math.Abs(leftList[i] - rightList[i]);
            }

            print($"total of all distances: {distanceDiff}");


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

