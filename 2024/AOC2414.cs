using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Schema;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;


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

                
                if (partTwo) { 
                    part2();

                    Print2d(visMap);
                    print("--------------------------------");
                    Print2d(visMap, false);
                    print($"^^^^^ This was for {secondsInFuture} secs ^^^^^");

                    drawRobots();
                    secondsInFuture++;
                }
                else { part1(); }                
                print($"Took {System.DateTime.Now - startTime} to complete.");
            }

            if (scan) {
                if (updateRobotPos()) {
                    Print2d(visMap);
                    print("--------------------------------");
                    Print2d(visMap, false);
                    print($"^^^^^ This was for {secondsInFuture} secs ^^^^^");

                    drawRobots();
                    scan = false;
                }
                if (secondsInFuture++ > 11000) { scan = false; }                
            }
        }

        /////////////////////////////////////////////////////////////////
        /// Everything above is for unity and getting the input files ///
        /////////////////////////////////////////////////////////////////
        
        [SerializeField]
        GameObject ground;
        [SerializeField]
        GameObject robotObj;
        [SerializeField]
        Transform parentTarget;

        [SerializeField, Range(1, 30)]
        int scanThreshold = 10;

        [SerializeField]
        int secondsInFuture = 100; //the part to answer is at 6644 seconds
        [SerializeField]
        bool scan = false;


        static (int x, int y) maxSize;
        (int minX, int minY, int maxX, int maxY)[] quadrantBounds;
        List<GameObject> allRobotObjs;
        List<robot> allRobots;
        int[,] visMap;

        class robot {

            static public int TotalRobots = 0;
            (int x, int y) startPos;
            (int x, int y) curVector;   

            public robot((int x, int y) StartPos, (int x, int y) CurVector) {
                this.startPos = StartPos;
                this.curVector = CurVector;
                TotalRobots++;
            }

            public (int x, int y) getFuturePos(int seconds) {

                int newX = ((curVector.x * seconds) + startPos.x) % maxSize.x;
                newX = (newX < 0) ? (maxSize.x + newX) : newX;
                int newY = ((curVector.y * seconds) + startPos.y) % maxSize.y;
                newY = (newY < 0) ? (maxSize.y + newY) : newY;
                return (newX, newY);                
            } 
        }

        void clearAllObjs() {
            if (allRobotObjs == null) { return; }
            foreach (GameObject obj in allRobotObjs) {
                GameObject.Destroy(obj);
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

        static void Print2d<T>(T[,] array2D, bool withTab = true) {         

            for (int row = 0; row < array2D.GetLength(1); row++) {
                string textRow = "";
                for (int col = 0; col < array2D.GetLength(0); col++) {
                    if (withTab) { textRow += "\t"; }
                    textRow += (array2D[col, row].Equals(0)) ? ". " : array2D[col, row] + " ";
                }
                print(textRow);
            }
        }

        void drawRobots() {

            clearAllObjs();
            allRobotObjs = new List<GameObject>();
            for (int row = 0; row < maxSize.y; row++) {
                for (int col = 0; col < maxSize.x; col++) {
                    if (visMap[col, row] > 0) { allRobotObjs.Add(Instantiate(robotObj, new Vector3(col, row*-1, 0) + parentTarget.position, Quaternion.identity, parentTarget)); }
                }
            }


        }

        bool updateRobotPos() {
            
            visMap = new int[maxSize.x, maxSize.y];

            foreach (var bot in allRobots) {
                var futurePos = bot.getFuturePos(secondsInFuture);                
                visMap[futurePos.x, futurePos.y]++;
            }
            
            for (int col = 0; col < maxSize.x; col++) {
                int count = 0;
                for (int row = 0; row < maxSize.y; row++) {
                    count += visMap[col, row];
                }
                if (count > scanThreshold) { return true; }
            }

            return false;
        }


        void part1() {
            maxSize = useTestInput ? (11, 7) : (101, 103);
            int wideDiv = (maxSize.x / 2);
            int tallDiv = (maxSize.y / 2);

            //int[,] visMap = new int[maxSize.x, maxSize.y];

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
            var robotInputs = Regex.Matches(input, @"p=(?<posX>\d+|-\d+),(?<posY>\d+|-\d+) v=(?<velX>\d+|-\d+),(?<velY>\d+|-\d+)");
            int[] botCountPerQuadrant = new int[4];
            List<robot> allRobots = new();
            robot.TotalRobots = 0;

            foreach( Match robotInput in robotInputs ) {
                (int x, int y) pos = ( int.Parse(robotInput.Groups["posX"].Value), int.Parse(robotInput.Groups["posY"].Value) );
                (int x, int y) vel = ( int.Parse(robotInput.Groups["velX"].Value), int.Parse(robotInput.Groups["velY"].Value) );
                allRobots.Add(new robot(pos, vel));
            } 

            foreach( var bot in allRobots) {
                var futurePos = bot.getFuturePos(secondsInFuture);                
                int quadrant = getQuadrant(futurePos);                
                if (quadrant >= 0 ) { botCountPerQuadrant[quadrant]++; }
            }
            
            long safetyFactor = 1;
            foreach ( var botCount in botCountPerQuadrant) {
                safetyFactor *= botCount;
            }

            //long safetyFactor = botCountPerQuadrant.Aggregate(1, (total, next) => total *= next);

            print($"The safety factor after {secondsInFuture} seconds = {safetyFactor}");
        }

        void part2() {

            maxSize = useTestInput ? (11, 7) : (101, 103);

            //ground.transform.position.Set(parentTarget.position.x,parentTarget.position.y, parentTarget.position.z);
            ground.transform.localScale = new Vector3Int(maxSize.x, maxSize.y);

            int wideDiv = (maxSize.x / 2);
            int tallDiv = (maxSize.y / 2);


            //look at the cool regex lol
            var robotInputs = Regex.Matches(input, @"p=(?<posX>\d+|-\d+),(?<posY>\d+|-\d+) v=(?<velX>\d+|-\d+),(?<velY>\d+|-\d+)");            
            allRobots = new();
            robot.TotalRobots = 0;

            foreach (Match robotInput in robotInputs) {
                (int x, int y) pos = (int.Parse(robotInput.Groups["posX"].Value), int.Parse(robotInput.Groups["posY"].Value));
                (int x, int y) vel = (int.Parse(robotInput.Groups["velX"].Value), int.Parse(robotInput.Groups["velY"].Value));
                allRobots.Add(new robot(pos, vel));
            }

            updateRobotPos(); 

        }



    }
}

