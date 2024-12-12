using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace CodeTAF
{
    public class AOC2412 : MonoBehaviour
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

        /////////////////////////////////////////////////////////////////
        /// Everything above is for unity and getting the input files ///
        /////////////////////////////////////////////////////////////////

        char[,] garden;
        (int x, int y) maxSize;
        (int x, int y)[] Directions = AocLib.Map.Directions;
        List<(int x, int y)> allRegionPosChecked;
        List<(int x, int y)> region;
        List<((int x, int y), (int x, int y))> sides;
        //int[,] debugParimeter;


        int findRegion((int x, int y) startPos) {
            char targetPlant = garden[startPos.x, startPos.y];
            int perimeter = 0;

            region.Add(startPos);

            for (int i = 0; i < Directions.Length; i++) {
                (int x, int y) nextPos = AocLib.Map.MoveForward(startPos, Directions[i]);
                if (region.Contains(nextPos)) { continue; }

                if (!AocLib.Map.IsInBounds(nextPos, maxSize) || garden[nextPos.x, nextPos.y] != targetPlant) {
                    perimeter++;                    
                } else {
                    perimeter += findRegion(nextPos);
                } 
            }
            //debugParimeter[startPos.x,startPos.y] = perimeter;
            return perimeter;
        }

        int findRegion2((int x, int y) startPos) {
            char targetPlant = garden[startPos.x, startPos.y];
            int perimeter = 0;

            region.Add(startPos);

            for (int i = 0; i < Directions.Length; i++) {
                (int x, int y) nextPos = AocLib.Map.MoveForward(startPos, Directions[i]);
                if (region.Contains(nextPos)) { continue; }

                if (!AocLib.Map.IsInBounds(nextPos, maxSize) || garden[nextPos.x, nextPos.y] != targetPlant) {
                    if (sides.Contains((nextPos, Directions[i]))) { continue; }

                    perimeter++;
                    findSide(startPos, Directions[i]);                    
                }
                else {
                    perimeter += findRegion2(nextPos);
                }
            }
            //debugParimeter[startPos.x,startPos.y] = perimeter;
            return perimeter;
        }

        void findSide((int x, int y) startPos, (int x, int y) curDir) {
            char targetPlant = garden[startPos.x, startPos.y];
            (int x, int y) searchDir = AocLib.Map.TurnRight(curDir);
            (int x, int y) curPos = startPos;
            (int x, int y) sidePos = AocLib.Map.MoveForward(curPos, curDir);

            for (int i = 0; i < 2; i++) {
                do {
                    sides.Add((sidePos, curDir));
                    curPos = AocLib.Map.MoveForward(curPos, searchDir);
                    sidePos = AocLib.Map.MoveForward(curPos, curDir);
                } while (AocLib.Map.IsInBounds(curPos, maxSize) && garden[curPos.x, curPos.y] == targetPlant && 
                        (!AocLib.Map.IsInBounds(sidePos,maxSize) || garden[sidePos.x, sidePos.y] != targetPlant));
                searchDir = AocLib.Map.TurnLeft(curDir);
                curPos = startPos;
            }            
        }


        void part1() {
            garden = AocLib.ParseSimpleCharMap(input);
            maxSize = (garden.GetLength(0),  garden.GetLength(1));
            allRegionPosChecked = new();
            //debugParimeter = new int[maxSize.x,maxSize.y];
            int priceTotal = 0;

            for (int row = 0; row < maxSize.y; row++) {
                for (int col = 0; col < maxSize.x; col++) {
                    if (allRegionPosChecked.Contains((col,row))) {  continue; }

                    region = new();
                    int perimeter = findRegion((col,row));
                    //print($"{garden[col,row]} : {region.Count} X {perimeter} = {perimeter * region.Count}");
                    //price  = perimeter * area
                    priceTotal += (perimeter * region.Count);
                    allRegionPosChecked.AddRange(region);
                }
            }

            print($"Total Cost of fencing for part 1 = {priceTotal}");
            //AocLib.Print2d(debugParimeter);
        }

        void part2() {
            garden = AocLib.ParseSimpleCharMap(input);
            maxSize = (garden.GetLength(0), garden.GetLength(1));
            allRegionPosChecked = new();
            


            int priceTotal = 0;

            for (int row = 0; row < maxSize.y; row++) {
                for (int col = 0; col < maxSize.x; col++) {
                    if (allRegionPosChecked.Contains((col, row))) { continue; }
                    region = new();
                    sides = new();
                    int perimeter = findRegion2((col, row));
                    //print($"{garden[col,row]} : {region.Count} X {perimeter} = {perimeter * region.Count}");
                    //price  = perimeter * area
                    priceTotal += (perimeter * region.Count);
                    allRegionPosChecked.AddRange(region);
                }
            }

            print($"Total Cost of fencing for part 2 = {priceTotal}");            

        }



    }
}

