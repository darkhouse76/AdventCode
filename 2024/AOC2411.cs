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

        [SerializeField, Range(0, 25)]
        int amountBlinks = 25;


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

            


        void part1() {
            //new list every time                       
            List<long> stones = new(AocLib.parseInputToLong(input, " ")); 

            for (int i = 0; i < amountBlinks; i++) {
                stones = blink(stones);
                //print(stones.Count);
            }
            
            print($"Number of stones after {amountBlinks} blinks = {stones.Count}");

        }

        void part2() {


        }



    }
}

