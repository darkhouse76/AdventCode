using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace CodeTAF
{
    public class AOC2104 : MonoBehaviour
    {
        [SerializeField]
        private bool partTwo = false;
        [SerializeField]
        private bool useTestInput = true;
        [SerializeField]
        private bool run = false;
        private string input;

        private readonly string inputFolderName = "inputs";


        private string RootPath {
            get {
                var g = AssetDatabase.FindAssets($"t:Script {GetType().Name}");
                return AssetDatabase.GUIDToAssetPath(g[0]);
            }
        }
        private string InputPath {
            get {
                return $"{RootPath[..^(GetType().Name.Length + 3)]}{inputFolderName}/";
            }
        }
        private string Day {
            get {
                return GetType().Name[^2..];
            }
        }
        private string TestInput {
            get {
                string filePath = $"{InputPath}{Day}test.txt";

                if (!File.Exists(filePath)) {
                    Debug.LogError($"NO input file found @ {filePath}");
                    return null;
                }
                return File.ReadAllText(filePath);
            }
        }
        private string RealInput {
            get {
                string filePath = $"{InputPath}{Day}real.txt";

                if (!File.Exists(filePath)) {
                    Debug.LogError($"NO input file found @ {filePath}");
                    return null;
                }
                return File.ReadAllText(filePath);
            }
        }

        class board {

            (int, bool)[,] boardState;

            int[] boardNumbers;    
            bool hasWon = false;

            public int UnmarkedScore { get; set; }
            
            public board(int[] boardNums) {

                boardNumbers = (int[])boardNums.Clone();
                boardState = new (int, bool)[5,5];
                int curNum = 0 ;

                for (int y = 0; y < 5; y++) {
                    for (int x = 0; x < 5; x++) {
                        UnmarkedScore += boardNums[curNum];
                        boardState[x,y] = (boardNums[curNum++], false);                        
                    }
                } 
            }

            public bool markNumber(int targetNum) {
                if (hasWon) {
                    return false;
                }

                if (!Array.Exists(boardNumbers, element => element == targetNum)) {
                    return false;
                }

                
                for (int y = 0; y < boardState.GetLength(1); y++) {
                    for (int x = 0; x < boardState.GetLength(0); x++) {
                        if (boardState[x,y].Item1 == targetNum) {
                            boardState[x, y] = (boardState[x, y].Item1, true);
                            UnmarkedScore -= boardState[x, y].Item1;
                        }
                    }
                }
                hasWon = checkIfWinningState();
                return hasWon;
            }

            private bool checkIfWinningState() {
                int amtMarked;

                //check rows
                for (int y = 0; y < boardState.GetLength(1); y++) {
                    amtMarked = 0;
                    for (int x = 0; x < boardState.GetLength(0); x++) {
                        if (boardState[x, y].Item2 == true && ++amtMarked == boardState.GetLength(0)) {
                            return true;
                        }
                    }
                }

                //check col
                for (int x = 0; x < boardState.GetLength(1); x++) {
                    amtMarked = 0;
                    for (int y = 0; y < boardState.GetLength(0); y++) {
                        if (boardState[x, y].Item2 == true && ++amtMarked == boardState.GetLength(0)) {
                            return true;
                        }
                    }
                }
                return false;
            }

            


        }

        void part1() {
            string[] lines = input.Split(new string[]{ "\r\n", " "}, StringSplitOptions.RemoveEmptyEntries);

            //for (int i = 0; i < lines.Length; i++) { print(lines[i]); }

            //get numbers to mark in order
            int[] callNums = Array.ConvertAll(lines[0].Split(","), int.Parse);

            //create all boards
            List<board> allBoards = new List<board>();
            
            for (int i = 1; i < lines.Length; i += 25) {                

                string[] curBoardNum = new string[25];
                Array.Copy(lines, i, curBoardNum, 0, 25);

                //Debug.Log("NEW BOARD:");
                //Array.ForEach(curBoardNum, v => Debug.Log(v));

                allBoards.Add(new board(Array.ConvertAll(curBoardNum, int.Parse)));   
            }

            bool hasWon = false;
            
            for(int i = 0; i < callNums.Length; i++) {

                foreach( board curBoard in  allBoards) {      

                    if (curBoard.markNumber(callNums[i])) {
                        //board has won
                        hasWon = true;
                        print($"Board WON! last number called = {callNums[i]} ||| Final Score = {curBoard.UnmarkedScore * callNums[i]}");
                        break;
                    }
                }
                if (hasWon) { break; }
            }
        }

        void part2() {
            string[] lines = input.Split(new string[] { "\r\n", " " }, StringSplitOptions.RemoveEmptyEntries);            

            //get numbers to mark in order
            int[] callNums = Array.ConvertAll(lines[0].Split(","), int.Parse);

            //create all boards
            List<board> allBoards = new List<board>();

            for (int i = 1; i < lines.Length; i += 25) {

                string[] curBoardNum = new string[25];
                Array.Copy(lines, i, curBoardNum, 0, 25);;

                allBoards.Add(new board(Array.ConvertAll(curBoardNum, int.Parse)));
            }

            int numOfWinners = 0;

            for (int i = 0; i < callNums.Length; i++) {

                foreach (board curBoard in allBoards) {

                    if (curBoard.markNumber(callNums[i])) {
                        //board has won

                        print($"Board WON! last number called = {callNums[i]} ||| Final Score = {curBoard.UnmarkedScore * callNums[i]}");                        
                    }
                }
                
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
    }
}

