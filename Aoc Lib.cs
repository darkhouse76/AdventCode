using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

namespace CodeTAF
{
    public class AocLib : MonoBehaviour
    {
        
        public static void TestPrint(string house) {

            print($"test print = {house}");
            //Debug.Log(house);
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

    }
}
