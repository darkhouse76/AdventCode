using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace CodeTAF
{
    public class AOC2409 : MonoBehaviour
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

        int firstFreeIndexCache;        

        bool isFreeSpaceToMove(List<int> fsIds, int size, int upperIndexLimit, out int moveStartIndex) {
            int freeSize = 0;
            moveStartIndex = -1;                      

            for (int i = firstFreeIndexCache; i < upperIndexLimit; i++) {
                if (fsIds[i] < 0) {
                    if (freeSize == 0) { moveStartIndex = i; }
                    if (++freeSize == size) { 
                        if (moveStartIndex == firstFreeIndexCache) { firstFreeIndexCache += size; }
                        return true; }
                } else {
                    freeSize = 0;
                }
            }
            return false;
        }

        void part1() {
            List<int> fsIds = new();
            //List<int> freeIndexes = new();

            int numFree = 0; //might need for part 2. I have feeling
            int numFiles = 0;
            int numFileBlocks = 0;

            for (int i = 0; i < input.Length; i++) {
                for (int j = 0; j < (int)(input[i] - '0'); j++) {
                    if (i % 2 == 0) {
                        numFiles++;
                        fsIds.Add(numFileBlocks);
                    } else {
                        numFree++;
                        //freeIndexes.Add(fsIds.Count);
                        fsIds.Add(-1);
                    }
                }
                if (i % 2 == 0) numFileBlocks++;
            }           
            
            
            int[] beginningBlocks = fsIds.GetRange(0, numFiles).ToArray();
            List<int> endingBlocks = fsIds.GetRange(numFiles, fsIds.Count - numFiles);
            endingBlocks.RemoveAll(x => x < 0);
            endingBlocks.Reverse();

            int[] finalFS = new int[beginningBlocks.Length];
            int moveFileCount = 0;

            for (int i = 0; i < beginningBlocks.Length; i++) {
                finalFS[i] = (beginningBlocks[i] >= 0) ? beginningBlocks[i] : endingBlocks[moveFileCount++];                
            }
            
            long checkSum = 0;
            //string fileSystem = "";
            for (int i = 0; i < finalFS.Length; i++) {
                checkSum += finalFS[i] * i;  
                //fileSystem += finalFS[i];
            }
            //print(fileSystem);

            print($"The checksum for the filesystem = {checkSum}");           

        }

        void part2() {

            List<int> fsIds = new();
            List<int> sizeOfBlocks = new(); 

            int numFiles = 0;
            int numFileBlocks = 0;

            for (int i = 0; i < input.Length; i++) {
                for (int j = 0; j < (int)(input[i] - '0'); j++) {
                    if (i % 2 == 0) {
                        numFiles++;
                        fsIds.Add(numFileBlocks);
                    }
                    else {                        
                        fsIds.Add(-1);
                    }
                }
                if (i % 2 == 0) { 
                    numFileBlocks++; 
                    sizeOfBlocks.Add((int)(input[i] - '0')); 
                }                
            }
            //init the cache
            firstFreeIndexCache = fsIds.IndexOf(-1);

            //go from the back
            for (int checkIndex = (fsIds.Count - 1); checkIndex > 0; checkIndex--) {
                if (fsIds[checkIndex] < 0) continue;

                int curBlockSize = sizeOfBlocks[fsIds[checkIndex]];
                if (isFreeSpaceToMove(fsIds, curBlockSize, checkIndex, out int moveToIndex )) {

                    for (int i = 0; i < curBlockSize; i++) {
                        fsIds[moveToIndex + i] = fsIds[checkIndex - i];
                        fsIds[checkIndex - i] = -1;
                    }                   
                }
                //skip the block because it either was moved or not
                checkIndex -= (curBlockSize - 1);      
            }            

            long checkSum = 0;            
            for (int i = 0; i < fsIds.Count; i++) {
                if (fsIds[i] < 0) continue;
                checkSum += fsIds[i] * i;
            } 

            print($"The checksum for the filesystem = {checkSum}");
        }



    }
}

