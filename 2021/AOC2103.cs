using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace CodeTAF
{
    public class AOC2103 : MonoBehaviour
    {
        [SerializeField]
        private bool partTwo = false;
        [SerializeField]
        private bool useTestInput = true;
        [SerializeField]
        private bool run = false;
        private string input;

        private string inputFolderName = "inputs";


        string rootPath {
            get {
                var g = AssetDatabase.FindAssets($"t:Script {GetType().Name}");
                return AssetDatabase.GUIDToAssetPath(g[0]);
            }
        }
        string inputPath {
            get {
                return $"{rootPath[..^(GetType().Name.Length + 3)]}{inputFolderName}/";
            }
        }
        string day {
            get {
                return GetType().Name[^2..];
            }
        }
        string testInput {
            get {
                string filePath = $"{inputPath}{day}test.txt";

                if (!File.Exists(filePath)) {
                    Debug.LogError($"NO input file found @ {filePath}");
                    return null;
                }
                return File.ReadAllText(filePath);
            }
        }
        string realInput {
            get {
                string filePath = $"{inputPath}{day}real.txt";

                if (!File.Exists(filePath)) {
                    Debug.LogError($"NO input file found @ {filePath}");
                    return null;
                }
                return File.ReadAllText(filePath);
            }
        }





        void part1() {

            string[] lines = input.Split("\r\n");

            string gammaRate = "";
            string epsilonRate = "";

            int[] numON = new int[lines[0].Length];
            int[] numOFF = new int[lines[0].Length];

            foreach (string line in lines) {
                //int numOFF = 0;
                //int numON = 0;                

                for (int i = 0; i < line.Length; i++) {
                    if (line[i] == '1') { numON[i]++; }
                    else { numOFF[i]++; }
                }
                
            }

            for (int i = 0;i < numON.Length; i++) {
                if (numON[i] > numOFF[i]) {
                    gammaRate += "1";
                    epsilonRate += "0";
                }
                else {
                    gammaRate += "0";
                    epsilonRate += "1";
                }
            }            


            int gammaRateNum = Convert.ToInt32(gammaRate, 2); 
            int epsilonRateNum = Convert.ToInt32(epsilonRate, 2);


            print($"gamma Rate = {gammaRate} = {gammaRateNum}");
            print($"epsilon Rate = {epsilonRate} = {epsilonRateNum}");
            print($"Power = {gammaRateNum * epsilonRateNum}");


        }

        char getMostCommon(int curBit, List<string> bitLines) {
            
            int numON = 0;
            int numOFF = 0;

            foreach (string line in bitLines) {
                if (line[curBit] == '1') { numON++; }
                else { numOFF++; }
            }

            if (numON == numOFF) { return '1'; }
            return (numON > numOFF) ? '1' : '0';
        }

        List<string> filterRates(int curBit, char targetBit, List<string> bitLines) {

            List<string> filterLines = new List<string>();            

            for (int i = 0; i < bitLines.Count; i++) { 
                if (bitLines[i][curBit] == targetBit) { filterLines.Add(bitLines[i]); }
            }
            return filterLines;
        }

        void part2() {
            string[] lines = input.Split("\r\n");            

            var oxyGen = new List<string>(lines);
            var co2 = new List<string>(lines);


            for (int i = 0; i < lines[0].Length; i++ ) {
                if (oxyGen.Count > 1) { oxyGen = filterRates(i, getMostCommon(i, oxyGen), oxyGen); }
                if (co2.Count > 1) { co2 = filterRates(i, (getMostCommon(i, co2) == '1' ? '0': '1'), co2); }   
            }

            int oxyGenRate = Convert.ToInt32(oxyGen[0], 2);
            int co2Rate = Convert.ToInt32(co2[0], 2);    


            print($"Oxygen Rate = {oxyGen[0]} = {oxyGenRate}");
            print($"CO2 Scrubber Rate = {co2[0]} = {co2Rate}");
            print($"Life Support Rate = {oxyGenRate * co2Rate}");

        }

        void Update() {
            if (run) {
                run = false;
                Debug.Log("========================================================================");

                input = useTestInput ? testInput : realInput;

                var startTime = System.DateTime.Now;

                if (partTwo) { part2(); }
                else { part1(); }
                print($"Took {System.DateTime.Now - startTime} to complete.");
            }
        }
    }
}

