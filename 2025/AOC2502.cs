using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace CodeTAF
{
    public class AOC2502 : MonoBehaviour
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

        bool IsInvalid(ulong productID) {                      
            string ID = productID.ToString();

            //if odd then must be valid Part 1 (Not invalid)
            if (ID.Length % 2 != 0) { return  false; }

            int halfIndexPos = ID.Length / 2;           

            return ID[..halfIndexPos] == ID[halfIndexPos..];
        }

        bool IsInvalid2(ulong productID) {
            string ID = productID.ToString();
            int halfIndexPos = ID.Length / 2;
            string pattern = @"^(\d+)\1+$"; //new, after original solve

            //for (int i = 1; i <= halfIndexPos; i++) { //old way
                //print($"{ID} Trying {ID[..i]}"); //remove before full testing

                //string pattern = @"^(?:" + ID[..i] + @"){2,}$"; //old pattern that originally solved in ~48seconds
                
                
            if (Regex.IsMatch(ID, pattern)) {
                return true;
            }
            //} //old way

            return false;            
        }


        void part1() {

            ulong answer = 0;

            foreach (Match range in Regex.Matches(input, @"(?<low>\d*)-(?<high>\d*)") ) {

                ulong startNum = ulong.Parse(range.Groups["low"].Value);
                ulong endNum = ulong.Parse(range.Groups["high"].Value);

                for (ulong i = startNum; i <= endNum; i++) {
                    if (IsInvalid(i)) {
                        answer += i;
                    }
                }
            }

            print($"Part 1 answer: {answer}");
        }

        void part2() {
            //print(IsInvalid2(44644677));            

            ulong answer = 0;

            foreach (Match range in Regex.Matches(input, @"(?<low>\d*)-(?<high>\d*)")) {

                ulong startNum = ulong.Parse(range.Groups["low"].Value);
                ulong endNum = ulong.Parse(range.Groups["high"].Value);

                for (ulong i = startNum; i <= endNum; i++) {
                    if (IsInvalid2(i)) {
                        answer += i;
                    }
                }
            }

            print($"Part 2 answer: {answer}");

        }



    }
}
