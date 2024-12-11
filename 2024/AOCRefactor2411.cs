using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEditor;
using UnityEngine;

namespace CodeTAF
{
    public class AOCRefactor2411 : MonoBehaviour
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

        [SerializeField, Range(0, 1000000)]
        int workSizeMax = 500000;

        [SerializeField, Range(0, 100)]
        int maxNumThreads = 100;


        List<long> blink(List<long> curStones) {
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
            }

            print($"Number of stones after {amountBlinks} blinks = {stones.Count}");

        }

        static readonly object locker = new object();
        static long totalStones = 0;        
        static List<long> remainingStones = new List<long>();
        static List<int> numberOfBlinksLeft = new List<int>();

        List<Thread> threads;

        void part2() {

            //new list every time                       
            List<long> allStones = new(AocLib.parseInputToLong(input, " "));
            //long prevNumStones = 0;

            totalStones = 0;
            remainingStones = new List<long>();
            numberOfBlinksLeft = new List<int>();

            foreach (var stone in allStones) {
                remainingStones.Add(stone);
                numberOfBlinksLeft.Add(amountBlinks);
            }

            //remainingStones.Add(allStones[baseStone]);
            //numberOfBlinksLeft.Add(amountBlinks);
            threads = new();
            for (int i = 0; i < remainingStones.Count - 1; i++) {
                threads.Add(new Thread(doBlinks));
            }
            foreach (var t in threads) {
                t.Start();
            }

            doBlinks();

            bool isRunning = true;

            while (isRunning) {
                foreach (var t in threads) {
                    if (t.IsAlive) { 
                        isRunning = true; 
                        break; 
                    }
                    isRunning = false;
                }
            }
            /*
            int count = 0;
            foreach (var t in threads) {
                print(t.IsAlive);
                print($"{++count} : {t.ThreadState}");
            }
            */            

            print($"{threads.Count} threads used");
            print($"Number of stones after {amountBlinks} blinks = {totalStones}");

        }

        private void doBlinks() {
            while (remainingStones.Count > 0) {
                int remainingBlinks;
                List<long> stones;
                lock (locker) {
                    if (remainingStones.Count == 0) { return; }
                    int lastIndex = remainingStones.Count - 1;
                    //print(lastIndex);
                    stones = new List<long> { remainingStones[lastIndex] };
                    remainingStones.RemoveAt(lastIndex);
                    remainingBlinks = numberOfBlinksLeft[lastIndex];
                    numberOfBlinksLeft.RemoveAt(lastIndex);
                }

                for (int i = remainingBlinks; i > 0; i--) {
                    stones = blink(stones);
                    //if got through all the blinks remaining then count them and move on
                    if (i == 1) {
                        lock (locker) {
                            totalStones += stones.Count;
                        }
                        break;
                    }
                    //if the amount of stones are too big to work. Then store the stones and work with one. 
                    if (stones.Count > workSizeMax) {
                        lock (locker) {
                            foreach (var stone in stones) {
                                remainingStones.Add(stone);
                                numberOfBlinksLeft.Add(i - 1);
                            }
                            if (threads.Count < maxNumThreads) { 
                                Thread newT = new Thread(doBlinks);
                                threads.Add(newT);
                                newT.Start();
                            }
                        }
                        break;
                    }
                }
            }
        }
    }
}
