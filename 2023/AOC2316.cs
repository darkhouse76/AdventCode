using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using static UnityEngine.Rendering.DebugUI.Table;

namespace CodeTAF
{
    public class AOC2316 : MonoBehaviour
    {
        [SerializeField]
        private bool partTwo = false;
        [SerializeField]
        private bool useTestInput = true;
        [SerializeField]
        private bool run = false;
        private string input;

        const char EMPTY_SPACE = '.';
        const char MIRROR_ANGLE_RIGHT = '/';
        const char MIRROR_ANGLE_LEFT = '\\';
        const char MIRROR_VERTICAL = '|';
        const char MIRROR_HORZIONTAL = '-';
        

        char[,] ParseInput(string input) {
            string[] mapLines = input.Split("\r\n");
            Array.Reverse(mapLines);
            char[,] output = new char[mapLines[0].Length, mapLines.Length];

            for (int line = 0; line < mapLines.Length; line++) {
                for (int col = 0; col < mapLines[line].Length; col++) {
                    output[col, line] = mapLines[line][col];
                }
            }
            return output;
        }

        void PrintMap(char[,] chars) {
            bool reverseMap = true;

            if (reverseMap) {
                for (int row = chars.GetLength(0) - 1; row >= 0; row--) {
                    string textRow = "";
                    for (int col = 0; col < chars.GetLength(1); col++) {
                        textRow += "\t" + chars[col, row];
                    }
                    print(textRow);
                }
                return;
            }

            for (int row = 0; row < chars.GetLength(0); row++) {
                string textRow = "";
                for (int col = 0; col < chars.GetLength(1); col++) {
                    textRow += "\t" + chars[col, row];
                }
                print(textRow);
            }
        }

        T[,] SetMapCondition<T>(T[,] inputMap,T targetValue ) {

            T[,] outputMap = new T[inputMap.GetLength(0), inputMap.GetLength(1)];

            for (int col = 0; col < inputMap.GetLength(0); col++) {
                for(int row = 0; row < inputMap.GetLength(1);row++) {
                    outputMap[col, row] = targetValue;
                }
            }

            return outputMap;
        }

        Vector2Int GetNewAngle(Vector2Int startDir, bool angledRight) {
            int dirMod = 1;
            if (!angledRight) { dirMod = -1; }
            
            return new Vector2Int(startDir.y  * dirMod,startDir.x * dirMod);
        }

        bool[,] FireLaser(bool[,] energizeMap, char[,] mirrorMap, Vector2Int curLoc, Vector2Int startDir, bool firstLaser = false) {           

            //if first one then sets it back one for the sake of simple.
            if (firstLaser) { curLoc -= startDir; }

            //print($"Firing Laser start at {curLoc} going {startDir}");

            int maxX = mirrorMap.GetLength(0);
            int maxY = mirrorMap.GetLength(1);

            while (true) {

                //move in dir
                curLoc += startDir;                
                //check if out of bounds;
                if (curLoc.x < 0 || curLoc.x >= maxX) { return energizeMap; }
                if (curLoc.y < 0 || curLoc.y >= maxY) { return energizeMap; }                              
                
                switch (mirrorMap[curLoc.x,curLoc.y]) {
                    case EMPTY_SPACE:
                        energizeMap[curLoc.x, curLoc.y] = true;
                        break;
                    case MIRROR_HORZIONTAL:                        
                        if (startDir == Vector2Int.left || startDir == Vector2Int.right) {
                            energizeMap[curLoc.x, curLoc.y] = true;
                            break; 
                        }        
                        //if already split once then don't do again. To prevent infinite loops of splits. 
                        if (energizeMap[curLoc.x, curLoc.y]) { return energizeMap; }
                        energizeMap[curLoc.x, curLoc.y] = true;
                        return FireLaser(FireLaser(energizeMap, mirrorMap, curLoc, Vector2Int.left), mirrorMap,curLoc,Vector2Int.right);                        
                    case MIRROR_VERTICAL:                        
                        if (startDir == Vector2Int.up || startDir == Vector2Int.down) {                            
                            energizeMap[curLoc.x, curLoc.y] = true;
                            break; 
                        }
                        //if already split once then don't do again. To prevent infinite loops of splits. 
                        if (energizeMap[curLoc.x, curLoc.y]) { return energizeMap; }                        
                        energizeMap[curLoc.x, curLoc.y] = true;                        
                        return FireLaser(FireLaser(energizeMap, mirrorMap, curLoc, Vector2Int.up), mirrorMap, curLoc, Vector2Int.down);                        
                    case MIRROR_ANGLE_RIGHT:                        
                        energizeMap[curLoc.x, curLoc.y] = true;
                        return FireLaser(energizeMap, mirrorMap, curLoc, GetNewAngle(startDir, true));
                    case MIRROR_ANGLE_LEFT:                        
                        energizeMap[curLoc.x, curLoc.y] = true;
                        return FireLaser(energizeMap, mirrorMap, curLoc, GetNewAngle(startDir, false));
                }
            }           
        }

