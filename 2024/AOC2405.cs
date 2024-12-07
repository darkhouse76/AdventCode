using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;


namespace CodeTAF
{
    public class AOC2405 : MonoBehaviour
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

        Dictionary<int, List<int>> rules;


        (int[] rules, List<int[]> updates) parseInput(string input) {
            string[] sections = input.Split("\r\n\r\n");

            int[] rulesSection = AocLib.parseInputToInt(sections[0], new string[] { "\r\n", "|" });
            string[] updateStrings = sections[1].Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            List <int[]> updateLines = new List <int[]>();

            foreach (string update in updateStrings) {
                updateLines.Add(AocLib.parseInputToInt(update,","));
            }

            return (rulesSection, updateLines);
        }

        void createRuleDict(int[] rawRules) {
            // go through all the "left" numbers and add the right number to the value of the left number key
            
            for (int i = 0; i < rawRules.Length; i += 2) {
                if (!rules.ContainsKey(rawRules[i])) { 
                    rules.Add(rawRules[i], new List<int> { rawRules[i + 1] }); 
                    continue; 
                }
                rules[rawRules[i]].Add(rawRules[i + 1]);
            }

            //// go through all the "right" numbers and add the left number (negative) to the value of the right number key
            for (int i = 0; i < rawRules.Length; i += 2) {
                if (!rules.ContainsKey(rawRules[i+1])) {
                    rules.Add(rawRules[i+1], new List<int> { rawRules[i] * -1 });
                    continue;
                }
                rules[rawRules[i+1]].Add(rawRules[i] * -1);
            }
        }

        
        bool checkIfValid(int[] curUpdate, out int middlePage, bool fixUpdateOnly = false) {
            return checkIfValid(curUpdate.ToList(), out middlePage, fixUpdateOnly);           
        }

        bool checkIfValid(List<int> curUpdate, out int middlePage, bool fixUpdateOnly = false) {
            int upperBound = curUpdate.Count;
            middlePage = curUpdate[upperBound / 2];

            //exits if any index in the update is invalid by not following on of the rules.
            for (int curIndex = 0; curIndex < curUpdate.Count; curIndex++) {
                foreach (int rule in rules[curUpdate[curIndex]]) {
                    //if negative then needs to not be earlier in the update array
                    if (rule > 0 && curIndex != 0) {
                        if (curUpdate.GetRange(0, curIndex).Contains(rule)) {
                            if (fixUpdateOnly) { middlePage = makeValid(curUpdate); }
                            return fixUpdateOnly;
                        }
                    }
                    else if (curIndex != upperBound) {
                        //if  then needs to not be later in the update array                        
                        if (curUpdate.GetRange(curIndex, upperBound - curIndex).Contains(rule * -1)) {
                            if (fixUpdateOnly) { middlePage = makeValid(curUpdate); }                            
                            return fixUpdateOnly;
                        }
                    }
                }
            }

            return !fixUpdateOnly;
        }

        int makeValid(List<int> curUpdate) {

            List<int> validList = new List<int>() { curUpdate[0] };            
            int middlePage = -1;
            int curIndex = 1;
            
            //int[] testList = validList;

            while (validList.Count < curUpdate.Count) {            
                for (int i = 0; i < validList.Count; i++) {
                    List<int> testList = validList;
                    testList.Insert(testList.Count - 1 - i, curUpdate[curIndex]);

                    if (checkIfValid(testList, out middlePage)) {
                        validList = testList;
                        break;
                    }                    
                }
                curIndex++;                
            }

            return middlePage;
        }

        void part1() {
            var sections = parseInput(input);
            rules = new Dictionary<int, List<int>>();
            createRuleDict(sections.rules);

            int middleTotals = 0;
            foreach (int[] update in sections.updates) {
                if (checkIfValid(update, out int middleUpdate)) { middleTotals += middleUpdate; }
            }

            print($"Total of all middle pages = {middleTotals}");
        }

        void part2() {
            var sections = parseInput(input);
            rules = new Dictionary<int, List<int>>();
            createRuleDict(sections.rules);

            int middleTotals = 0;
            foreach (int[] update in sections.updates) {
                if (checkIfValid(update, out int middleUpdate, true)) { middleTotals += middleUpdate; }
            }

            print($"Total of all middle pages fixed = {middleTotals}");      

        }



    }
}

