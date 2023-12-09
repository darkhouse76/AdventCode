using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeTAF
{
    public class AoC10 : MonoBehaviour {
        [SerializeField]
        private bool partTwo = false;
        [SerializeField]
        private bool useTestInput = true;
        [SerializeField]
        private bool run = false;
        private string input;

        private int register;
        private int cycleCount;
        private int signalStrengths;
        private int[] targetCycles = { 20, 60, 100, 140, 180, 220 };
        private char[] CRTscreen;

        void Cycle(int numCycles = 1,int registerChange = 0) {

            for (int i = 0; i < numCycles; i++) {
                cycleCount++;
                if (targetCycles.Contains<int>(cycleCount)) {
                    signalStrengths += (cycleCount * register);
                }
            }

            register += registerChange;
        }

        void CycleP2(int numCycles = 1, int registerChange = 0) {

            for (int i = 0; i < numCycles; i++) {

                int curScreenX = cycleCount - (40 * (cycleCount / 40));

                if (register - 1 <= curScreenX && curScreenX <= register + 1) {
                    //in sprite range
                    CRTscreen[cycleCount] = '#';
                }
                cycleCount++;               
            }
            register += registerChange;
        }


        void part1() {

            string[] instructions = input.Split("\r\n");

            register = 1;
            cycleCount = 0;
            signalStrengths = 0;
            int cyclesToExcuteAddx = 2;
            //int cyclesToExcuteNoOp = 1;

            for (int i = 0; i < instructions.Length; i++) {
                string[] instruction = instructions[i].Split();

                if (instruction.Length > 1) {
                    //it's addX
                    Cycle(cyclesToExcuteAddx, int.Parse(instruction[1]));
                }
                else {
                    //it's noop
                    Cycle();
                }
            }

            print($"Total Signal Strength = {signalStrengths}");

        }

        void part2() {
            //note the result had to be put into a basic text for it to proper allign instead of using unity log. 
            string[] instructions = input.Split("\r\n");

            CRTscreen = new char[240];
            Array.Fill<char>(CRTscreen, '.');

            register = 1;
            cycleCount = 0;

            int cyclesToExcuteAddx = 2;            

            for (int i = 0; i < instructions.Length; i++) {
                string[] instruction = instructions[i].Split();

                if (instruction.Length > 1) {
                    //it's addX
                    CycleP2(cyclesToExcuteAddx, int.Parse(instruction[1]));
                }
                else {
                    //it's noop
                    CycleP2();
                }
            }

            string screen = "";

            for (int i = 0; i < CRTscreen.Length; i ++) {               
                
                if (i%40 == 0) { screen += "\n"; }
                screen += CRTscreen[i];
            }

            print(screen);
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
@"addx 15
addx -11
addx 6
addx -3
addx 5
addx -1
addx -8
addx 13
addx 4
noop
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx -35
addx 1
addx 24
addx -19
addx 1
addx 16
addx -11
noop
noop
addx 21
addx -15
noop
noop
addx -3
addx 9
addx 1
addx -3
addx 8
addx 1
addx 5
noop
noop
noop
noop
noop
addx -36
noop
addx 1
addx 7
noop
noop
noop
addx 2
addx 6
noop
noop
noop
noop
noop
addx 1
noop
noop
addx 7
addx 1
noop
addx -13
addx 13
addx 7
noop
addx 1
addx -33
noop
noop
noop
addx 2
noop
noop
noop
addx 8
noop
addx -1
addx 2
addx 1
noop
addx 17
addx -9
addx 1
addx 1
addx -3
addx 11
noop
noop
addx 1
noop
addx 1
noop
noop
addx -13
addx -19
addx 1
addx 3
addx 26
addx -30
addx 12
addx -1
addx 3
addx 1
noop
noop
noop
addx -9
addx 18
addx 1
addx 2
noop
noop
addx 9
noop
noop
noop
addx -1
addx 2
addx -37
addx 1
addx 3
noop
addx 15
addx -21
addx 22
addx -6
addx 1
noop
addx 2
addx 1
noop
addx -10
noop
noop
addx 20
addx 1
addx 2
addx 2
addx -6
addx -11
noop
noop
noop";
        }

        string Input() {
            return
@"noop
addx 5
noop
noop
noop
addx 1
addx 2
addx 5
addx 2
addx 5
noop
noop
noop
noop
noop
addx -12
addx 18
addx -1
noop
addx 3
addx 5
addx -5
addx 7
noop
addx -36
addx 18
addx -16
noop
noop
noop
addx 5
addx 2
addx 5
addx 2
addx 13
addx -6
addx -4
addx 5
addx 2
addx 4
addx -3
addx 2
noop
addx 3
addx 2
addx 5
addx -40
addx 25
addx -22
addx 25
addx -21
addx 5
addx 3
noop
addx 2
addx 19
addx -10
addx -4
noop
addx -4
addx 7
noop
addx 3
addx 2
addx 5
addx 2
addx -26
addx 27
addx -36
noop
noop
noop
noop
addx 4
addx 6
noop
addx 12
addx -11
addx 2
noop
noop
noop
addx 5
addx 5
addx 2
noop
noop
addx 1
addx 2
addx 5
addx 2
addx 1
noop
noop
addx -38
noop
addx 9
addx -4
noop
noop
addx 7
addx 10
addx -9
addx 2
noop
addx -9
addx 14
addx 5
addx 2
addx -24
addx 25
addx 2
addx 5
addx 2
addx -30
addx 31
addx -38
addx 7
noop
noop
noop
addx 1
addx 21
addx -16
addx 8
addx -4
addx 2
addx 3
noop
noop
addx 5
addx -2
addx 5
addx 3
addx -1
addx -1
addx 4
addx 5
addx -38
noop";
        }


    }
}