        int AmountEnergized(bool[,] energizeMap, bool printIt = false) {

            char[,] visMap = new char[energizeMap.GetLength(0), energizeMap.GetLength(1)];
            visMap = SetMapCondition<char>(visMap, '.');            

            int amtEnergized = 0;

            for (int col = 0; col < energizeMap.GetLength(0); col++) {
                for (int row = 0; row < energizeMap.GetLength(1); row++) {
                    if (energizeMap[col, row]) {  
                        amtEnergized++;
                        visMap[col, row] = '#';                        
                    }
                }
            }

            if (printIt) { PrintMap(visMap); }
            return amtEnergized;
        }


        void part1() {

            char[,] mirrorMap = ParseInput(input);
            bool[,] energizeMap = new bool[mirrorMap.GetLength(0),mirrorMap.GetLength(1)];

            energizeMap = SetMapCondition<bool>(energizeMap, false);

		    PrintMap(mirrorMap);
            energizeMap = FireLaser(energizeMap, mirrorMap, new Vector2Int(0, mirrorMap.GetLength(1)-1), Vector2Int.right, true);
            print("----------------------------------------------------------------");
            print($"Total Tiles Energized = {AmountEnergized(energizeMap,true)}");

        }

        void part2() {
            char[,] mirrorMap = ParseInput(input);
            int mirrorMaxX = mirrorMap.GetLength(0);
            int mirrorMaxY = mirrorMap.GetLength(1);
            bool[,] energizeMap = new bool[mirrorMaxX, mirrorMaxY];
            //bool[,] HighestEnergizeMap = new bool[mirrorMap.GetLength(0), mirrorMap.GetLength(1)];

            energizeMap = SetMapCondition<bool>(energizeMap, false); //resets the map to false;

            //PrintMap(mirrorMap);
            
            int highestEnergizeConfig = 0;
            int amtEnergize;            

            for (int row = 0;  row < mirrorMaxY;  row++) {
                //from left
                energizeMap = SetMapCondition<bool>(energizeMap, false); //resets the map to false;
                amtEnergize = AmountEnergized(FireLaser(energizeMap, mirrorMap, new Vector2Int(0, row), Vector2Int.right, true));
                highestEnergizeConfig = math.max(highestEnergizeConfig,amtEnergize);

                //from right
                energizeMap = SetMapCondition<bool>(energizeMap, false); //resets the map to false;
                amtEnergize = AmountEnergized(FireLaser(energizeMap, mirrorMap, new Vector2Int(mirrorMaxX - 1, row), Vector2Int.left, true));
                highestEnergizeConfig = math.max(highestEnergizeConfig, amtEnergize);
            }

            for (int col = 0; col < mirrorMaxX; col++) {
                //from up
                energizeMap = SetMapCondition<bool>(energizeMap, false); //resets the map to false;
                amtEnergize = AmountEnergized(FireLaser(energizeMap, mirrorMap, new Vector2Int(col, mirrorMaxY - 1), Vector2Int.down, true));
                highestEnergizeConfig = math.max(highestEnergizeConfig, amtEnergize);

                //from down
                energizeMap = SetMapCondition<bool>(energizeMap, false); //resets the map to false;
                amtEnergize = AmountEnergized(FireLaser(energizeMap, mirrorMap, new Vector2Int(col, 0), Vector2Int.up, true));
                highestEnergizeConfig = math.max(highestEnergizeConfig, amtEnergize);
            }


            print("----------------------------------------------------------------");
            print($"Highest Total Tiles Energized = {highestEnergizeConfig}");
        }

