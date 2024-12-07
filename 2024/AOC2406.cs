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
        //Dictionary<(int x, int y), (int x, int y)> path;
        Dictionary<(int x, int y), List<(int x, int y)>> path2;
        Dictionary<(int x, int y), (int x, int y)> loopTurns;


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
        
        //probably not needed
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

        //refactor
        bool walk3() {
            var targetPos = AocLib.Map.MoveForward(curPos, curDir);
            if (!AocLib.Map.IsInBounds(targetPos, maxSize)) {
                return false;
            }

            if (labMap[targetPos.x, targetPos.y] == '#') {
                curDir = AocLib.Map.TurnRight(curDir);
                if (!path2[curPos].Contains(curDir)) { path2[curPos].Add(curDir); }
                walk3();
            }
            else {
                curPos = (targetPos.x, targetPos.y);
                if (!path2.ContainsKey(curPos)) {
                    path2.Add(curPos, new List<(int x, int y)>());
                }
                if (!path2[curPos].Contains(curDir)) { path2[curPos].Add(curDir); }                
            }
            return true;
        }        

        //trying to refactor
        bool checkForLoopFromStart((int x, int y) testFrom, (int x, int y) testDir) {           

            (int x, int y) pathDir;
            var testPos = testFrom; //might not be needed
            
            var nextPos = AocLib.Map.MoveForward(testPos, testDir);

            //go until we hit a obj or outof bounds.
            while (AocLib.Map.IsInBounds(nextPos, maxSize) && labTestMap[nextPos.x, nextPos.y] != '#') {
                if (loopTurns.TryGetValue(nextPos, out pathDir) && testDir == pathDir) { return true; }
                testPos = nextPos;
                nextPos = AocLib.Map.MoveForward(testPos, testDir);
            }
            if (loopTurns.TryGetValue(testPos, out pathDir) && testDir == pathDir) { return true; }
            loopTurns.TryAdd(testPos, testDir);
            
            
            return AocLib.Map.IsInBounds(nextPos, maxSize) && checkForLoopFromStart(testPos, AocLib.Map.TurnRight(testDir));
        }
              

        void part1() {
            labMap = AocLib.ParseSimpleCharMap(input);
            maxSize = (labMap.GetLength(0), labMap.GetLength(1));
            walkedOn = AocLib.SetAllValues(new bool[maxSize.x, maxSize.y], false);
            curPos = AocLib.Map.Find(labMap,'^');
            curDir = AocLib.Map.Directions[AocLib.Map.UP];
            totalWalkAmt = 0;

            path2 = new();

            while (walk3()) { continue; }

            totalWalkAmt = path2.Count;

            print($"Guard walked total of {totalWalkAmt} distinct positions");

        }

        void part2() {
            labMap = AocLib.ParseSimpleCharMap(input);
            maxSize = (labMap.GetLength(0), labMap.GetLength(1));
            walkedOn = AocLib.SetAllValues(new bool[maxSize.x, maxSize.y], false); //might not be needed
            curPos = AocLib.Map.Find(labMap, '^');
            curDir = AocLib.Map.Directions[AocLib.Map.UP];
            startPos = (curPos.x, curPos.y);
            totalLoops = 0;
            validObjLoop = new();
            path2 = new();      

            


            path2.Add(startPos, new());
            path2[startPos].Add(curDir);

            while (walk3()) { continue; };

            foreach (KeyValuePair<(int x, int y),List<(int x, int y)>> kv in path2) {
                foreach (var dir in kv.Value) {
                    var fakeObjPos = AocLib.Map.MoveForward(kv.Key, dir);
                    //if startPos or already used in a validLoop or already a obj or out of bounds pos
                    //then disregard can't be new pos.
                    if (fakeObjPos == startPos || 
                        validObjLoop.Contains(fakeObjPos) || 
                        !AocLib.Map.IsInBounds(fakeObjPos, maxSize) ||
                        labMap[fakeObjPos.x,fakeObjPos.y] == '#') 
                    { 
                        continue; 
                    }
                    //reset the labTestMap
                    labTestMap = (char[,])labMap.Clone();
                    //add the fake obj to the "test" labMap
                    labTestMap[fakeObjPos.x, fakeObjPos.y] = '#';                
                    loopTurns = new();                

                    //start the check from the start
                    if (checkForLoopFromStart(startPos, AocLib.Map.Directions[AocLib.Map.UP])) {                    
                        totalLoops++;
                        validObjLoop.Add(fakeObjPos);                    
                    }
                }
            }            

            //foreach (var fakeObj in validObjLoop) { print($"FakeObj = {fakeObj}"); }

            print($"Total possible loops: {totalLoops}");
        }

    }
}

