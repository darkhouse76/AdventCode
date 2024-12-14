using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;


namespace CodeTAF
{
    public class AOC2413 : MonoBehaviour
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

        class clawMachine {
            (int x, int y) buttonA;
            (int x, int y) buttonB;
            (int x, int y) prizePos;
            int buttonACost = 3;
            int buttonBCost = 1;

            public clawMachine((int x, int y) buttonA, (int x, int y) buttonB, (int x, int y) prizePos) {
                this.buttonA = buttonA;
                this.buttonB = buttonB;
                this.prizePos = prizePos;                
            }

            public int getMinimalTokens() {
                (bool x, bool y) isCostEffective = (buttonA.x > (buttonB.x * 3), buttonA.y > (buttonB.y * 3));
                

                if (isCostEffective.x) {
                    int pushesOfB;
                    int startingAmt = prizePos.x / buttonA.x;
                    startingAmt = (startingAmt > 100) ? 100 : startingAmt;

                    for (int pushesOfA = startingAmt; pushesOfA >= 0; pushesOfA-- ) {
                        int xPos = buttonA.x * pushesOfA;
                        int yPos = buttonA.y * pushesOfA;

                        pushesOfB = (prizePos.y - yPos) / buttonB.y;
                        if (pushesOfB > 100) { return 0; } //might need to watch that this is creating too early out

                        if (((prizePos.y - yPos) % buttonB.y) == 0 && (xPos + (buttonB.x * pushesOfB) == prizePos.x)) {
                            //found winning solution....should be the cheapest.....
                            return (pushesOfA * buttonACost) + (pushesOfB * buttonBCost);                            
                        }
                    }

                } else if (isCostEffective.y) {
                    int pushesOfB;
                    int startingAmt = prizePos.y / buttonA.y;
                    startingAmt = (startingAmt > 100) ? 100 : startingAmt;

                    for (int pushesOfA = startingAmt; pushesOfA >= 0; pushesOfA--) {
                        int xPos = buttonA.x * pushesOfA;
                        int yPos = buttonA.y * pushesOfA;

                        pushesOfB = (prizePos.x - xPos) / buttonB.x;
                        if (pushesOfB > 100) { return 0; } //might need to watch that this is creating too early out

                        if (((prizePos.x - xPos) % buttonB.x) == 0 && (yPos + (buttonB.y * pushesOfB) == prizePos.y)) {
                            //found winning solution....should be the cheapest.....
                            return (pushesOfA * buttonACost) + (pushesOfB * buttonBCost);
                        }
                    }
                } else {
                    int pushesOfA;
                    int startingAmt = prizePos.x / buttonB.x;
                    startingAmt = (startingAmt > 100) ? 100 : startingAmt;                    

                    for (int pushesOfB = startingAmt; pushesOfB >= 0; pushesOfB--) {
                        int xPos = buttonB.x * pushesOfB;
                        int yPos = buttonB.y * pushesOfB;

                        pushesOfA = (prizePos.y - yPos) / buttonA.y;
                        //if (pushesOfA > 100) { return 0; } //might need to watch that this is creating too early out

                        if (((prizePos.y - yPos) % buttonA.y) == 0 && (xPos + (buttonA.x * pushesOfA) == prizePos.x)) {
                            //found winning solution....should be the cheapest.....
                            return (pushesOfA * buttonACost) + (pushesOfB * buttonBCost);
                        }
                    }                    
                }
                return 0;
            }


        }

        void part1() {

            var numbersStr = Regex.Matches(input, @"\d+");
            List<int> allNumbers = new();
            List<clawMachine> allMachines = new();            

            //this feel sloppy to parse but here I am lol. 
            foreach (Match numbers in numbersStr) {
                allNumbers.Add(int.Parse(numbers.Value));
            }

            for (int i = 0; i <  allNumbers.Count; i += 6) {
                var buttonASetting = (allNumbers[i], allNumbers[i+1]);
                var buttonBSetting = (allNumbers[i+2], allNumbers[i+3]);
                var prizePos = (allNumbers[i+4], allNumbers[i+5]);

                allMachines.Add(new clawMachine(buttonASetting, buttonBSetting, prizePos));
            }            

            int totalTokenMinimal = 0;

            foreach (var machine in allMachines) {
                totalTokenMinimal += machine.getMinimalTokens();                
            }

            print($"Minimal amount of tokens need = {totalTokenMinimal}");

        }

        void part2() {


        }



    }
}

