using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;


namespace CodeTAF
{
    public class AOC2414 : MonoBehaviour
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

        [SerializeField]
        int secondsInFuture = 100;

        static (int x, int y) maxSize;
        (int minX, int minY, int maxX, int maxY)[] quadrantBounds;

        class robot {
            (int x, int y) startPos;
            (int x, int y) curVector;            


            public robot((int x, int y) StartPos, (int x, int y) CurVector) {
                this.startPos = StartPos;
                this.curVector = CurVector;                
            }

            public (int x, int y) getFuturePos(int seconds) {
                (int x, int y) nonWrapPos = ((curVector.x * seconds) + startPos.x, (curVector.y * seconds) + startPos.y);

                int newX = Math.Abs(nonWrapPos.x) - (Math.Abs(nonWrapPos.x / maxSize.x) * maxSize.x);
                newX = nonWrapPos.x < 0 ? (maxSize.x - newX) : newX;
                int newY = Math.Abs(nonWrapPos.y) - (Math.Abs(nonWrapPos.y / maxSize.y) * maxSize.y);
                newY = nonWrapPos.y < 0 ? (maxSize.y - newY) : newY;

                return (newX, newY);
            }

        }

        int getQuadrant((int x, int y) pos) {
            for (int i = 0; i < quadrantBounds.Length; i++) {
                if (quadrantBounds[i].minX <= pos.x && pos.x < quadrantBounds[i].maxX && 
                    quadrantBounds[i].minY <= pos.y && pos.y < quadrantBounds[i].maxY) 
                {
                    return i;
                }
            }
            return -1;
        }


        void part1() {
            maxSize = useTestInput ? (11, 7) : (100, 102);
            int wideDiv = (maxSize.x / 2);
            int tallDiv = (maxSize.y / 2);

            quadrantBounds = new (int minX, int minY, int maxX, int maxY)[] {
                //top left
                (0, 0, wideDiv, tallDiv),
                //top right
                ((maxSize.x-wideDiv), 0, maxSize.x + 1, tallDiv),
                //bottom left
                (0, (maxSize.y-tallDiv), wideDiv, maxSize.y + 1),
                //bottom right
                ((maxSize.x-wideDiv), (maxSize.y-tallDiv), maxSize.x + 1, maxSize.y + 1)
            };

            //look at the cool regex lol
            var robotInputs = Regex.Matches(input, @"p=(?<posX>\d+|-\d),(?<posY>\d+|-\d) v=(?<velX>\d+|-\d),(?<velY>\d+|-\d)");
            int[] botCountPerQuadrant = new int[4];
            List<robot> allRobots = new();

            foreach( Match robotInput in robotInputs ) {
                (int x, int y) pos = ( int.Parse(robotInput.Groups["posX"].Value), int.Parse(robotInput.Groups["posY"].Value) );
                (int x, int y) vel = ( int.Parse(robotInput.Groups["velX"].Value), int.Parse(robotInput.Groups["velY"].Value) );
                allRobots.Add(new robot(pos, vel));
            }
            
            foreach( var bot in allRobots) {                
                int quadrant = getQuadrant(bot.getFuturePos(secondsInFuture));                
                if (quadrant >= 0 ) { botCountPerQuadrant[quadrant]++; }
            }

            int safetyFactor = botCountPerQuadrant.Aggregate(1, (total, next) => total *= next);

            print($"The safety factor after {secondsInFuture} seconds = {safetyFactor}");
        }

        void part2() {


        }



    }
}

