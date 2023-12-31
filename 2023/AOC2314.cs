using System;
using UnityEngine;

namespace CodeTAF
{
    public class AOC2314 : MonoBehaviour
    {
        [SerializeField]
        private bool partTwo = false;
        [SerializeField]
        private bool useTestInput = true;
        [SerializeField]
        private bool run = false;
        private string input;
        [SerializeField]
        private bool doSteps = false;

        int curLine = 0;        
        DateTime startTime;


        const char ROCK_ROUND = 'O';
        const char ROCK_CUBE = '#';
        const char EMPTY_SPOT = '.';
        const int TARGET_CYCLES_AMT = 1000;

        char[,] platformMap;




        char[,] ParseInput(string[] input) {
            Array.Reverse(input);

            char[,] output = new char[input[0].Length, input.Length];
            for (int line  = 0; line < input.Length; line++) {
                for (int col = 0; col < input[line].Length; col++) {
                    output[col,line] = input[line][col];
                }
            }
            return output;
        }

        char[,] TiltUp(char[,] platformMap) {

            int platformMaxX = platformMap.GetLength(0);
            int platformMaxY = platformMap.GetLength(1);

            for (int row = platformMaxY-1; row >= 0; row--) {
                for (int col = 0; col < platformMaxX; col++) {
                    //found round rock.
                    //print($"Col = {col} : Row = {row}");
                    if (platformMap[col,row] == ROCK_ROUND ) {
                        //print("FOUND ROCK");
                        for (int i = (row + 1); i < platformMaxY; i++) {
                            if (platformMap[col,i] != EMPTY_SPOT) {
                                //found rock above
                                //print("FOUND GRAV SPOT = " + platformMap[col, i] + $"@ {col},{i}");
                                platformMap[col, row] = EMPTY_SPOT;
                                platformMap[col, i - 1] = ROCK_ROUND;
                                
                                break;
                            }
                            else if (i == (platformMaxY - 1)) {
                                platformMap[col, row] = EMPTY_SPOT;
                                platformMap[col, i] = ROCK_ROUND;                                
                            }
                        }                        
                    }
                }
            }

            return platformMap;
        }

        char[,] TiltDown(char[,] platformMap) {
            int platformMaxX = platformMap.GetLength(0);
            int platformMaxY = platformMap.GetLength(1);

            for (int row = 0; row < platformMaxY; row++) {
                for (int col = 0; col < platformMaxX; col++) {
                    //found round rock.                    
                    if (platformMap[col, row] == ROCK_ROUND) {                        
                        for (int i = (row - 1); i >= 0; i--) {
                            if (platformMap[col, i] != EMPTY_SPOT) {
                                //found rock above                                
                                platformMap[col, row] = EMPTY_SPOT;
                                platformMap[col, i + 1] = ROCK_ROUND;
                                break;
                            }
                            else if (i == (0)) {
                                platformMap[col, row] = EMPTY_SPOT;
                                platformMap[col, i] = ROCK_ROUND;
                            }
                        }
                    }
                }
            }

            return platformMap;
        }

        char[,] TiltRight(char[,] platformMap) {
            int platformMaxX = platformMap.GetLength(0);
            int platformMaxY = platformMap.GetLength(1);

            for (int row = 0; row < platformMaxY; row++) {
                for (int col = platformMaxX - 1; col >= 0; col--) {
                    //found round rock.                    
                    if (platformMap[col, row] == ROCK_ROUND) {
                        //print("FOUND ROCK");
                        for (int i = (col + 1); i < platformMaxY; i++) {
                            if (platformMap[i, row] != EMPTY_SPOT) {
                                //found rock above                                
                                platformMap[col, row] = EMPTY_SPOT;
                                platformMap[i - 1, row] = ROCK_ROUND;

                                break;
                            }
                            else if (i == (platformMaxY - 1)) {
                                platformMap[col, row] = EMPTY_SPOT;
                                platformMap[i, row] = ROCK_ROUND;
                            }
                        }
                    }
                }
            }

            return platformMap;
        }

