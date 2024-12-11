using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


namespace CodeTAF
{
    public class AOC2411 : MonoBehaviour
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

        [SerializeField, Range(0, 75)]
        int amountBlinks = 25;

        [SerializeField, Range(0, 8)]
        int baseStone = 0;

        [SerializeField, Range(0, 1000000)]
        int workSizeMax = 500000;


        List<long> blink(List<long> curStones ) {
            List<long> newStones = new List<long>();

            for (int i = 0; i < curStones.Count; i++) {

                //if 0 then make 1
                if (curStones[i] == 0) {
                    newStones.Add(1);    
                    continue; 
                }

                int numDigits = AocLib.numberOfDigits(curStones[i]);
                //if even digits then split
                if (numDigits % 2 == 0) {
                    long div = (long)Math.Pow(10, AocLib.numberOfDigits(curStones[i]) / 2);
                    newStones.Add(curStones[i] / div);
                    newStones.Add(curStones[i] % div);
                    continue;
                }

                //otherwise time 2024
                newStones.Add(curStones[i] * 2024);
            }
            return newStones;
        }

        Dictionary<long,long> blink2(Dictionary<long,long> curStones) {
            Dictionary<long,long> newStones = new Dictionary<long,long>();

            foreach (KeyValuePair<long,long> stone in curStones) {
                //if 0 then make 1
                if (stone.Key == 0) {
                    if (!newStones.TryAdd(1, stone.Value)) {
                        newStones[1] += stone.Value;
                    }                                       
                    continue;
                }

                int numDigits = AocLib.numberOfDigits(stone.Key);
                //if even digits then split
                if (numDigits % 2 == 0) {
                    long div = (long)Math.Pow(10, AocLib.numberOfDigits(stone.Key) / 2);
                    long stone1 = (stone.Key / div);
                    long stone2 = (stone.Key % div);
                    if (!newStones.TryAdd(stone1, stone.Value)) {
                        newStones[stone1] += stone.Value;
                    }
                    if (!newStones.TryAdd(stone2, stone.Value)) {
                        newStones[stone2] += stone.Value;
                    }                                       
                    continue;
                }

                //otherwise time 2024
                if (!newStones.TryAdd((stone.Key * 2024), stone.Value)) {
                    newStones[(stone.Key * 2024)] += stone.Value;
                }
            }            
            return newStones;
        }



        void part1() {
            //new list every time                       
            List<long> stones = new(AocLib.parseInputToLong(input, " "));            
            for (int i = 0; i < amountBlinks; i++) {
                stones = blink(stones);                
            }
            
            print($"Number of stones after {amountBlinks} blinks = {stones.Count}");

        }

        void part2() {
            //new list every time                       
            List<long> allStones = new(AocLib.parseInputToLong(input, " "));
            //long prevNumStones = 0;

            long totalStones = 0;

            Dictionary<long, long> stones = new();

            foreach (var stone in  allStones) {
                stones[stone] = 1;                
            }
            for (int i = 0; i < amountBlinks; i++) {
                stones = blink2(stones);                    
            } 
            
            foreach (var stone in stones) {
                totalStones += stone.Value;
            }

            print($"Number of stones after {amountBlinks} blinks = {totalStones}");

        }

        void part2old() {
            /*
            for (int i = 10; i > 0; i--) {
                print(i);
                if (i == 7) {
                    print("split here = " + (i - 1));
                }
            }
            return;
            */            

            //new list every time                       
            List<long> allStones = new(AocLib.parseInputToLong(input, " "));
            //long prevNumStones = 0;

            long totalStones = 0;
            List<long> remainingStones = new List<long>();
            List<int> numberOfBlinksLeft = new List<int>();

            foreach ( var stone in allStones) { 
                remainingStones.Add(stone);
                numberOfBlinksLeft.Add(amountBlinks);
            }

            //remainingStones.Add(allStones[baseStone]);
            //numberOfBlinksLeft.Add(amountBlinks);

            while (remainingStones.Count > 0 ) {
                int lastIndex = remainingStones.Count - 1;
                List<long> stones = new List<long> { remainingStones[lastIndex] };                
                remainingStones.RemoveAt(lastIndex);
                int remainingBlinks = numberOfBlinksLeft[lastIndex];
                numberOfBlinksLeft.RemoveAt(lastIndex);

                for (int i = remainingBlinks; i > 0; i--) {
                    stones = blink(stones);
                    //if got through all the blinks remaining then count them and move on
                    if ( i == 1 ) { 
                        totalStones += stones.Count; 
                        break; 
                    }
                    //if the amount of stones are too big to work. Then store the stones and work with one. 
                    if ( stones.Count > workSizeMax) {
                        foreach (var stone in stones) {
                            remainingStones.Add(stone);
                            numberOfBlinksLeft.Add(i - 1);
                        }
                        break;
                    }                    
                }                
            }         
            

            print($"Number of stones after {amountBlinks} blinks = {totalStones}");

        }



    }
}

