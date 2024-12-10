using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace CodeTAF
{
    public class AOC2410 : MonoBehaviour
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
        
        int[,] heightMap;
        (int x, int y) maxSize;
        (int x, int y)[] Directions = AocLib.Map.Directions;

        int findTrail((int x, int y) startPos, List<(int x, int y)> trailNodes, int curDirNum = -1, int prevHeight = -1 ) {
            
            (int x, int y) curDir = (curDirNum == -1) ? (0,0) : Directions[curDirNum];
            int curHeight = heightMap[startPos.x, startPos.y];            
            int numCorrectPaths = 0 ;
            
            (int score, List<(int x, int y)> nodes) result;
            //check if valid next step in the path
            if (curHeight != prevHeight + 1) { return 0; }         
           
            //valid end to a trail
            if (heightMap[startPos.x, startPos.y] == 9) {
                if (trailNodes.Contains(startPos)) { return 0; }
                trailNodes.Add(startPos);
                return 1; 
            }            

            //check all other directions to see if path continues
            for (int i = 0; i < Directions.Length; i++) {
                if (Directions[i] == AocLib.Map.getOppositeDir(curDir)) { continue; }
                (int x, int y) nextPos = AocLib.Map.MoveForward(startPos, Directions[i]);

                if (AocLib.Map.IsInBounds(nextPos,maxSize)) {
                    int score = findTrail(nextPos, trailNodes, i, curHeight);                    
                    numCorrectPaths += score;                    
                }
            }
            return numCorrectPaths;
        }

        void part1() {            
            heightMap = AocLib.ParseSimpleIntMap(input);
            maxSize = (heightMap.GetLength(0), heightMap.GetLength(1));            

            int trailScoreTotals = 0;
            for (int row = 0; row < maxSize.y; row++) {
                for (int col = 0; col < maxSize.x; col++) {
                    if (heightMap[col,row] != 0) { continue; }
                    var result = findTrail((col, row), new List<(int x, int y)>());
                    trailScoreTotals += result;                    
                }
            }

            print($"Total of trail scores = {trailScoreTotals}");

        }

        void part2() {


        }



    }
}

