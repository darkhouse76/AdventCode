using System;
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

        void part2() {


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

