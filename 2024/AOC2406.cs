using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace CodeTAF{
    
    public class AOC2406 : MonoBehaviour
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

        
        char[,] labMap;
        char[,] labTestMap;
        (int x, int y) maxSize;
        bool[,] walkedOn;
        (int x, int y) startPos;
        (int x, int y) curPos;
        (int x, int y) curDir;
        int totalWalkAmt;
        int totalLoops;
        List<(int x, int y)> validObjLoop;
        
        //key is pos and value is the dir
        Dictionary<(int x, int y), (int x, int y)> path;

        
        //if I didn't know the start dir...but i do
        (int x, int y) getStartPos(out (int x, int y) curDir) {
            var startIndicators = new char[] { '^', '<', 'v', '>' };
            curDir = default;

            for (int i = 0; i < startIndicators.Length; i++) {
                
                if (AocLib.Map.TryFind(labMap, startIndicators[i], out var startPos)) {
                    curDir = AocLib.Map.Directions[i];
                    return startPos; 
                }                
            }
            return default;
        }
        
        
        bool walk() {
            var targetPos = AocLib.Map.MoveForward(curPos, curDir);
            if (!AocLib.Map.IsInBounds(targetPos, maxSize)) {
                return false;
            }

            if (labMap[targetPos.x,targetPos.y] == '#') { 
                curDir = AocLib.Map.TurnRight(curDir); 
                walk(); 
            } else {
                curPos = (targetPos.x,targetPos.y);
                if (!walkedOn[curPos.x,curPos.y]) { walkedOn[curPos.x,curPos.y] = true; totalWalkAmt++; }
            }
            return true;            
        }

        bool walk2() {
            var targetPos = AocLib.Map.MoveForward(curPos, curDir);
            if (!AocLib.Map.IsInBounds(targetPos, maxSize)) {
                return false;
            }

            if (labMap[targetPos.x, targetPos.y] == '#') {
                curDir = AocLib.Map.TurnRight(curDir);
                walk2();
            }
            else {
                curPos = (targetPos.x, targetPos.y);
                if (!walkedOn[curPos.x, curPos.y]) {
                    walkedOn[curPos.x, curPos.y] = true;
                    path.Add(curPos, curDir);
                }

                var fakeObjPos = AocLib.Map.MoveForward(curPos, curDir);                
                if (AocLib.Map.IsInBounds(fakeObjPos,maxSize)) {
                    labTestMap = (char[,])labMap.Clone();
                    //add the fake obj to the "test" labMap
                    labTestMap[fakeObjPos.x, fakeObjPos.y] = '#';
                    if (checkForLoop(curPos, curDir, new List<(int x, int y)>() { curPos }) ) {
                        if (!validObjLoop.Contains(fakeObjPos)) {
                            totalLoops++;
                            validObjLoop.Add(fakeObjPos);
                        }
                    }
                }
            }
            return true;
        }

        bool checkForLoop((int x, int y) testFrom, (int x, int y) testDir, List<(int x, int y)> loopPos) {
            if (AocLib.Map.MoveForward(testFrom, testDir) == startPos) { return false; }
            testDir = AocLib.Map.TurnRight(testDir);

            (int x, int y) pathDir;
            if (path.TryGetValue(testFrom, out pathDir) && testDir == pathDir) { return true; }

            var testPos = testFrom;
            var nextPos = AocLib.Map.MoveForward(testPos, testDir);
            //go until we find prev path going the same way or we hit a obj
            while (AocLib.Map.IsInBounds(nextPos, maxSize) && labTestMap[nextPos.x, nextPos.y] != '#') {
                if ((path.TryGetValue(nextPos, out pathDir) && testDir == pathDir) || loopPos.Contains(nextPos)) { return true; }
                testPos = nextPos;
                nextPos = AocLib.Map.MoveForward(testPos, testDir);
            }
            loopPos.Add(testPos);
            return AocLib.Map.IsInBounds(nextPos, maxSize) && checkForLoop(testPos, testDir, loopPos); 
        }

        bool checkForLoop(List<(int x, int y)> allObjs) {
            //might need to check if a obj is already in the "new" obj location
            if (AocLib.Map.MoveForward(curPos, curDir) == startPos) { return false; }

            return (AocLib.Map.TurnRight(curDir) == path[curPos]);
            /*
            var testDir = AocLib.Map.TurnRight(curDir);
            if (testDir.x == 0 && testDir.y > 0) { return allObjs.Exists(objPos => (objPos.x == curPos.x && objPos.y > curPos.y)); }
            if (testDir.x == 0 && testDir.y < 0) { return allObjs.Exists(objPos => (objPos.x == curPos.x && objPos.y < curPos.y)); }
            if (testDir.y == 0 && testDir.x > 0) { return allObjs.Exists(objPos => (objPos.y == curPos.y && objPos.x > curPos.x)); }

            return allObjs.Exists(objPos => (objPos.y == curPos.y && objPos.x < curPos.x)); }
            */
        }

        void part1() {
            labMap = AocLib.ParseSimpleCharMap(input);
            maxSize = (labMap.GetLength(0), labMap.GetLength(1));
            walkedOn = AocLib.SetAllValues(new bool[maxSize.x, maxSize.y], false);
            curPos = AocLib.Map.Find(labMap,'^');
            curDir = AocLib.Map.Directions[AocLib.Map.UP];
            totalWalkAmt = 0;


            while (walk()) { continue; }

            print($"Guard walked total of {totalWalkAmt} distinct positions");

        }

        void part2() {
            labMap = AocLib.ParseSimpleCharMap(input);
            maxSize = (labMap.GetLength(0), labMap.GetLength(1));
            walkedOn = AocLib.SetAllValues(new bool[maxSize.x, maxSize.y], false);
            curPos = AocLib.Map.Find(labMap, '^');
            curDir = AocLib.Map.Directions[AocLib.Map.UP];
            startPos = (curPos.x, curPos.y);
            totalLoops = 0;
            validObjLoop = new();

            path = new();

            List<(int x, int y)> allObjs = AocLib.Map.FindAll(labMap, '#');

            walkedOn[startPos.x, startPos.y] = true;
            path.Add(startPos, curDir);

            while (walk2()) { continue; };

            print($"Total possible loops: {totalLoops}");
        }

    }
}