        void Update() {
            if (run) {
                run = false;
                Debug.Log("========================================================================");

                if (useTestInput) { input = InputTest(); }
                else { input = Input(); }

                var startTime = System.DateTime.Now;
                if (partTwo) { part2(); }
                else { part1(); }
                print($"Took {System.DateTime.Now - startTime} to complete.");
            }
        }


        string InputTest() {
            return
@".|...\....
|.-.\.....
.....|-...
........|.
..........
.........\
..../.\\..
.-.-/..|..
.|....-|.\
..//.|....";
        }

        string Input() {
            return
@"\.........../.............../.......\...........................\./.........................|....../.|.......-
....-.........../|......|.......-.......................................................................\./...
-....-...-...........-................|..-..-.-......./.................\.-.......\/-..-..............\.......
...\....-.|.....\.|......./.../...\......|....................................................................
...|..........|.........|........|......|..........................-.../.../...............|.......-.....|....
\......................../.\..............\..|................/..|........./..-................./..\..........
..|..|.....\.....................|.................-...-............................../......\.....\..........
|-.............................../../|...............\.....|..\......................\...........-...-........
|..............\..../........../.....|./.....\........./...........|.....-.../........|....\......|..........|
.|................\....../.....|...........\........\|....-...............-......|........|...................
......\..........................................\...../.....|.../..................-....../..................
.\....\..................\..\......-.............|.........|...................................../-|.....-....
\................/.........-....-......../....|..|/...................\|.\...............-....-............../
.....................\...........-....\......\............||.....-.......\.\.......................\...|.../..
..................|......./....\...-..................|...|/..........................................\.......
..\...................../...................--.................-......../............./..|.......\|.\.|..\|...
...-../....|........../.....\..........-........................../.............................-.......\.-...
../.......|........................-....../........................................./.|...|...........|...\...
.....-./..........................................|........\....\................/.......-...|....-...........
................|..../..|...................-...............\/..|.........................................\...
...........-...\.|..-.\.-.......-./.....\../..-\..-...|........./.............-..........................-...\
.........\../........-.|........|............../....\........|..........|......|../........../...........\....
..|...........................|.............\........\................../.................../......\....\|....
....................-........................//\............../...........-........-........\................/
......./..............|/..|.../............/.-...........-.............................\............../.......
.../............/...|....|\.....-.-..|.......\...|..........-.........\\..........-.-.../......./...../\......
......|/\./.....\..................................|...|.............................................|...-....
............./.|........................................|...\.......-......................../.........\...-..
....................../...\.........\.....................................-.........................\.........
.............\.../....|..................-.|-.-/....|.....-./.......\.......|../..............................
...||...\......./.........\|.\..\....|....../.......\.........-.\.............../.......................\....|
.|......-...............-..||......|../..............................\.../...........-.................|......
.........\/............-....................................|..................................../....\.......
..\........\.-/......|/..//........./.......|./............-./.......................\.-.........../..........
...|..../....|............./|......-..........|..-......................../...\.........../........-...|......
......................../\.....|.................|..........\............-|.........................-.\.......
.........................|..\.\........../..........................................\........................|
-..-......./.................\....|...........-...-.......|...|.....|..|...|........|.\.......................
.......-................./.....................\..|....\......-...|.\.....|.\../..................|......|....
..../....................-....\......-.../.........../........|...././.............-....\............./.......
..|...|..................\.-..............|............\../.................-...............|..|....|.........
..........-....\|.........|.......................|................................\.....--..-.............\..
..........................\...../....-/........................|......................../.\...................
...\................\...........................................\........./............/.......\-......\......
-......./.......................\\................................./|..................\....|...|..-..........
....../..........................\.....\...........\..................\..../...\......\........|...../..../...
..-.........-../....-......-.........../..........-......................\........./......................./..
.......|-..\.......|.....-.........-............-/........../.....\\.............................\./..........
/.-.........../.................................-............./.\....................\....|....\..............
............\......................./../............/.........|../.-.\...................|../.................
../....|................|..\..............\..............................-............./../..|................
...\...............-..........-.....-\...............\..\.//-......./.../|.......|..|.-.........\....\.....-..
.........-........\...........-..................................-...................../.....................|
.\.........-.........|./-.-\.-...........|.../...........|....-..........|-..-...............................\
...........\.../......./..........|.....|.../...............-..................|/....|..........//......--....
.....-.........................................................|..../..|.-......../-......./....|..|..........
.........-..................-..................../..\....................-|......\......................\.....
..........................-.\...|......-...|..-......-./.......-............/....\..............\.......\.....
..-.-.............\............/........../.....................-/.................-./.|/.....................
............\...|...../.|....|...|......|..........|................................\...................\.....
.../....\....../.../...-............./../............./..................//....||...../.........-......-......
..../|..............\-../...-\.\.........../............................../...........-................-...|./
..../|........\......-.................................../........../................-.\|../.........../......
......../.\\...............|..\..........|......|...............-.-.......................|.........\.........
.......|..\..|/........................................................\.......\....|...\...\...........|.....
........./.....\|....................................\..........\\/.......\../\..\\//......-..................
.........|........................................|.....|...........|.../............|....-..-.\\.|...........
.|.................|.........\...............................\..../....\.....\..../..|.......\.......|........
.....|.....-............/........................|.................................................-..........
\.....|.................................\.|.......\................................./.........|.\.............
............................................................/.........\.......................................
..................../.\...//|................./...|./...\|....\\..|.........-.....-.-.........../.../...\.....
.............-........................\...............................-\./...........\........../....\........
.....................|.....|.......................|.....|........../........-...../............|.......\.....
/.-..........................\......../.......\../.....-.......-........................|-...../..............
./...\..\...................|....../..........-................./............./.\..\.......|.\................
....-....\........|...../..-................\|........|........................./\............\......--.\.....
....\../................/.......\.......-.............|.........-.......-..\..................-...............
-......|...................|............................................................................../...
.............../...........\........................|.........../...\..-...........|...........|.....|........
......-....................-........--...|...............//.../.\.|..//...../..............-..\./..\..........
.......\.\................................\......\..|..............\.\.......\.../............../......\...-..
.....|......./../-...../../.../....................\..............|..............\......\....\.\.|............
........|-..........\........................|.......|..-.................|....\...........................\\.
./...............\..............|/................................|.........\......................\..........
.\...........|-..-................-.........../...................................../.....\...................
.-.......|../.........|........|...............................|....../......\....|.........\/....../.........
\.............|................|..../............-.....................\..\.....-|.....-.....|................
............../...\....\/......-.....................................-....-...................|...../.........
........./.............\.............................|.........-..........-.........-.........\.........-.....
.............../.....-...\..\.........\....-......./....\...............\......\-..../........................
.|.......-.|......................|......-....|\|......|...|..............\..........-...\.........|..........
.................../........./..........\../..................../...............|..\.......--.......\.........
..............-..../...................-..../../............-/..|.........................-......|............
.|....|........................|..\............|....-..../|............-.|.|..................|...............
.......//.....\..............|../..............................-...........\|..-..................\....|..|../
........-................|............../....|....-..............-...............|.......\..............|./\..
.............................|.|....../.........../.....\....|.............../...................\.....|......
...../...............|........../../..\..................\/..........................-......-........\..|.....
....../....................../..............\...\.......-.....|/.........-....................................
...........-.\./.../..\.........................\..................................../..|.............-.......
.......|............|............/......................|.\............/....\.\..........-.....-.......-.-....
..............\.....-.\..\....-.../............................-.........|.|...........................|..\...
.../...\..................|........../../..../............./..\..................................|............
..--....|.................................................-..........|../....|.../....../................./...
........./.....................\...................../.|\....||..-../.....-.|.........................-.......
............./......./.....\.........\....-.............-..........\....................-...\.....|...../...-.
..../.-..........\.......\..|.............|........|..-............................./.|............./...-....-
....../....../.....................\......-.........\..........-....\......-............\.|...................
.............................\................\.......-.......................|......|........................";
        }


    }
}
