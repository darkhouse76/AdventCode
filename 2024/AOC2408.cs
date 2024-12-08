using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace CodeTAF
{
    public class AOC2408 : MonoBehaviour
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

        Dictionary<char, List<(int x, int y)>> antennaPos;
        (int x, int y) maxSize; 
            
        void getAllAntennaLocations(char[,] antennaMap) {
            for (int col = 0; col < maxSize.x; col++) {
                for (int row = 0; row < maxSize.y; row++) {
                    char curAntenna = antennaMap[col, row];
                    if (curAntenna == '.') continue;

                    if ( !antennaPos.ContainsKey(curAntenna) ) {
                        antennaPos.Add(curAntenna, new List<(int x, int y)>());
                    }
                    antennaPos[curAntenna].Add((col, row));                        
                }
            }            
        }
        

        void part1() {

            char[,] antennaMap = AocLib.ParseSimpleCharMap(input);
            maxSize = (antennaMap.GetLength(0), antennaMap.GetLength(1));

            antennaPos = new();
            List<(int x, int y)> antiNodes = new();
            getAllAntennaLocations(antennaMap);

            foreach (KeyValuePair<char,List<(int x, int y)>> antenna in antennaPos) {

                for (int i = 0; i < antenna.Value.Count; i++) {
                    //where at minus the other one
                    var curMainAntenna = antenna.Value[i];

                    for (int j = 0; j < antenna.Value.Count; j++) {

                        if (j == i) { continue; }
                        (int x, int y) antiNodePos = (curMainAntenna.x + (curMainAntenna.x - antenna.Value[j].x), (curMainAntenna.y + (curMainAntenna.y - antenna.Value[j].y)));

                        if (AocLib.Map.IsInBounds(antiNodePos, maxSize) && !antiNodes.Contains(antiNodePos)) { antiNodes.Add(antiNodePos); }
                    }
                }
            }
                print($"Number of antinodes found on map = {antiNodes.Count}");
        }

        void part2() {
            char[,] antennaMap = AocLib.ParseSimpleCharMap(input);
            maxSize = (antennaMap.GetLength(0), antennaMap.GetLength(1));

            antennaPos = new();
            List<(int x, int y)> antiNodes = new();
            getAllAntennaLocations(antennaMap);

            foreach (KeyValuePair<char, List<(int x, int y)>> antenna in antennaPos) {

                for (int i = 0; i < antenna.Value.Count; i++) {
                    //where at minus the other one
                    var curMainAntenna = antenna.Value[i];

                    for (int j = 0; j < antenna.Value.Count; j++) {

                        if (j == i) { continue; }
                        (int x, int y) antiNodeOffset = (curMainAntenna.x - antenna.Value[j].x, curMainAntenna.y - antenna.Value[j].y);
                        (int x, int y) antiNodePos = curMainAntenna;
                        if (!antiNodes.Contains(antiNodePos)) { antiNodes.Add(antiNodePos); }

                        do { 
                            antiNodePos = (antiNodePos.x + antiNodeOffset.x, (antiNodePos.y + antiNodeOffset.y));
                            if (AocLib.Map.IsInBounds(antiNodePos, maxSize) && !antiNodes.Contains(antiNodePos)) { antiNodes.Add(antiNodePos); }
                        } while (AocLib.Map.IsInBounds(antiNodePos, maxSize));
                    }
                }
            }
            print($"Number of antinodes found on map = {antiNodes.Count}");
        }



    }
}

