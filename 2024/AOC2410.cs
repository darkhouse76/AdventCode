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
        
        // Key: (Node Position tuple, direction entered with (IE: came from the South to this northern Node. So give it North))
        // Value: number of valid hiking trails from this node. 
        Dictionary<((int x, int y) pos, (int x, int y) dir) , int> cachedNodes;
        int[,] heightMap;

        (int x, int y)[] Directions = AocLib.Map.Directions;



        //Dictionary<(int posX, int posY, int dirX, int dirY), int> cachedNodes;

        int findTrail((int x, int y) startPos, int curDirNum, int prevHeight ) {

            int numCorrectPaths = 0 ;
            if (cachedNodes.TryGetValue((startPos, Directions[curDirNum]), out numCorrectPaths)) {
                return numCorrectPaths;
            }

            int curHeight = heightMap[startPos.x, startPos.y];
            if (curHeight != prevHeight + 1) { return 0; }
            //valid end to a trail
            if (heightMap[startPos.x, startPos.y] == 9) { return 1; }            


            for (int i = 0; i < Directions.Length; i++) {
                if (Directions[i] == AocLib.Map.getOppositeDir(curDirNum)) { continue; }

                if (heightMap[s])


            }


        }

        void part1() {
                       

        }

        void part2() {


        }



    }
}

