using System;
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
        (int x, int y) maxSize;
        bool[,] walkedOn;
        (int x, int y) curPos;
        (int x, int y) curDir;
        int totalWalkAmt = 0;

        
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
                curPos = targetPos;
                if (!walkedOn[curPos.x,curPos.y]) { walkedOn[curPos.x,curPos.y] = true; totalWalkAmt++; }
            }
            return true;            
        }


        void part1() {
            labMap = AocLib.ParseSimpleCharMap(input);
            maxSize = (labMap.GetLength(0), labMap.GetLength(1));
            walkedOn = AocLib.SetAllValues(new bool[maxSize.x, maxSize.y], false);
            curPos = AocLib.Map.Find(labMap,'^');
            curDir = AocLib.Map.Directions[AocLib.Map.UP];
            
            while (walk()) { continue; }

            print($"Guard walked total of {totalWalkAmt} distinct positions");

        }

        void part2() {
            labMap = AocLib.ParseSimpleCharMap(input);
            maxSize = (labMap.GetLength(0), labMap.GetLength(1));
        }



    }
}

