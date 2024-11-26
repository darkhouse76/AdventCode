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

            public int X1 { get; set; }
            public int Y1 { get; set; }
            public int X2 { get; set; }
            public int Y2 { get; set; }
            public int AmtSteps { get; set; }
            public bool IsHorizontalVertical { get; set; }

            public line(int x1, int y1, int x2, int y2) {

                IsHorizontalVertical = (x1 == x2 || y1 == y2);

                X1 = x1;
                Y1 = y1;
                X2 = x2;
                Y2 = y2;

                AmtSteps = (x1 == x2) ? Math.Abs(y2 - y1) : Math.Abs(x2 - x1);

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

            //print(allLines.Count);
            //print($"Map size =  ({line.maxX+1} X {line.maxY+1})");
            
            int[,] mainMap = new int[line.maxX+1, line.maxY+1];

            mainMap = AocLib.SetAllValues(mainMap, 0);            

            foreach (line curLine in allLines) {
                if (curLine.IsHorizontalVertical) {
                    if (curLine.X1 == curLine.X2) {                        
                        for (int i = Math.Min(curLine.Y1, curLine.Y2);i <= Math.Max(curLine.Y1, curLine.Y2);i++) {                            
                            mainMap[curLine.X1, i]++;
                        }
                    }
                    else if (curLine.Y1 == curLine.Y2) {
                        for (int i = Math.Min(curLine.X1, curLine.X2); i <= Math.Max(curLine.X1, curLine.X2); i++) {                            
                            mainMap[i, curLine.Y1]++;
                        }
                    }
                }
            }

            int totalOverlap = 0;
            for (int col =  0; col < mainMap.GetLength(0); col++) {
                for (int row = 0; row < mainMap.GetLength(1); row++) {
                    if (mainMap[row,col] > 1) { totalOverlap++; }
                }
            }

            AocLib.Print2d(mainMap, true);

            print($"Total points that overlap {totalOverlap}"); 

        }

        void part2() {
            string[] cords = input.Split(new string[] { "\r\n", " -> ", "," }, StringSplitOptions.RemoveEmptyEntries);

            int[] cordsNum = Array.ConvertAll(cords, int.Parse);

            line.maxX = 0;
            line.maxY = 0;
            List<line> allLines = new List<line>();

            for (int i = 0; i < cordsNum.Length; i += 4) {
                allLines.Add(new line(cordsNum[i], cordsNum[i + 1], cordsNum[i + 2], cordsNum[i + 3]));
            }

            //print(allLines.Count);
            //print($"Map size =  ({line.maxX+1} X {line.maxY+1})");

            int[,] mainMap = new int[line.maxX + 1, line.maxY + 1];

            mainMap = AocLib.SetAllValues(mainMap, 0);

            foreach (line curLine in allLines) {
                if (curLine.IsHorizontalVertical) {
                    if (curLine.X1 == curLine.X2) {
                        for (int i = Math.Min(curLine.Y1, curLine.Y2); i <= Math.Max(curLine.Y1, curLine.Y2); i++) {
                            mainMap[curLine.X1, i]++;
                        }
                    }
                    else if (curLine.Y1 == curLine.Y2) {
                        for (int i = Math.Min(curLine.X1, curLine.X2); i <= Math.Max(curLine.X1, curLine.X2); i++) {
                            mainMap[i, curLine.Y1]++;
                        }
                    }
                }
                else {

                    int xDir = (curLine.X1 < curLine.X2) ? 1 : -1; 
                    int yDir = (curLine.Y1 < curLine.Y2) ? 1 : -1;

                    for (int i = 0; i <= curLine.AmtSteps; i++) {
                        mainMap[(curLine.X1 + (xDir * i)), (curLine.Y1 + (yDir * i))]++;
                    }
                }
            }

            int totalOverlap = 0;
            for (int col = 0; col < mainMap.GetLength(0); col++) {
                for (int row = 0; row < mainMap.GetLength(1); row++) {
                    if (mainMap[row, col] > 1) { totalOverlap++; }
                }
            }

            //AocLib.Print2d(mainMap, true);

            print($"Total points that overlap {totalOverlap}");

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

