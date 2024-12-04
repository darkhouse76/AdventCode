using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace CodeTAF
{
    public class AOC2404 : MonoBehaviour
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

        (int x, int y)[] directions = new[] { (0, 1), (1, 0), (0, -1), (-1, 0), (1, 1), (1,-1), (-1,-1), (-1,1) };
        char[,] wordSearch;
        (int x, int y) maxSize;

        bool checkDir((int x, int y) startPos, (int x, int y) curDir) {
            string foundWord = wordSearch[startPos.x, startPos.y].ToString();
            (int x, int y) farEndSearch = (startPos.x + (curDir.x * 3), startPos.y + (curDir.y * 3));

            if (farEndSearch.x >= maxSize.x || farEndSearch.x < 0 || farEndSearch.y >= maxSize.y || farEndSearch.y < 0) { 
                return false; 
            } 
            
            for (int i = 1; i < 4; i++) {                
                foundWord += wordSearch[startPos.x + curDir.x * i, startPos.y + curDir.y * i];
            }
            if (foundWord == "XMAS" || foundWord == "SAMX") {
                return true;
            }
            return false;
        }

        bool checkforX((int x, int y) startPos) {

            List<char> crossLetters = new() { wordSearch[startPos.x, startPos.y] };

            //string forwardSlash = wordSearch[startPos.x, startPos.y].ToString();
            //string backSlash = forwardSlash;

            (int x, int y)[] crossDirections = new[] { (1, 1), (-1, -1), (1, -1), (-1, 1) };            

            //checks if middle letter is A and if the top left and bottom right are different.
            if (crossLetters[0] != 'A' || wordSearch[startPos.x + 1,startPos.y + 1] == wordSearch[startPos.x - 1 , startPos.y - 1]) { return false; }

            foreach (var dir in  crossDirections) {
                (int x, int y) checkPos = ((startPos.x + dir.x), (startPos.y + dir.y));
                //check if part of the cross would be out of bounds  NOTE: might not be needed if I do the for loop not on the edges
                //if (checkPos.x >= maxSize.x || checkPos.x < 0 || checkPos.y >= maxSize.y || checkPos.y < 0) {
                //    return false;
                //}

                crossLetters.Add(wordSearch[checkPos.x,checkPos.y]);                
            }

            if (crossLetters.Count(letter => letter == 'M') != 2 || crossLetters.Count(letter => letter == 'S') != 2) { return false; }            
            return true;            
        }


        void part1() {
            wordSearch = AocLib.ParseSimpleCharMap(input);
            maxSize = (wordSearch.GetLength(0),  wordSearch.GetLength(1));

            //AocLib.Print2d(wordSearch);

            int totalFound = 0;

            for (int col = 0; col <  maxSize.x; col++) {
                for (int row = 0; row < maxSize.y; row++ ) {
                    foreach (var dir in directions) {
                        if (checkDir((col,row),dir)) { totalFound++; }
                    }
                }            
            }

            print($"Total XMAS found: {totalFound/2}");
        }

        void part2() {
            wordSearch = AocLib.ParseSimpleCharMap(input);
            maxSize = (wordSearch.GetLength(0), wordSearch.GetLength(1));

            int totalFound = 0;

            for (int col = 1; col < maxSize.x-1; col++) {
                for (int row = 1; row < maxSize.y-1; row++) {
                    if (checkforX((col,row))) { totalFound++; }
                }
            }
            print($"Total XMAS found: {totalFound}");
        }

    }
}

