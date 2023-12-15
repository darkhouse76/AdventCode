using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace CodeTAF
{
    public class AOC2313 : MonoBehaviour
    {
        [SerializeField]
        private bool partTwo = false;
        [SerializeField]
        private bool useTestInput = true;
        [SerializeField]
        private bool run = false;
        private string input;



        //expecting reflectPos to be x.5
        bool CheckReflect(char[,] mirorArray, float ReflectPos, bool checkVertical) {           

            float distFromReflect = 0.5f;
            int checkPos = (int)(ReflectPos + distFromReflect);
            int checkNeg = (int)(ReflectPos - distFromReflect);

            if (!partTwo) {

                if (checkVertical) {
                    do {
                        for (int Line = 0; Line < mirorArray.GetLength(1); Line++) {
                            if (mirorArray[checkPos, Line] != mirorArray[checkNeg, Line]) {
                                return false;
                            }
                        }

                        distFromReflect++;
                        checkPos = (int)(ReflectPos + distFromReflect);
                        checkNeg = (int)(ReflectPos - distFromReflect);

                    } while (checkPos < mirorArray.GetLength(0) && checkNeg >= 0);
                }
                else {
                    do {
                        for (int Line = 0; Line < mirorArray.GetLength(0); Line++) {
                            if (mirorArray[Line, checkPos] != mirorArray[Line, checkNeg]) {
                                return false;
                            }
                        }

                        distFromReflect++;
                        checkPos = (int)(ReflectPos + distFromReflect);
                        checkNeg = (int)(ReflectPos - distFromReflect);

                    } while (checkPos < mirorArray.GetLength(1) && checkNeg >= 0);

                }
            }
            else {

                bool correctedSmudge = false;
                if (checkVertical) {
                    do {
                        for (int Line = 0; Line < mirorArray.GetLength(1); Line++) {
                            if (mirorArray[checkPos, Line] != mirorArray[checkNeg, Line]) {
                                if (correctedSmudge) {
                                    return false;                                                                        
                                }
                                correctedSmudge = true;                                
                            }
                        }

                        distFromReflect++;
                        checkPos = (int)(ReflectPos + distFromReflect);
                        checkNeg = (int)(ReflectPos - distFromReflect);

                    } while (checkPos < mirorArray.GetLength(0) && checkNeg >= 0);
                    if (!correctedSmudge) { return false; } //didn't actually correct a smudge
                }
                else {
                    do {
                        for (int Line = 0; Line < mirorArray.GetLength(0); Line++) {
                            if (mirorArray[Line, checkPos] != mirorArray[Line, checkNeg]) {
                                if (correctedSmudge) {
                                    return false;
                                }
                                correctedSmudge = true;
                            }
                        }

                        distFromReflect++;
                        checkPos = (int)(ReflectPos + distFromReflect);
                        checkNeg = (int)(ReflectPos - distFromReflect);

                    } while (checkPos < mirorArray.GetLength(1) && checkNeg >= 0);
                    if (!correctedSmudge) { return false; } //didn't actually correct a smudge
                }
            }

            return true; //maybe i guess might work???
        }

        //check Vertical and Horizontal Reflection positions until valid one. 
        int FindReflect(char[,] mirorArray) {          

            //check Vertical
            for (int pos = 0; pos < (mirorArray.GetLength(0)-1); pos++) {
                if (CheckReflect(mirorArray,(pos + 0.5f), true)) {
                    //print($"Valid Reflect is Vertical and at {pos + 1}");
                    return pos + 1;                    
                }                
            }

            //check Horizontal
            for (int pos = 0; pos < (mirorArray.GetLength(1) - 1); pos++) {                
                if (CheckReflect(mirorArray, (pos + 0.5f), false)) {
                    //print($"Valid Reflect is Horizontal and at {pos + 1}");
                    return (pos + 1) * 100;
                }
            }

            return 0; //shouldn't ever happen.
        }

        char[,] GetArray(string[] mirrors) {

            char[,] mirrorArray = new char[mirrors[0].Length,mirrors.Length];

            for (int line = 0; line < mirrors.Length; line++) {
                for (int column = 0; column < mirrors[line].Length; column++) {
                    mirrorArray[column,line] = mirrors[line][column];
                }
            }

            return mirrorArray;
        }

        void part1() {

            //parse the info
            string[] mirrorsLines = input.Split("\r\n");

            List<char[,]> mirrorArray = new List<char[,]>();

            int prevIndex = 0;
            int curIndex = Array.IndexOf(mirrorsLines, "");

            do {
                mirrorArray.Add(GetArray(mirrorsLines[prevIndex..curIndex]));

                prevIndex = curIndex + 1;
                curIndex = Array.IndexOf(mirrorsLines, "", prevIndex);
            } while (curIndex > 0);

            mirrorArray.Add(GetArray(mirrorsLines[prevIndex..]));    
            

            int totalResult = 0;

            for (int curArray = 0; curArray < mirrorArray.Count; curArray++) {
                //print("CHECK ARRAY = " + curArray);
                totalResult += FindReflect(mirrorArray[curArray]);
            }

            print($"Final Total Result= {totalResult}");

        }

        void part2() {
            part1();
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
@"#.##..##.
..#.##.#.
##......#
##......#
..#.##.#.
..##..##.
#.#.##.#.

#...##..#
#....#..#
..##..###
#####.##.
#####.##.
..##..###
#....#..#";
        }

        string Input() {
            return
@"#....#.
#....##
.####..
.####..
#....##
#.##.#.
.####.#

.#...####.##..#
..#.###...#.##.
..#.###...#.##.
.#...####.##..#
.####..##.##..#
.####.#.###.##.
####..#.#######
#.......####..#
####.#.#.......
..#....##.#####
.##.##...##.###
..#.#.#.##.....
.....##.#..####
##..#.###..####
.##..#...######

..#..#.
#....#.
...##..
.##.#..
.####..
..#.#.#
....###
#..#.#.
#..#.#.
....###
..#.#.#
.####..
.##.#..

##.###...#.##
##.###...#.##
###.#####..##
##....#.##.##
#....#.#.####
.#....###....
#....#...####
.##.##.####..
..#.##.###.#.

#.#...#...##...##
#.#...#...##...##
#....##....#.....
##...##...##.##..
#..##..####....##
.....#.##..#.###.
#.......###..#...
..####.#.#.###..#
#..###.#.#.#####.
.....#.###....###
.....#.####...###

##....##.
##.##.###
.#.##.#.#
..#..#..#
........#
........#
..#..#..#
.#.##.#.#
##.##.###
##....##.
.##..#...

##..#..
...####
..#...#
..###..
##.####
##.#.#.
......#
....#.#
##.#.#.

##.#.##.#
...#..#..
.##...#..
.##...#..
.#.#..#..
##.#.##.#
.#...####
..#.#..##
...#...##
.#....#.#
.#....#.#

#.######.#..###
.#..##....#..##
####..####.##..
###.##.####....
####..#######..
.#......#.#.#..
.#......#..#...
#..#..#..#...##
#.#.##.#.##.###

.......##
#####.#..
#..##.#..
#######.#
....#####
.##......
####.#...
######.##
.....####
#..#.#...
.##.#..##

#.######.#.#.
##.##.###...#
#.....#....##
.#.......###.
.#.......###.
##....#....##
##.##.###...#
#.######.#.#.
#.#.##.#.#.##
..#.########.
..#.########.
#.#.##.#.#.##
#.######.#.#.

#.#.##.
..##..#
.#.##..
#..##.#
#...#.#
....##.
.#..##.
..#.#.#
..#.#.#
.#..##.
....##.
#...#.#
#..##.#

...#..#.##..#
..#.#..#.####
..#.#..#.####
...#..#.##..#
###..#..###..
...##....####
##..#..####..
##..#.##....#
#..#..#.#.#.#
##...#..#...#
...###..#.###

.##..#.##
##.###.##
##.###...
##..#.###
..#.#..##
##.###...
...######
###..####
..#...#..
..#....##
..####.##

......###..##
###.######.#.
..###...#..##
...##..#.....
...##.##.####
###..#.##.#.#
###..#.##.#.#
...##.##.####
...##..#....#
..###...#..##
###.######.#.

#..#.####
.#.#.###.
.###.###.
#..#.####
#...#...#
#...#...#
#..#.####

#.#...##.
##.##....
..#.##..#
..#.#.##.
#.##.#.##
###.##..#
####.....
##..#....
##.##....
##.##....
##..#....
####.....
###.##..#

#.#.#.##.
...#.#.##
...###.##
#.#.#.##.
#####..##
.#..##..#
####.#.##
....#.##.
.#...#.#.
.####.#.#
#..#.#.#.
..######.
..#..#...
#.#.#####
#.#.#####

.#.######
#...#....
#....####
#....####
#...#....
.#.######
##...#..#
###..#...
#.#.#####
..#.#....
.#.#..##.
.##.#....
.####.##.
#.##.####
#..######

###....###...
...#..#....#.
..........###
.###..###.#.#
...#..#...###
###.##.####..
###.##.###..#
#.#....#.#.#.
...####.....#
##......####.
##......##.#.
#.#.##.#.##..
#.##..##.#.##
#.#....#.#.##
#.#.##.#.##..
##......##.#.
##......####.

#...#.##..###
.#.#####.####
#..#..#######
#....##..#.##
.#.###..##.##
.##...##.....
.##.####.####
.##.####.####
.##...##.....
.#.###..##.##
##...##..#.##
#..#..#######
.#.#####.####

..#..####
#...#..#.
..#.##.#.
.#####...
#.##....#
#.##....#
.#####...
..#.##.#.
#...#..#.
..#..####
..##.####

.#.#.#..##.##
.#.####.....#
..##......#..
###.#..##..##
###.#..##..##
..##.#....#..
.#.####.....#
.#.#.#..##.##
.#.#.#..##.##

....#...###....##
.#.#..##..##..###
.#.#..##..##..###
....#...####...##
####.#..#...##...
......#.#..##...#
#....##.#.##...##
#..####.#....##.#
#..####.#....##.#
#....##.#.##...##
......#.#..##...#

#.#.#.#.##.##
#.#.#.#.##.##
.#..#.#.##.##
####.##.#...#
.###.##..###.
.###.#.#.##.#
#......##.##.
.##.#..###.##
####....####.
####....####.
.##....###.##
#......##.##.
.###.#.#.##.#

######.......
######.......
......#######
#####.##.#..#
####..#..#.##
##..##..#####
##..#####.###
#..#...##.#..
.###.##..#.##

#....##..
#######.#
#....#.##
.#..#..#.
##..##.#.
##..###..
##..###..
#....###.
......#.#
##..##...
##..##...
......#.#
#....#.#.

.........####
#..##..#.#...
.........#...
........##...
.#.##.#..####
#......######
#.####.#.####
.#....#.#.#..
...##....##..
#.#..#.##..##
#..##..#.....
.........#...
..#..#.......
.######.##.##
##....#.#....

....########.....
#..##......##..#.
#..##......##..#.
....########.....
..#.#..##..#.#..#
###.#......#.###.
#..#.##..##.#..#.
.....#...##......
#...###..###...#.
...##########...#
......####.......
...#...##...#....
##..########..##.

#..#####..#
###.#######
######.....
....#.#####
........##.
#..#..#.##.
######.#..#

###.###
##.#.#.
##.###.
..##.#.
...#...
####.#.
..#...#
######.
##....#
..####.
##.#...
##.###.
..#####
..##.#.
###..##
###..#.
..##.#.

.#.##.#.#
...#.....
...#.....
.#.##.#.#
.#.#.#..#
..#..#.#.
#..##.##.
.#.#..##.
##.....#.
###.#...#
###.#...#
##.#...#.
.#.#..##.
#..##.##.
..#..#.#.

...#.#..#
###.#####
.#.#.....
###.##..#
.##.#.##.
.#.#.#..#
.#...#..#
###..####
###.#....
###.#....
###..####
.#...#..#
.#.#.#..#
.#..#.##.
###.##..#

..#..#...#.
##....##...
.##..##.#.#
#......#.#.
##....###.#
#.#..#.##.#
.#.##.#..#.
#.####.#..#
#.####.#.##

#.##.##
.#..#..
#.##.##
.####..
..##.##
.####..
##..###
.####..
#....##
.#..#..
..##...

..#.#.#..
#...#..#.
#...#..#.
..#.#.#.#
..#.#.#.#
#...#..#.
#...#..#.
..#.#.#..
.###.....
.####.###
..#.###..
...#.#...
#.##....#
..####...
#####..#.
.#.##.#..
##..##.#.

.###.###..##...##
...####..#.##..#.
..#.#..##...###.#
#..#.##..####...#
.#..###...#.##.#.
.##..##.#.##.#.##
.##..##.#.##.#.##
.#..###...#.##.#.
#..#.##..####...#
..#.#..##...###.#
...####..#..#..#.
.###.###..##...##
..####...#..####.
..####...#..####.
.###.###..##...##

..##..###
##..##.#.
#######..
......#..
..##...##
#.##.##.#
......###

#######.#.###.##.
..##....##..#.#..
..##....##..#.#..
#########.###.##.
#..#####........#
.#....###.####...
.....####.##..#..
#..#.#.##...#....
#..#.#.##...#....

########.####..
#......#.#..###
#......#.#..###
########.####..
#.#.#.####..#..
..#...##...####
##...#.#..#.###
#.#.#.#......##
.##....##...###
####..#.##.###.
.#..###.#..#.##

.####....
.#...#.#.
.#...#.#.
.####....
##.#...##
#.##....#
#..#..#..
###..###.
###.###..
#.###.#.#
#.###.#..

##..##.#.
.####.###
#....###.
.......##
#.##.#..#
##..###..
......##.
.####.###
##..##...
#....###.
##..###..
#.##.#..#
#.##.#.##

#.....#..##
#.....#..##
.#.#..###..
..#.#..##.#
..##.######
#...#...#..
#.##.#.....
...###.#...
#.#...#.###
..#.##.#...
#..#.###.##
.##...##...
#.#..##..##
###.#######
..###...#..
..#.##.##..
##..##...##

#..##..
..#.##.
#.#.#..
#.#.#.#
..#.##.
#..##..
.#...#.
.###..#
.###..#

..##....#..##.###
.####....#.##..#.
.####....#.#...#.
..##....#..##.###
.......#.##.#####
.####....#####.##
##..##.###...##..
.......##...#.#.#
##..##..####....#
.####..##.##.####
########......###
.#..#...#.#.###.#
#.##.###.##......
..##...#.##.##...
#.##.##.#.#...#.#

#.#.#.###
..##....#
..#..#...
###.#.##.
.####....
#.####.#.
##.####..
..###.###
..##..###
#..####..
#..####..
..##..###
..###.###

.#.....
#..####
.##.##.
###.###
.....##
.....##
###.###
.##.##.
#..####
.#...#.
.#...#.
#..####
.##.##.

....##....#.##...
#........##......
..#.##.#...#...##
##......##.###.##
##.####.##...####
..##..##......#..
##.#..#..#..#..##
..#....#..###.###
.#......#..##.#..

#..#..###...#
#.#.##.######
#.#.##.######
#..#..###...#
####..#..##..
...#.#.#..##.
#...#.##...##
###.###.##..#
#..##.#...#..
#..##.#...#..
###.###.##..#
#...#.##...##
.#.#.#.#..##.

###.#.####.
####.#####.
##########.
###.#.####.
....#.#.#.#
...#.......
....#.#####
#####.#.##.
##.....###.
....#...#.#
##.#.##.#..
##.....####
..#.#...#.#
##......#..
.....#.#.#.

###....#..##.
####...#..##.
###.......#..
#..##....##..
##.##...#..#.
.#...#.##..#.
.#...####..#.
#...#..##.###
##..######.##
#.##.........
####..##.##.#
###...##.###.
###...##.###.

.#..###..##..
.#..###..##..
###....#....#
...#.####.#..
#.#..###...#.
..#.##.#.##..
..#.#..#.##..
#.#..###...#.
...#.####.#..
###....#....#
.#..###..##..

#..####..##
.########..
.#..##..#..
####..#####
..#....#...
.#.####.#..
#..####..##
#...##...##
.###..###..
.#.#..#.###
.#.####.#..

.#..##.
.#..##.
##.#.##
##.##.#
.....##
#.#.#.#
.#.###.
.#.###.
###.#.#
.....##
##.##.#
##.#.##
.#..##.

#.....#..#.
####.#....#
.##...#..#.
#...#.#..#.
#.###..##..
##.#...##..
###.#..##..
#.#.###..##
###...#..#.
.##.#......
.####......

###.###..##
###.###..##
##.#..##.#.
.#.#.#.#..#
..#....#.##
###.##..#.#
......#####
..###....#.
#.......#..
#.#.#.###..
.#.##.###..
..#.#....##
###..###...
#...##.##.#
#...##..#.#

####..######..#
.###..#.##.#..#
....##..##..##.
#.#....#..#....
..##..#.##.#..#
#.##..#.##.#..#
#...#..#..#..#.
####..##..##..#
.###..######..#

.#.####.#..
.########..
.##.#..##..
#.#....#.##
#..#..#..##
#.#.##.#.##
.###..###..
#.#.##.#.##
..##..##...
.#.#..#.#..
.###..###..

####.##
.#....#
##.#.#.
..##.##
...#.##
#####..
.#.####
.#.####
#####..
...#.##
..##.##

#..####
.###.##
#..##..
#..####
.#...##
.###.##
####.##

.........##....
###..####..####
.#....#..##..#.
.##..##.####.##
.##..##......##
..####...##...#
.######......##
##.##.###..###.
.##..##.#..#.##
#..#...######..
..####........#
.##..##.####.##
.######..##..##

....#.##.##..
#.##.#.......
#..##.#.#.###
.##....###.##
#..###..#...#
....###.#####
#..##....##.#
####.#...####
.##.##...##.#
.##.##...##.#
####.#...####

#......##
.#.######
##.#.#...
#.###.###
..#####..
#......##
#......##
..#####..
#.###.###
##.#.#...
.#.######
#....#.##
##.#.####

###....#.
#..####.#
....##.##
.##.##.#.
.##.#...#
.##....##
.......#.
.......##
.##.##...
####...#.
.##.#.#..
.##..###.
.##..###.

#.##..#..#.
....###..##
#.#....##..
##.##..##..
##.#.#....#
..#...####.
..#...####.
##.#.#....#
##.##..##..
#.#....##..
....###..##
#.##..#..#.
.#.##.#####
#....##..##
..#.###..##

..#..##.#
....####.
...#...#.
#..##..#.
.#.##...#
#.#.#....
#...#.###
#..##.###
#.#.#....
..##.##..
....#####
....#####
..##.##..
#.#.#....
#..##.###
#...#.###
#.#.#....

##########...
###...#..#...
###.#.#..#...
##########...
##.#######.##
##.....#...#.
#####..#..##.
##.###.......
...#..##.#..#
..##...##.##.
####.#.#.##..

####.###..#
####.##.#.#
....###..##
##...#.#.#.
###.#..#..#
###.#..#..#
##...#.###.

..###...##...
...##.######.
##..###....##
...#.#......#
...###......#
##.###.####.#
.#....######.
.....#..##..#
..###########
###.#.######.
###..#......#

###.###.###...##.
##.##.######..##.
.#####..#.#.##..#
.##.#..##.##.####
..##..##.#...####
.###..##.#...####
.##.#..##.##.####
.#####..#.#.##..#
##.##.######..##.

....##.
#.#.#.#
..##...
.#.#.##
.#.#.##
..##...
#.#.#.#
....##.
##..#..
.#..#..
....##.

..###......
.#.##...##.
##.#..###..
##.#..###..
.#.##...##.
..###..#...
..####..#.#
.##...#...#
.##...#...#
..####..#.#
..###..#...
.#.##...##.
##.#..###..

..##..#.#..#.#.
##..#####..####
##..####....###
.#..#..######..
##..####....###
.#..#.########.
##..##........#
..##....####...
#.##.#.#.##.#.#
######.#....#.#
.####....##..#.
.......#....#..
.####.#.#..#.#.

..#...#######
.#..##..####.
....#..#.##.#
.....##.####.
#...##.......
##..##.######
#.#.#....##..
.###.##.#..#.
.##..#.#.##.#
.##..###.##.#
.###.##.#..#.

.###..#...##..###
##...#.#.#....#..
##...#.#.#....#..
.###..#...##..###
#.###..##....#..#
#..##.#######.#.#
..####..#.####...
.#...#.##...##.#.
.#...#.##...##.#.
..####....####...
#..##.#######.#.#
#.###..##....#..#
.###..#...##..###

####.#...
.....####
#######.#
....#...#
.....#.##
....##.##
####.#..#
....#.#.#
.....#...
.....###.
......###
.##...#.#
....#..##

##.#.######.#
....#..#..##.
..#####.###.#
###.###..#.##
###.####.####
..#.##.#..#.#
#####...#..##
####.#....#.#
###..#....#.#
#####...#..##
..#.##.#..#.#
###.####.####
###.###..#.##

.#.#..##..#.#..
..###.....##...
...###..###....
#.##......##.##
.##........##..
.#...#..#...#..
####.#..#.#####
##.###..###.###
.#.#.#..#.#.#..
.#.#.####.#.#..
....######.....
.###..##..###..
##..#.##.#..###
.##.######.##..
.#..........#..
##..######..###
.#..........#..

#.....#.###..
..##..##...#.
###.##.#..#..
.#.###.####.#
.#.###.######
.#.###.######
.#.###.####.#

##.##....##
##.##....##
#.###....##
##..#.##.#.
#.###.#####
.#.#.#..#.#
#.##.####.#
..##.#..#.#
###.##..##.
#...######.
##..#.##.#.
#....####..
....#....#.
.#.#..##..#
#...#....#.

#..####
#...###
.#..#..
#....##
.#.....
.##..##
..#.#..
.....##
#.##.##
.#.##..
.#.####
#....##
#..#...

##...#...
.###.##.#
#.###.#.#
###..####
###..####
#.###.#.#
.###.##.#
###..#...
###..#...
.###.##.#
#.###.#.#

....####..###
...#####..###
.#.#..######.
...##..#..#..
#.#####....##
#.....##..##.
#....########
.#.#....##...
....##..##..#
.##...######.
#.##.#.#..#.#
......######.
#...#...##...

##.#.#.#...
..#.###..##
##....#..##
#.#.##.##..
#.#.##.##..
##....#..##
..#.###..##
##.#.#.#...
.......###.
.......###.
.#.#.#.#...
..#.###..##
##....#..##

##.#..#
###...#
#...##.
##.....
..#....
.##....
##.####
##.####
.##....
..#....
##.....

#..##..##..#..#..
#.########.#..#.#
.#...##...#.##.#.
..###..###......#
.###.##.###.##.##
.#.######.#.##.#.
##..#..#..######.
.###.##.###.##.##
.#..#..#..#.##.#.
.#..####..#.##.#.
...#....#........
....#..#.........
....#..#.........
..##.....#......#
.#.#.##.#.#.##.#.
#.#..##..#.####.#
###..##..###..###

##.##.#######
..#.....#..#.
#.###....##.#
.##.###......
..#.###.#..#.
..#.###.#..#.
.##.###......

##..#..##..
###.#.#..#.
#.####....#
.....##..##
#####..##..
....#.#..#.
..##.######
##.##.####.
#.#.#..##..
.####.####.
#.....####.
#...#.####.
.####.####.
#.#.#..##..
##.##.####.

##.###....####.
####.#.###.##.#
.##.###....##..
.##.#.###.#..#.
.....###.######
......#.###..##
.##.##.#.......
#####..####..##
#..#...########
####.######..##
....#..########

..#..#.###..#
.###.#...#..#
..#.###.#####
.##..##.#.##.
.##..##.#.##.
..#.###.#####
.###.....#..#
..#..#.###..#
.#..##....##.
##.##.#...##.
#.##.#...####

..###.#####
.#.########
.#####..#.#
.#....##.#.
.#....##.#.
.#####..#.#
.#.########
#.###.#####
#.###.#####
.#.########
.#####..#.#

.#..#.#.#
......##.
##..##.#.
##..##...
......##.
.#..#.#.#
##..##..#
##..###..
######.##
#######..
#.##.###.

.######
#......
#......
.##..##
.##..##
#......
#.#..#.

.#.....
#.....#
..##.#.
#.##...
..####.
#.#.#..
#.#.#..
..####.
#.##...
..##.#.
#.....#
.#.....
...#...
...##..
#......
#......
.#.##..

.....#.
#.##...
#.##...
..#..#.
..#..#.
#.##...
#.##...
.....#.
##..#.#

...##....
#.#..##..
#..###.##
###......
.#..#..##
#....#...
.##.#####
..#......
.#.....##
#.#..#.##
..##.####
..###.###
..#.#.###

.##..#.###.##
.##..#.###.##
.#...#..##.##
...#.#.#.##..
#..####.#...#
#.##.#...##..
.##.#.#..#.#.
....#.#####..
#.##.###...#.
..###.#####.#
..###.#####.#
#.##.###...#.
....#.#####.#

.##.##.#.###.
#..##.##..#.#
#.#######.##.
.....#..###..
.#######.#.#.
.#####.#.#.#.
.....#..###..
#.#######.##.
#..##.##..#.#
.##.##.#.###.
##.#.###.####
##.#.###.####
.##.##.#.###.

###..###.
.#....#..
.#....#.#
.#....#.#
.#....#..
###..###.
.#....#..
##.##.###
...##....
########.
#.####.##
#.####.##
.######.#
..####...
.#.##.#.#
#......#.
..#####..

..#..#..###
#.#.#...###
#.#.#...###
..#..#..###
#...#..##..
#..##.#....
.#.###.#.##
#.....#....
##.##.##..#
#..##.##..#
#.....#....";
        }


    }
}
