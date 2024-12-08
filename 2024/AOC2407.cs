using System;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace CodeTAF
{
    public class AOC2407 : MonoBehaviour
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

        bool checkForValidOps(string curOps, (int depth, long answer, int[] equationNums) target ) {

            if (curOps.Length == target.depth) {
                long answer = (curOps[0] == '*')? (target.equationNums[0] * target.equationNums[1]) : (target.equationNums[0] + target.equationNums[1]);

                //check for valid solution
                for (int i = 1; i < curOps.Length; i++) {
                    switch (curOps[i]) {
                        case '*':
                            answer *= target.equationNums[i + 1];
                            break;
                        case '+':
                            answer += target.equationNums[i + 1];
                            break;
                    }
                    //if the answer is ever higher than the target answer, it exits
                    if (answer > target.answer) return false;
                }
                return (answer == target.answer);
            }

            return checkForValidOps(curOps + '*', target) || checkForValidOps(curOps + '+', target);
        }

        bool checkForValidOps2(string curOps, (int depth, long answer, int[] equationNums) target) {

            if (curOps.Length == target.depth) {
                long answer = 0;
                switch (curOps[0]) {
                    case '*':
                        answer = target.equationNums[0] * target.equationNums[1];
                        break;
                    case '+':
                        answer = target.equationNums[0] + target.equationNums[1];
                        break;
                    case '|':
                        answer = long.Parse(target.equationNums[0].ToString() + target.equationNums[1].ToString());
                        break;
                }

                //check for valid solution
                for (int i = 1; i < curOps.Length; i++) {
                    switch (curOps[i]) {
                        case '*':
                            answer *= target.equationNums[i + 1];
                            break;
                        case '+':
                            answer += target.equationNums[i + 1];
                            break;
                        case '|':
                            answer = long.Parse(answer.ToString() + target.equationNums[i + 1].ToString());
                            break;
                    }
                    //if the answer is ever higher than the target answer, it exits
                    if (answer > target.answer) return false;
                }
                return (answer == target.answer);
            }

            return checkForValidOps2(curOps + '*', target) || checkForValidOps2(curOps + '+', target) || checkForValidOps2(curOps + '|', target);
        }


        void part1() {
            string[] equations = input.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

            long totalOfValidAnswers = 0;
            foreach (string equation in equations) {
                string[] parts = equation.Split(":", StringSplitOptions.RemoveEmptyEntries);
                long answer = long.Parse(parts[0]);
                int[] opSide = AocLib.parseInputToInt(parts[1], " ");
                int numOps = opSide.Length - 1;

                if (checkForValidOps("",(numOps,answer,opSide))) {
                    totalOfValidAnswers += answer;
                }
            }
            print($"Sum of all the valid answers = {totalOfValidAnswers}");

        }

        void part2() {
            string[] equations = input.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

            long totalOfValidAnswers = 0;
            foreach (string equation in equations) {
                string[] parts = equation.Split(":", StringSplitOptions.RemoveEmptyEntries);
                long answer = long.Parse(parts[0]);
                int[] opSide = AocLib.parseInputToInt(parts[1], " ");
                int numOps = opSide.Length - 1;

                if (checkForValidOps2("", (numOps, answer, opSide))) {
                    totalOfValidAnswers += answer;
                }
            }
            print($"Sum of all the valid answers = {totalOfValidAnswers}");

        }



    }
}

