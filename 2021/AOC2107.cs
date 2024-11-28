using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace CodeTAF
{
    public class AOC2107 : MonoBehaviour
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
                string filePath = $"{InputPath}{Day}test.txt";

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




        (int min, int max, int remainder) getRangeToSearch(int[] startPos, int targetAmtSections) {
            int numOfPos = startPos.Max() + 1;
            int[] amtPos = new int[numOfPos];

            for (int i = 0; i < startPos.Length; i++) {
                amtPos[startPos[i]]++;            
            }

            int remainder = numOfPos % targetAmtSections;

            int sectionSize = numOfPos / targetAmtSections;
            int minRange = 0;
            int maxRange = 0;
            int highCount = 0;
            

            for (int i = 0; i < amtPos.Length; i+= sectionSize) {

                int curCount = 0;
                for (int j = 0; (j < sectionSize && (i + j) < numOfPos); j++) {
                    //print(i + j);
                    curCount += amtPos[i + j];
                }

                if (curCount == highCount) { maxRange = i + (sectionSize - 1); }
                else if (curCount > highCount) { highCount = curCount; minRange = i; maxRange = i + (sectionSize - 1); }
                

                //print("_____");
            }


            //print($"range = {minRange} - {maxRange} for a total of {highCount}");
            //for (int i = 0; i < amtPos.Length; i++) { print($"Pos: {i} has {amtPos[i]} ships"); }

            return (minRange, maxRange, remainder);

        }

        int getFuelCost(int[] startPos, int targetPos, bool isPart2 = false) {
            int fuelCost = 0;
            foreach (int ship in startPos) {
                int amtMoves = Math.Abs(targetPos - ship);
                if (isPart2) {
                    fuelCost += amtMoves * (amtMoves + 1) / 2;
                } 
                else {
                    fuelCost += amtMoves;
                }
                
            }
            return fuelCost;
        }        

        void part1() {

            int[] startPos = AocLib.parseInputToInt(input, ",");

            var range = getRangeToSearch(startPos, 4);
            //print(startPos.Max());
            int numOfPos = startPos.Max() + 1;


            int lowestFuelCost = int.MaxValue;
            int bestPos = 999999;
            for (int i = range.min; i < range.max; i++) {
                int curFuelCost = getFuelCost(startPos, i);
                if (lowestFuelCost > curFuelCost) { lowestFuelCost = curFuelCost; bestPos = i; }                
            }

            for (int i = (numOfPos - range.remainder) ; i < (numOfPos); i++) {
                //print(i);
                int curFuelCost = getFuelCost(startPos, i);
                if (lowestFuelCost > curFuelCost) { lowestFuelCost = curFuelCost; bestPos = i; }
            }

            print($" The lowest Cost was {lowestFuelCost} for position {bestPos}");


        }

        void part2() {
            int[] startPos = AocLib.parseInputToInt(input, ",");

            //var range = getRangeToSearch(startPos, 4);
            //print(startPos.Max());
            int numOfPos = startPos.Max() + 1;


            int lowestFuelCost = int.MaxValue;
            int bestPos = 999999;

            //for (int i = range.min; i < range.max; i++) {
            //    int curFuelCost = getFuelCost(startPos, i, true);
            //    if (lowestFuelCost > curFuelCost) { lowestFuelCost = curFuelCost; bestPos = i; }
            //}

            for (int i = 0; i < (numOfPos); i++) {
                //print(i);
                int curFuelCost = getFuelCost(startPos, i, true);
                if (lowestFuelCost > curFuelCost) { lowestFuelCost = curFuelCost; bestPos = i; }
            }

            print($" The lowest Cost was {lowestFuelCost} for position {bestPos}");

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
    }
}

