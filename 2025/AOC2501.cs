using System;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace CodeTAF
{
    public class AOC2501 : MonoBehaviour
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
                filePath = (File.Exists(filePath+"2.txt")) ? $"{filePath}2.txt" : $"{filePath}.txt";

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

        int GetRotation(string instruction) {

            if (instruction[..1] == "R") { return 1; }
            return -1;
        }

        (int,int) RotateDial(int dialStartPoint, int ticks, int directionMod = 1) {
            int dialPoint = dialStartPoint;
            int zeroCount = 0;


            //if start point is within -+ of num ticks from 0

            if ((dialStartPoint <= ticks && directionMod == -1) || 
                (dialStartPoint >= 100-ticks && directionMod == 1)) {

                print("ZERO Enocunter");
            } 

            


            dialPoint = dialPoint + (ticks * directionMod);
            zeroCount += Math.Abs((dialPoint < 0) ? dialPoint-100 : dialPoint / 100);
            
            dialPoint %= 100;

            //if (dialPoint > 99) {
            //    zeroCount += dialPoint / 100;
           // } else if (dialPoint < 0) {
             //   zeroCount += Math.Abs(dialPoint / 100);
            //}

            return ((dialPoint < 0) ? 100 + dialPoint : dialPoint, zeroCount);
        }

        void part1() {
            string[] instructions = input.Split("\r\n");
            int dialPoint = 50;
            int password = 0;
            int advPassword = 0;
            int fullTurns;

            for (int i = 0; i < instructions.Length; i++) {
                (dialPoint, fullTurns) = RotateDial(dialPoint, int.Parse(instructions[i][1..]), GetRotation(instructions[i]));

                advPassword += fullTurns;

                if (dialPoint == 0) {
                    password++;
                }                
            }

            print($"The passwords are Part 1: {password} Part 2: {advPassword}");
        }

        void part2() {
            part1();
        }



    }
}

