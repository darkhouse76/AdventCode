using System;
using System.Collections.Generic;
using UnityEngine;


namespace CodeTAF
{
    public class AocLib : MonoBehaviour
    {
        
        public static void TestPrint(string house) {

            print($"test print = {house}");
            //Debug.Log(house);
        }


        public static class Map {

            public readonly static (int x, int y)[] Directions = new[] { (0, 1), (1, 0), (0, -1), (-1, 0) };

            public const int DOWN = 0;
            public const int RIGHT = 1;
            public const int UP = 2;
            public const int LEFT = 3;

            public static (int x, int y) getOppositeDir((int x, int y) curDir) {
                return (curDir.x * -1, curDir.y * -1);
            }

            public static (int x, int y) getOppositeDir(int curDirNum) {
                return (Directions[curDirNum].x * -1, Directions[curDirNum].y * -1);
            }


            public static (int x, int y) TurnAround((int x, int y) curDir) {
                return TurnRight(TurnRight(curDir));
            }

            public static (int x, int y) TurnLeft((int x, int y) curDir) {
                int newDir = -1;
                for (int i = 0; i < Directions.Length; i++) {
                    if (curDir == Directions[i]) { newDir = i + 1; }
                }
                return (newDir > 3) ? Directions[0] : Directions[newDir];
            }

            public static (int x, int y) TurnRight((int x, int y) curDir) {
                int newDir = 99;
                for (int i = 0; i < Directions.Length; i++) {
                    if (curDir == Directions[i]) { newDir = i - 1; }
                }
                return (newDir < 0) ? Directions[3] : Directions[newDir];
            }

            public static (int x, int y) MoveForward((int x, int y) curPos, (int x, int y) curDir) {
                return (curPos.x + curDir.x, curPos.y + curDir.y);                
            }

            public static (int x, int y) MoveBack((int x, int y) curPos, (int x, int y) curDir) {
                return (curPos.x - curDir.x, curPos.y - curDir.y);
            }

            //finds the first instance of whatever target in the 2d array.
            // Recommend to use the TryFind version if uncertain if actually in the array. 
            public static (int x, int y) Find<T>(T[,] map, T target) {
                for (int col = 0; col < map.GetLength(0); col++) {
                    for (int row = 0; row < map.GetLength(1); row++) {
                        if (map[col, row].Equals(target)) { return (col, row); }
                    }
                }
                return (-1, -1);
            }

            //Tries to find the first instance of whatever target in the 2d array.
            public static bool TryFind<T>(T[,] map, T target, out (int x, int y) targetPos) {
                for (int col = 0;  col < map.GetLength(0); col++) {
                    for (int row = 0; row < map.GetLength(1); row++) {
                        if (map[col, row].Equals(target)) { 
                            targetPos = (col, row);
                            return true; }
                    }
                }
                targetPos = default;
                return false;
            }
            //finds All instances of whatever target in the 2d array.
            // Recommend to use the TryFindAll version if uncertain if actually in the array. 
            public static List<(int x, int y)> FindAll<T>(T[,] map, T target) {
                List<(int x, int y)> targetsPos = new();
                for (int col = 0; col < map.GetLength(0); col++) {
                    for (int row = 0; row < map.GetLength(1); row++) {
                        if (map[col, row].Equals(target)) { targetsPos.Add((col, row)); }
                    }
                }
                return targetsPos;
            }
            //Tries to find All instances of whatever target in the 2d array.
            public static bool TryFindAll<T>(T[,] map, T target, out List<(int x, int y)> targetsPos) {
                targetsPos = new();
                for (int col = 0; col < map.GetLength(0); col++) {
                    for (int row = 0; row < map.GetLength(1); row++) {
                        if (map[col, row].Equals(target)) {
                            targetsPos.Add((col, row));
                            return true;
                        }
                    }
                }                
                return false;
            }

            //checks if the position is in the bounds. Either by tuple maxSize or the 2d array it's self
            public static bool IsInBounds((int x, int y) checkPos, (int x, int y) sizeMax) {
                return (checkPos.x >= 0 && checkPos.y >= 0 && checkPos.x < sizeMax.x && checkPos.y < sizeMax.y);              
            }
            //checks if the position is in the bounds. Either by tuple maxSize or the 2d array it's self
            public static bool IsInBounds<T>((int x, int y) checkPos, T[,] map) {
                (int x, int y) sizeMax = (map.GetLength(0), map.GetLength(1));
                return (checkPos.x >= 0 && checkPos.y >= 0 && checkPos.x < sizeMax.x && checkPos.y < sizeMax.y);
            }



        }

        
        //takes string of even grid of char and put them into a 2d array of char.
        //can be reversed vertically if needed to shift the 0,0 point to the bottom left instead of the the top left.        
        public static char[,] ParseSimpleCharMap(string input, bool reverseMap = false) {
            string[] mapLines = input.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            if (reverseMap) { Array.Reverse(mapLines); }            
            char[,] output = new char[mapLines[0].Length, mapLines.Length];

            for (int line = 0; line < mapLines.Length; line++) {
                for (int col = 0; col < mapLines[line].Length; col++) {
                    output[col, line] = mapLines[line][col];
                }
            }
            return output;
        }

        public static int[,] ParseSimpleIntMap(string input, bool reverseMap = false) {
            string[] mapLines = input.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            if (reverseMap) { Array.Reverse(mapLines); }
            int[,] output = new int[mapLines[0].Length, mapLines.Length];

            for (int line = 0; line < mapLines.Length; line++) {
                for (int col = 0; col < mapLines[line].Length; col++) {
                    output[col, line] = mapLines[line][col] - '0';
                }
            }
            return output;
        }



