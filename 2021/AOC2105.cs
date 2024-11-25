using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace CodeTAF
{
    public class AOC2105 : MonoBehaviour
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

        class line {

            public static int maxX;
            public static int maxY;

            int X1 { get; set; }
            int Y1 { get; set; }
            int X2 { get; set; }
            int Y2 { get; set; }
            bool isHorizontalVertical { get; set; }

            public line(int x1, int y1, int x2, int y2) {

                isHorizontalVertical = (x1 == x2 || y1 == y2) ? true : false;

                X1 = x1;
                Y1 = y1;
                X2 = x2;
                Y2 = y2;

                if (x1 > maxX) { maxX =  x1; }
                if (x2 > maxX) { maxX = x2; }

                if (y1 > maxY) { maxY = y1; }
                if (y2 > maxY) { maxY = y2; }  
            }
        }


        void part1() {
            string[] cords = input.Split(new string[] { "\r\n", " -> ", ","}, StringSplitOptions.RemoveEmptyEntries);

            int[] cordsNum = Array.ConvertAll(cords, int.Parse);

            line.maxX = 0;
            line.maxY = 0;
            List<line> allLines = new List<line>();

            for (int i = 0; i < cordsNum.Length; i+= 4) {
                allLines.Add(new line(cordsNum[i], cordsNum[i + 1], cordsNum[i + 2], cordsNum[i + 3]));
            }

            print(allLines.Count);

            print($"Map size =  ({line.maxX} X {line.maxY})");
            
            
            //for (int i = 0; i < cords.Length; i++) { print(cords[i]); }


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

