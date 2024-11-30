using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace CodeTAF
{
    public class AOC2108 : MonoBehaviour
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

        enum DisplayPos
        {
            top,
            topLeft,
            topRight,
            middle,
            bottomLeft,
            bottomRight,
            bottom
        }

        void part1() {
            
            int[] targetSizes = new int[] { 2, 3, 4, 7 };
            int targetsFound = 0;

            string[] displayInfos = input.Split(new string[] { "\r\n", " | " }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 1; i < displayInfos.Length; i += 2) {
                
                string[] patterns = displayInfos[i].Split(" ");
                foreach (string pattern in patterns) {
                    //print(pattern.Count());

                    if (targetSizes.Contains(pattern.Count())) { targetsFound++; }
                }
                //print(patterns.Count());
            }

            print($"1, 4, 7, and 8 appears {targetsFound}x times in output Patterns ");
        }


        bool containsAll(string curString, char[] charsToCheck) {

            foreach (char c in charsToCheck) {
                if (!curString.Contains(c)) { return false; }
            }
            return true;
        }

        int getNumberFromCypher(string pattern, string[] cypher) {
            for (int i = 0; i < cypher.Length; i++) {
                if (pattern.Length == cypher[i].Length && containsAll(pattern, cypher[i].ToCharArray())) { return i; }
            }
            return -1;
        }

        void part2() {

        // cdfbe: 5 (can be found last by same count and not 2 or 3) (last)
        // gcdfa: 2 (can be found by checking if same count and doesn't have topLeft) -found
        // fbcad: 3 -found
        // cefabd: 9 --can be found (same count and by does contain all of 1 and not 0)
        // cdfgeb: 6 --can be found (same count and by doesn't contain all of 1 and not 0) (last)
        // cagedb: 0 --can be found (doesn't have middle) -found 
        // acedgfb: 8 -found
        // eafb: 4 -found
        // dab: 7 -found
        // ab: 1 -found
                   

            string[] displayInfos = input.Split(new string[] { "\r\n", " | " }, StringSplitOptions.RemoveEmptyEntries);
            string[] displayPatternsAll = new string[displayInfos.Length / 2];
            string[] displayOutputsAll = new string[displayInfos.Length / 2];

            //print(displayInfos.Length);
            int counter = 0;
            for (int i = 0; i < displayInfos.Length; i += 2) {
                displayPatternsAll[counter] = displayInfos[i];
                displayOutputsAll[counter++] = displayInfos[i+1];
            }

            // with 1 and 7 we can figure out top (0)
            // with 1 and 2 we can figure out RightTop (2) and RightBottom (5)
            // with 3 and knowing top/rightTop/RightBottom you can narrow down what is middle(3)/bottom(6)
            // with 4 and 3 you can figure out middle(3) and LeftTop (1) and 

            int allOutputTotals = 0;

            for (int curDisplay = 0; curDisplay < displayPatternsAll.Length; curDisplay++) {

                string[] displayPatterns = displayPatternsAll[curDisplay].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                string[] displayOutputs = displayOutputsAll[curDisplay].Split(" ", StringSplitOptions.RemoveEmptyEntries);

                char[] cypherDisplayPos = new char[7];
                string[] numCypher = new string[10];

                List<string> unkPatterns = displayPatterns.ToList(); 

                foreach (string pattern in displayPatterns) {
                    switch(pattern.Length) {
                        case 2: //number 1
                            numCypher[1] = pattern; unkPatterns.Remove(pattern); break;
                        case 4: //number 4
                            numCypher[4] = pattern; unkPatterns.Remove(pattern); break;
                        case 3: //number 7
                            numCypher[7] = pattern; unkPatterns.Remove(pattern); break;
                        case 7: //number 8
                            numCypher[8] = pattern; unkPatterns.Remove(pattern); break;
                    }
                }

                //figure out what top is
                for (int i = 0; i < numCypher[7].Length ; i++) {
                    if (!numCypher[1].Contains(numCypher[7][i])) {
                        cypherDisplayPos[(int)DisplayPos.top] = numCypher[7][i];
                    }
                }
                        
                //figure out which pattern is 3
                foreach (string pattern in unkPatterns) {
                    //if count is 5 and has all char of 7 then it is the 3 pattern
                    if (pattern.Length == 5 && containsAll(pattern, numCypher[7].ToCharArray())) {
                        numCypher[3] = pattern;
                        unkPatterns.Remove(pattern);
                        break;
                    }            
                }
                //figure out what topLeft
                foreach (char letter in numCypher[4]) {
                    if (!numCypher[3].Contains(letter)) { cypherDisplayPos[(int)DisplayPos.topLeft] = letter; };
                }
                //figure out what middle
                foreach (char letter in numCypher[4]) {                
                    if (!numCypher[1].Contains(letter) && letter != cypherDisplayPos[(int)DisplayPos.topLeft]) {
                        cypherDisplayPos[(int)DisplayPos.middle] = letter;
                    }
                }
                //find 2
                foreach (string pattern in unkPatterns) {
                    //if same count and doesn't have topLeft then 2
                    if (pattern.Length == 5 && !pattern.Contains(cypherDisplayPos[(int)DisplayPos.topLeft])) {
                        numCypher[2] = pattern;
                        unkPatterns.Remove(pattern);
                        break;
                    }
                }

                //find 0
                foreach (string pattern in unkPatterns) {
                    //if same count and isn't the others   NOT COMPLETE
                    if (pattern.Length == 6 && !pattern.Contains(cypherDisplayPos[(int)DisplayPos.middle])) {
                        numCypher[0] = pattern;
                        unkPatterns.Remove(pattern);
                        break;
                    }
                }

                //find 9 
                foreach (string pattern in unkPatterns) {
                    //same count and by does contain all of 1 and not 0 then 9
                    if (pattern.Length == 6 && containsAll(pattern, numCypher[1].ToCharArray())) {
                        numCypher[9] = pattern;
                        unkPatterns.Remove(pattern);
                        break;
                    }
                }

                //find 5 and 6 by elemnation 
                foreach (string pattern in unkPatterns) {
                    switch (pattern.Length) {
                        case 5:
                            numCypher[5] = pattern; break;
                        case 6:
                            numCypher[6] = pattern; break;
                    }                
                }

                ////////Not end up needed //////
                //figure out what topRight and bottomRight (need to know what is 2)
                //if (numCypher[2].Contains(numCypher[1][0])) {
                //    cypherDisplayPos[(int)DisplayPos.topRight] = numCypher[1][0];
                //    cypherDisplayPos[(int)DisplayPos.bottomRight] = numCypher[1][1];
                //}
                //else {
                //    cypherDisplayPos[(int)DisplayPos.topRight] = numCypher[1][1];
                //    cypherDisplayPos[(int)DisplayPos.bottomRight] = numCypher[1][0];
                //}



                //debug

                //print($"----Display #{curDisplay} ----");

                //for (int i = 0; i < numCypher.Length; i++) {
                //    print($"{i} = {numCypher[i]}");
                //}

                //for (int i = 0; i < displayOutputs.Length; i++) {
                //    print(displayOutputs[i]);
                //}

                int output = 0;
                int numPosMod = 1000;
                for (int i = 0; i < 4; i++) {
                    output += (getNumberFromCypher(displayOutputs[i], numCypher) * numPosMod);
                    numPosMod /= 10;
                }
                //print(output);

                allOutputTotals += output;
            }

            print($"Grand total of all display outputs = {allOutputTotals}");
        
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