        //prints to console the 2d array. can be reversed vertically and with tab spacing or now.
        public static void Print2d<T>(T[,] array2D, bool withTab = true, bool reverseMap = false) {
            //bool reverseMap = true;

            if (reverseMap) {
                for (int row = array2D.GetLength(1) - 1; row >= 0; row--) {
                    string textRow = "";
                    for (int col = 0; col < array2D.GetLength(0); col++) {
                        if (withTab) { textRow += "\t"; }
                        textRow += array2D[col, row];
                    }
                    print(textRow);
                }
                return;
            }

            for (int row = 0; row < array2D.GetLength(1); row++) {
                string textRow = "";
                for (int col = 0; col < array2D.GetLength(0); col++) {
                    if (withTab) { textRow += "\t"; }
                    textRow += array2D[col, row];
                }
                print(textRow);
            }
        }

        //set all values of an 2d array to the target value. IE: all of bool[,] bob = false;
        public static T[,] SetAllValues<T>(T[,] inputMap, T targetValue) {

            T[,] outputMap = new T[inputMap.GetLength(0), inputMap.GetLength(1)];

            for (int col = 0; col < inputMap.GetLength(0); col++) {
                for (int row = 0; row < inputMap.GetLength(1); row++) {
                    outputMap[col, row] = targetValue;
                }
            }

            return outputMap;
        }

        //Parse all numbers in a string array and return int array. 
        //may not need due to other methods to do this. 
        public static int[] GetIntNumbers(string[] numList) {
            List<int> numbers = new List<int>();
            
            foreach (string num in numList) {
                if (int.TryParse(num, out int newNum)) {
                    numbers.Add(newNum);
                }
            }
            return numbers.ToArray();
        }

        //parses everything into numbers and then converts to int array. 
        //overloads include string array or 1 string or 2 strings.
        public static int[] parseInputToInt(string input, string[] separators) {
            string[] stringArray = input.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return Array.ConvertAll(stringArray, int.Parse);
        }

        public static int[] parseInputToInt(string input, string arg1) {
            return parseInputToInt(input, new string[] { arg1 });
        }

        public static int[] parseInputToInt(string input, string arg1, string arg2) {
            return parseInputToInt(input, new string[] { arg1, arg2 });            
        }
        //parses everything into numbers and then converts to long array. 
        //overloads include string array or 1 string or 2 strings.
        public static long[] parseInputToLong(string input, string[] separators) {
            string[] stringArray = input.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return Array.ConvertAll(stringArray, Int64.Parse);
        }

        public static long[] parseInputToLong(string input, string arg1) {
            return parseInputToLong(input, new string[] { arg1 });
        }

        public static long[] parseInputToLong(string input, string arg1, string arg2) {
            return parseInputToLong(input, new string[] { arg1, arg2 });
        }

        //returns the numberOf digits in either a int or long.
        //Note: this is one of the faster ways to do this. There are "shorter lines" ways.
        public static int numberOfDigits(int number) {

            if (number >= 0) {
                switch (number) {
                    case < 10: return 1;
                    case < 100: return 2;
                    case < 1000: return 3;
                    case < 10000: return 4;
                    case < 100000: return 5;
                    case < 1000000: return 6;
                    case < 10000000: return 7;
                    case < 100000000: return 8;
                    case < 1000000000: return 9;
                    default: return 10;
                }                
            }
            else {
                switch (number) {
                    case > -10: return 2;
                    case > -100: return 3;
                    case > -1000: return 4;
                    case > -10000: return 5;
                    case > -100000: return 6;
                    case > -1000000: return 7;
                    case > -10000000: return 8;
                    case > -100000000: return 9;
                    case > -1000000000: return 10;
                    default: return 11;
                }                
            }
        }

        public static int numberOfDigits(long number) {

            if (number >= 0) {
                switch (number) {
                    case < 10L: return 1;
                    case < 100L: return 2;
                    case < 1000L: return 3;
                    case < 10000L: return 4;
                    case < 100000L: return 5;
                    case < 1000000L: return 6;
                    case < 10000000L: return 7;
                    case < 100000000L: return 8;
                    case < 1000000000L: return 9;
                    case < 10000000000L: return 10;
                    case < 100000000000L: return 11;
                    case < 1000000000000L: return 12;
                    case < 10000000000000L: return 13;
                    case < 100000000000000L: return 14;
                    case < 1000000000000000L: return 15;
                    case < 10000000000000000L: return 16;
                    case < 100000000000000000L: return 17;
                    case < 1000000000000000000L: return 18;                    
                    default: return 19;
                }
            }
            else {
                switch (number) {
                    case > -10L: return 2;
                    case > -100L: return 3;
                    case > -1000L: return 4;
                    case > -10000L: return 5;
                    case > -100000L: return 6;
                    case > -1000000L: return 7;
                    case > -10000000L: return 8;
                    case > -100000000L: return 9;
                    case > -1000000000L: return 10;
                    case > -10000000000L: return 11;
                    case > -100000000000L: return 12;
                    case > -1000000000000L: return 13;
                    case > -10000000000000L: return 14;
                    case > -100000000000000L: return 15;
                    case > -1000000000000000L: return 16;
                    case > -10000000000000000L: return 17;
                    case > -100000000000000000L: return 18;
                    case > -1000000000000000000L: return 19;
                    default: return 20;
                }
            }
        }


    }
}
