using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;


namespace CodeTAF
{
    public class AOC2109 : MonoBehaviour
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

            char[,] heightmap = AocLib.ParseSimpleCharMap(input);
            AocLib.Print2d(heightmap, true);

            //print(heightmap[1, 1]);

            (int x, int y)[] directions = new [] { (0,1), (1,0), (0,-1), (-1,0) };

            (int x, int y) maxSize = (heightmap.GetLength(0),  heightmap.GetLength(1));
            int riskLevel = 0;            

            for (int col = 0; col < maxSize.x; col++) {
                for (int row = 0; row < maxSize.y; row++) {
                    int selectedHeight = int.Parse( heightmap[col, row].ToString() );
                    bool isLowPoint = true;
                    int equalCount = 0;
                    int checkedDir = 0;
                    foreach (var dir in directions) {                        
                        (int x, int y) checkDir = (col + dir.x, row + dir.y);

                        if ((checkDir.x < 0 || checkDir.x >= maxSize.x) || (checkDir.y < 0 || checkDir.y >= maxSize.y)) { continue; }
                        int checkHeight = int.Parse(heightmap[checkDir.x, checkDir.y].ToString());
                        checkedDir++;
                        if (selectedHeight > checkHeight) { isLowPoint = false; break; }
                        if (selectedHeight == checkHeight) { equalCount++; };

                    }

                    //if not ruled out as low point and all checked dir were equal to the selected height then it's a low point. 
                    if (isLowPoint && equalCount != checkedDir) { 
                        print($"at ({col},{row}) is low point: {selectedHeight}"); 
                        riskLevel += selectedHeight + 1; 
                    }

                }
            }

            print($"Risk Level for all points: {riskLevel}");



        }

        int[,] heightmap;
        (int x, int y) maxSize;
        bool[,] Checked;
        (int x, int y)[] directions = new[] { (0, 1), (1, 0), (0, -1), (-1, 0) };

        int checkPos((int x,int y) targetPos, (int x, int y) prevDir) {
            int basinCount = 0;
            for (int curDir = 0; curDir < directions.Length; curDir++) {
                if (directions[curDir] == prevDir) { continue; }

                (int x, int y) checkDir = (targetPos.x + directions[curDir].x, targetPos.y + directions[curDir].y);
                if ((checkDir.x < 0 || checkDir.x >= maxSize.x) || (checkDir.y < 0 || checkDir.y >= maxSize.y)) { continue; }

                if (!Checked[checkDir.x,checkDir.y] && heightmap[checkDir.x,checkDir.y] < 9) {
                    Checked[checkDir.x,checkDir.y] = true;
                    basinCount += 1 + checkPos(checkDir, (directions[curDir].x * -1, directions[curDir].y * -1));
                }

            }

            return basinCount;
        }

        void part2() { 

            char[,] heightmapChar = AocLib.ParseSimpleCharMap(input);
            maxSize = (heightmapChar.GetLength(0), heightmapChar.GetLength(1));
            heightmap = new int[maxSize.x,maxSize.y];

            for (int col = 0; col < heightmapChar.GetLength(0); col++) {
                for (int row = 0; row < heightmapChar.GetLength(1); row++) {
                    heightmap[col,row] = int.Parse(heightmapChar[col,row].ToString());
                }
            }
            //int[,] heightmap = Array.ConvertAll<char[,],int[,]>(heightmapChar, c => (int)Char.GetNumericValue(c));
            
            AocLib.Print2d(heightmapChar, true);
            

            List<(int x, int y)> lowPoints = new();            
            Checked = AocLib.SetAllValues(new bool[maxSize.x, maxSize.y], false);

            //find all lowPoints
            for (int col = 0; col < maxSize.x; col++) {
                for (int row = 0; row < maxSize.y; row++) {
                    int selectedHeight = heightmap[col, row];
                    bool isLowPoint = true;
                    int equalCount = 0;
                    int checkedDir = 0;
                    foreach (var dir in directions) {
                        (int x, int y) checkDir = (col + dir.x, row + dir.y);

                        if ((checkDir.x < 0 || checkDir.x >= maxSize.x) || (checkDir.y < 0 || checkDir.y >= maxSize.y)) { continue; }
                        int checkHeight = heightmap[checkDir.x, checkDir.y];
                        checkedDir++;
                        if (selectedHeight > checkHeight) { isLowPoint = false; break; }
                        if (selectedHeight == checkHeight) { equalCount++; };

                    }

                    //if not ruled out as low point and all checked dir were equal to the selected height then it's a low point. 
                    if (isLowPoint && equalCount != checkedDir) {
                        print($"at ({col},{row}) is low point: {selectedHeight}");
                        lowPoints.Add((col, row));
                    }

                }
            }

            //get number of basin points 
            foreach (var point in  lowPoints) {
                print(checkPos(point, (0, 0)) + 1);
            }
            

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