        char[,] TiltLeft(char[,] platformMap) {
            int platformMaxX = platformMap.GetLength(0);
            int platformMaxY = platformMap.GetLength(1);

            for (int row = 0; row < platformMaxY; row++) {
                for (int col = 0; col < platformMaxX; col++) {
                    //found round rock.                    
                    if (platformMap[col, row] == ROCK_ROUND) {
                        for (int i = (col - 1); i >= 0; i--) {
                            if (platformMap[i, row] != EMPTY_SPOT) {
                                //found rock above                                
                                platformMap[col, row] = EMPTY_SPOT;
                                platformMap[i + 1, row] = ROCK_ROUND;
                                break;
                            }
                            else if (i == (0)) {
                                platformMap[col, row] = EMPTY_SPOT;
                                platformMap[i, row] = ROCK_ROUND;
                            }
                        }
                    }
                }
            }

            return platformMap;
        }
        //this might just be unnecessary
        char[,] TiltCycle(char[,] platformMap) {

            platformMap = TiltUp(platformMap);
            platformMap = TiltLeft(platformMap);
            platformMap = TiltDown(platformMap);
            return TiltRight(platformMap);            
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

        int GetTotalLoad(char[,] platformMap) {
            int totalLoad = 0;

            for (int row = 0; row < platformMap.GetLength(0); row++) {
                int rowMul = row + 1;
                for (int col = 0; col < platformMap.GetLength(1); col++) {
                    if (platformMap[col,row] != ROCK_ROUND) { continue; }

                    totalLoad += rowMul;
                }                
            }
            return totalLoad;
        }

        void part1() {
            char[,] platformMap = ParseInput(input.Split("\r\n"));

            PrintMap(platformMap);
            print("#########");


            platformMap = TiltUp(platformMap);
            PrintMap(platformMap);

            print($"Total Load = {GetTotalLoad(platformMap)}");

        }

        

        void part2() {
            char[,] platformMap = ParseInput(input.Split("\r\n"));         

            for (int i = 0; i < TARGET_CYCLES_AMT; i++) {
                platformMap = TiltCycle(platformMap);
            }

            print($"Total Load = {GetTotalLoad(platformMap)}");


        }

        private void Start() {
            curLine = 0;
            doSteps = false;
            
            platformMap = ParseInput(Input().Split("\r\n"));

        }

        void Update() {
            if (run) {
                run = false;
                Debug.Log("========================================================================");

                if (useTestInput) { input = InputTest(); }
                else { input = Input(); }

                startTime = System.DateTime.Now;
                if (partTwo) {
                    //part2();
                    curLine = 0;                    
                    doSteps = true;
                }
                else { part1(); }
                print($"Took {System.DateTime.Now - startTime} to complete.");

            }

            if (doSteps) {
                platformMap = TiltCycle(platformMap);
                //print(curLine);
                print(GetTotalLoad(platformMap));
                if (++curLine %1000000 == 0) {
                    print($"DONE %{(curLine*100)/TARGET_CYCLES_AMT} of Cycles");
                    //print($"Took {System.DateTime.Now - startTime} to complete.");
                }
                if (curLine == TARGET_CYCLES_AMT) {
                    print($"Total Load = {GetTotalLoad(platformMap)}");
                    doSteps = false;
                    print($"Took {System.DateTime.Now - startTime} to complete.");
                }                
            }
        }        

        string InputTest() {
            return
@"O....#....
O.OO#....#
.....##...
OO.#O....O
.O.....O#.
O.#..O.#.#
..O..#O..O
.......O..
#....###..
#OO..#....";
        }

        string Input() {
            return
@"..#..#......#...O.O#O#..#O.O#.O..O.O.#.O..OO.O......###...#..#....OO..O.#.....#.O...#.#OO#O#..O.O#O.
#.#..OOO..#..#...O......#.###O..O.O...#..........#..O......O.#O#......#....O..O#OO.........OO.##O...
..O.#....O.O...O#O...#.##O#OO.#OO.....#.O....O....#OO.O#....O.O.#.......O......O#.O.O...#.....#O.O.O
#OO.O...#.#......#.......##.O#...OO..O....#...#OO....O...#.#....O#.#.#.O.....#..O..O..#.#...O.#.O..O
O##O...O.#O....#.#O#........O....#.#.O.O#O..O...O.#OO.O###.#.#.O..........O##O#......O..##....#O##..
O..#..........O....O.O.O.OO.OO.OOOO...#..OOO..O..#O.##...#..OOOO.O####....O#....#O##.O.........O...O
.##...OO..O..#...O.O...O#.O.O.O..OO..O..##O.##...#.OOO.#O.....O...O....OOO.#O...#O#.......#.O.OO.O..
.O....O..#.OO...###....#.##...O.O#...O.#.O.#....O...#..#.#O.O.#.....O#O#O..OO..#.O.O...OO.O..##.#.O.
.#.##O#.OO....#..###.#.O...#.#O.OO.#..O..O......O...#....OO..##.#O#O.O..O.....#.#..#..O.#...#O....O.
...#.O..#OO...O#....O...#......O#.#..O.O#OOO#.#OO..O.....#.....#O.O#O...O.#.#..#O.#...##O..#..O.O#..
.##....O#.......##........#.#.O...O.O#.#.O........O......#.#O.O...O##.....OO..#O..O#.O....#...#O....
.#O..O.#..O#OO#..O.......#.O#O#...O...#O.O...OO#....#....O....#....O#O..OO....O.......#.....O.O#O##.
#....#..#.#..O#.#.OO....##...#O.O###.....O.##O#OO#......O.....O#.#O..O...#...#.O.O.O.#...#.....#.OO#
.#..OO.#...##.O...#......O...#O#.O.#O.#.#....#.#.....OO#.OO.......#O...OO.O.#....#.......#.#.##....O
...O....O.##O.O....#O...#....#OOOO......#..O..O..##O....#O.......O.#.O...O#..#O.O...#O.##..O....O...
#..#...##.##..#..O.#...#..O..#.....#...O.......#O#...O.#..#O#.#...#..O#....OO....O#..#O..O.O#.O#..#.
.OO.......##...O...#.#....#O#.#O..#.#O.#......O.O.#.......#O.O........#.#.O.#.OO.#..OO#..#O..O.#...#
...O..O..O#...#......O...#.OO.O....O.........O#O.O..#.#O..##..........O.O..#OO.O.#O..O...O...#......
...O.#...#.O##.#..OO#OO..#....O.#...#........#...O#...O..#..O..##.....O..##.O..#..#.OO...O.#.#.OO.#.
..O.#..O.O..#......O.....OO..O..OO.O.....O....#....##....OO....#..O#....O....#O#....#...##.....O#..#
OO.....#.##.OO#........O..##.....O.#.O#.....#.OO.OO.O...O.OO...#....#.#.O#.#...#OO...#..O#...O#O.O..
##...O#.#.O............O....#OOOOOO.....#.O#OO#O.OO....O#.#.O............#...OO.....#.....O#.....O..
..O...#....O##.O#.#.O#O...O..O...O.O....O..#..#...OO.O.OO..O.#.#..#..O#.....O.##........O......O....
...O.#.O...O.##..#.....O.#.........OO#....O......#OOOOO..........##.O......#.......##.##..O.#.....O#
#...#..#O..#.O.#....O..#.O.OOO#..O...#.#..O.OO.#...##..O..#.#..##.OOO....#..O.O.O.#.OO.O.O#..O#.....
..O.#......#..#.##.#.#.O.#.#.....O..#..O.#O#.#..#...O.O.O#.##....#.O.O.O....O..#.O.O......O..O......
..#O......O#.....#..#.#.....#.#.#.....#..O..........O...#.##.......O.#.....O...O##..O.O...#.....O.OO
..O...O.##.......#...#...#...O.........#...O...#.O.O....#.OO..#..OO...O...OOO#O...OO.O#..O.OO.....#O
.....O..#..OOO......##O..O.OO.#....O..O.##.#O..#..O.OOO...O#...OO.O..#.#.O#....#O..O..##.O....O.O..#
O#..O..........O....O##..OO....O.O..O.OOOO....O.......#...#O.#.###O.............#....O.O##..#O.....#
......#.OO#....O..O.........O..#.#O#O....O....OO#O..#.....#.O.#.#.O.OO...O.........#.##..OOO..#...#.
...#..O.....O##.....#O.O.O#....OOO.OO..O.OO.....#O.##O#.#...O.#..#....#..#..O.O...#O.O.##...O###O.#.
...O.#O#....#.#O..O....O.O..#...#.#..#......#....O.....#......##...O....#.#...#..O.....#....O#.....O
O.O#..#.O.O.....#..#O.....#..#O....#....O.O.O..#..O###O.#..O#...O##.O.......#........O..O#..OO..#...
#O..O..##OO....OO.O.....#...O...O...#..O#.#.......#O.OO##.....O.O...O...OOOO...OO....O......O.#....#
OOOO....O..##..OO....O#..##..OOO.#..O.OO.O#O....O.#.#.....OOO.##....O.O.O..#OO..O...#...#O.O.OO...O.
.#..OO.O...##..#......O..........#.O.....#........###..O#..#.O.O#.....O..O..O.#...O....#.O..O......O
....O.#O..#O.#..O...###..OO...O.OOO..#...O...OO.....#.O.#O..O........#..#...##.OOO.OO..#..O...#....O
..OO.....O#........OO#O.O.........#..#.....#.......#.O..#......#O#.O.O.O..OO..O.......O...O......#O.
##....O.....#..#.OOO..O.#.O.###..O...OOOO.#.......O..#...O.O#OO.O#...O....#.O.O..#.....O...O#...#..#
.O#..#..O..#O#.#.........O......O.O##...#..#...O....O..O.#....#...O..OOO..O....#..#.OO.O#.....#....#
.O#.OO.#.....#.O.#.O##O#......O......O.#O..#O#....#..OO.OO#.O.O.O..#OO#.#........###..........##....
...#O#....#....OO.....O.#..O..O....#..OOO##.......O......##..#...O#...#O#........O.....#..#.O.#.#.O#
.O.O....O.O....OO#.#...O#.OO..O.#...#...#...#.#.......#..#.O..#...O#..O...O..#..#.O.O.....O..#O.O..#
....O.O#...OO##OO....O.OO...O#...#.....O....OO..O.O...#....OO....O#.#.OO.O...........#.#.O..O.#.....
O.O.#.......#O.#O.....#...#OO....#.OO...#O.#..OO#.O....#.O#..........#OO#.......#O..#...#....O#.....
#.#..#.O...O...OOO.#.#..O....O..##O.O......#.O.#......O....O..#O......#O..#..O..O...O#..O......#O#.O
#.......#......O........O...##..#.#.O...O.......#..O......O#O.........#OO#...O.#...O..#.....#...#...
#...#..#.....O#.O#O..O#......#O...O..##....##....####..O......O..##..OO...........O#.......OO..O##..
..#O.#O...O.#....O#...#...#....#...#.......O.O.O..#...#.O.O....O..O.....O#O....###..O..O.#.OO##..O#.
O......#..........#.#..O###...O...#.#........#.O..O......#O#O.......#.#.O.#..OO#O.O..##.#.....O..O.O
.O.O.O#.O...O...O..#O.....O.#OOO..O.O.....O.#.#.#.#...#.......#O........O.#O.O...#.O.#..O...O.#O...O
.O....#O.O.#...###...O........O#O..#.O.OO#..OO...O.O......#..O.##..O#.#.#.#...O#O.......#..O.#.#O..#
..#...O..###O#..O..#...O...#OO...O.#..#.O.........OOOO.O...........#..O......#...#.#...##...O.......
....O#.OO...#....O#...#O.......OO.OOO..O......#......#...........####...O.##O.#.....O......#.......O
O...O..O.O.#O.##...OO#.O#O.#...#.#...OO...#O.O.OO.....#.................##.#..O.O.O..O.O.#.......O.O
O.O.#..#...O.O....O........##.#.....OO....O..O.......#..#O.#OO.OO.....##....O.O##....#.....#O.....OO
O.#.....#.......#O..O#......O..O..O#.O..O...#.O...##O#....O.#O....#O#...O......OO.O..O.#O#.#O..#....
#O.O#....#.....#O........#........#..O.OOO.O......#O#.....#.##.....##.#OO#.O......OO.OOOO.#.O#....O.
O.............#.....OO..OO.#.O#.#.......OO.......O#....#.#.#..........O..#...O.....O#..O...O.O#...#.
O.#O#.........#.#..O#O#..OOO#O#...#....##.#.O.##.O....OO....O......O.O#.O...O#O.#.##..........#OOO.O
...O.......OO...O#O#..O...O...O..OO..O..#..O.........##........O#....O..#.OO...OO...OO..#.##O#....#.
#....#O....O.#..O...O..#O#.....#OO.O#..O..#.....#..##........O..#O#O..........O....#OO..O#...O.O...O
O........O.O.O#..O....O###......O..##O.....O.....#O..O.#.....##.....#OO...OO...#..O#.#...#O.OO##.O..
.#O.........##.......#O......O#..OOOOOO#O....#..O#.#O#.......O.O...O...O#...#....O....#..#...#O.#.OO
.O.O...#.....OO.#.O..#OO.....#.........O....OO.....#.#.#.#.#..O...##...O..O...OO....#.O.......OO....
##OO.OOO#.#O#..O....O.O.O...O...O#..#O...O..#......OO.O..#...#O..O.#.OOOO.O...O......#.##O.#O.#...##
OO...O....#.#...#.#.O.#..#..O.O.O..#...#......O.......O......O.O.O......O.....OO##.#..O.............
....#.O.#......#..OO......O#.O.#...O.O.#.O...#O.....#..#............O..#O#.#.O..OO...O.OO..O.#...OO#
O#O..#O...#..OO............#.O..#..O.##OO.....OO.##..O.......O.#.O.....O...OO...OO.O.#......O...O#..
OO.##.#OO#O.......OO.O...#.OOO...O#....O.O#.O....OO.#..O..OO#OO.#....#..O##O.O.O....OOOO...###..#..O
....OO...#..O..........O.....O..O#.O.......##O.##OOO..OOO...O#.O.O...O..O#O..O.#O.#..O#.....O...OOO.
#..OOOO.#O......#...........O.##..##....#...O##O#......O.O..#...#O#....OO.#..OO..#..OO#.O...##......
.#O.....#O..O.#O.O.##.#.....OO....O#..##....O#..O.....#...#.O..OOO...##....OO...O..O...OO...OO...#O.
....O..O#.O#....OO#..#.#....OO##.O#O.##.....O........O#O...#.........O.....#.#.O.O...OO...O....O#.#.
...........O#OO#...#O.O..OO..O...O.....O.O..#.............O..O............#.O..O#...........#..#....
O.#.O........O.O..#..#O.#O.#......#.O#.O##..#O#O..O#O....#.##..#O....#........O.....#...#...O.......
.#.#O.O....OO.........O..#.....OOO.#...##.O.O.....#.OO..#...O.O........O#...O...O..#O...O.#......O..
....O..O...#O.O.OO.O.#...O#...O..#..O...#O#.#.....OO#..O..#...O#.O..#OO..#...OO.OO.#.#.....#....#.O#
..OOO.#......O..O.......#.......#...#....O.O#...O#.OO...#..#OO.O....#.#.#..O....O.....O.O.O#...O.O..
O......O.#.O.OO..O..#..O..#.....OO.O.OOOO#..O#....O.#..#...#.##.OO.....O.O.#.#.#.O.#O...OO.O.#.#...O
O...OO..###.O##........O..O.#.....OO..#....#.#.#..#.#O...O...###...#O..#O........#..#O.#.....###.OOO
.O..###O........OO###......#....##.....O.OO.....O.#...#.....O##O..OO.#..O...O#....OO#....O..#..#.OO.
.O..O..O#......O..O...###.....O..#.#....#........#....#.#..O#...O.#.#####O#O.......#O..O..##..O..O..
.OOO.OO.O###...........#.......O.O......O.....O...O.....OO.O#............#.OO.........OO...O#......#
....#.O#...#.#..OOO.#O.O.O...OO....#...##..O..OOO.......OOO....O#.O....O..O.###..O..O..#...##..O....
.......#O#.O..#...O#.O.O..O..O..O.O...OO.....O.O......#......#..##O.....O##...O...#.O..##..##.O...#O
OOOO..##............OO..O.....O...O.#.....#..#...O.#.O..O.#.##.....O..#..#...#O..OO....O..O...O.....
O..#OO.OO.....OO.O........#...O.O...OO.#.#......#....O...#OO...OO..#.O...#.O.......O...O...O#O##.O##
O.O.O.OO...##....#.......#...#......#..#O.......##.#.#O..#OO......O......#....OO...OO.#.....O.O..##.
O..O......##.....O...#...#.OO..OO.......###.#.......#.....O#OOO..#O.OO#......O..O.#.O..O..OO...#..#.
.....O...#.##.#.##...O#.#..O..OO.#O##O....O#O.O..OO.#O....O....#OO..O.O...OO...O....#....O.##O#...O.
.#.#....#..OOO.....#.....##.##..........#.O...##..O.#O.......OOOO#O.O.O#.O..O..O....O.....#......OO.
O..O##OO#....#O.....OO.O.O.##O.O............OOO..O#.....O...#O...#...........#OO..#.##.#.#.....O#...
.##.OO......#..##..O..OO...O..#...O..#O#.OO.O.OOO...#.OO..O.OO#..O.......#..#..##...O.O##O..OO...O..
......#....O..#O.#.O.#.#O.O......#.O.....O##.O......#....#.###O.O#..O#O.....O..O..O..##..#O#..O..O..
#O....#..O.#OO....#.....#....O.#...#.#..O..#.##..#.O.O.O......#.......#OO..O.#.O#OO.##.O.......OO..#
##....#...#.O..#.#..OOO...O...O..##...O.O..#..#...#.....O#.#O...O..#.#.....O....O#......O#O.O...O#O#
#..........O#...#.O#O.O.O.#..#O.O..#.#O.#.O......O.#......#.......O..#.#..#..##O..O.#...#..O#.##.O.O
O..#.O...O.........#...#........#.O..OO..O.#O.##.O..O..OO#.OO#O..OO...O#.O.#..#.#O#.#......O.#...#.O";
        }


    }
}
