using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;


namespace CodeTAF
{
    public class AOC2403 : MonoBehaviour
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
                string filePath = $"{InputPath}{Day}test";
                filePath = (File.Exists(filePath + "2.txt")) ? $"{filePath}2.txt" : $"{filePath}.txt";

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

            //var house = Regex.Matches(input, @"mul\(\d+,\d+\)");
            //var house = Regex.Matches(input, @"(?<=mul\()\d+(?=,\d+\)) || (?<=mul\(\d+,)\d+(?=\))");

            var validNumbersStr = Regex.Matches(input, @"((?<=mul\()\d+(?=,\d+\)))|((?<=mul\(\d+,)\d+(?=\)))");
            

            List<int> validNumbers = new List<int>();
            
            foreach(Match match in validNumbersStr) {
                //print(match.Value);
                validNumbers.Add(int.Parse(match.Value));
            }
            //print("-------");
            int totalResult = 0;
            for (int i = 0; i < validNumbers.Count; i+= 2) {

                totalResult += validNumbers[i] * validNumbers[i + 1];
            }
            print($"Total of all the multiplications: {totalResult}");
        }

        void part2() { 

            var validNumbersStr = Regex.Matches(input, @"((?<=mul\()\d+(?=,\d+\)))|((?<=mul\(\d+,)\d+(?=\)))|(do\(\))|(don't\(\))");

            bool shouldMul = true;
            List<int> validNumbers = new List<int>();

            foreach (Match match in validNumbersStr) {
                //print(match.Value);
                switch (match.Value) {
                    case "do()":
                        shouldMul = true;
                        continue;
                    case "don't()":
                        shouldMul = false;
                        continue;
                    default:
                        if (shouldMul) { validNumbers.Add(int.Parse(match.Value)); }                        
                        break;
                }  
            }

            //print(validNumbers.Count);

            int totalResult = 0;
            for (int i = 0; i < validNumbers.Count; i += 2) {
                //print($"({validNumbers[i]}*{validNumbers[i+1]})");
                totalResult += validNumbers[i] * validNumbers[i + 1];
            }
            print($"Total of all the multiplications: {totalResult}");

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

