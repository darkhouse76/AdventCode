using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;


namespace CodeTAF
{
    public class AOC2402 : MonoBehaviour
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

        bool isSafeReport(int[] levels) {
            bool isIncreasing = (levels[0] < levels[1]);
            for (int i = 1; i < levels.Length; i++) {
                int prevLevel = levels[i - 1];
                int curLevel = levels[i];

                if (prevLevel == curLevel
                    || (prevLevel < curLevel) != isIncreasing
                    || Math.Abs(prevLevel - curLevel) > 3) { return false; }
            }
            return true;
        }          

        void part1() {
            string[] lines = input.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            List<int[]> reports = new();

            for (int i = 0; i < lines.Length; i++) {
                reports.Add(AocLib.parseInputToInt(lines[i], " "));
            }            

            int numSafeReports = reports.ToArray().Count(report => isSafeReport(report));

            ///orginal way I came up with. Then I condense to use the Count method for arrays///
            /*
            int numSafeReports = 0;
            foreach (int[] report in reports) {
                if (isSafeReport(report)) { numSafeReports++; }
            }
            */

            print($"Number of safe reports: {numSafeReports}");


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

