using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeTAF
{
    public class AOC2306 : MonoBehaviour
    {
        [SerializeField]
        private bool useTestInput = true;
        [SerializeField]
        private bool run = false;
        private string input;


        void Main() {
            //change to real data
            int[] raceTime = { 48, 93, 85, 95 };
            int[] raceDist = { 296, 1928, 1236, 1391 };

            int totalWins = 1;

            for (int i = 0; i < raceTime.Length; i++) {
                int possibleWins = 0;
                for (int buttonHold = 1; buttonHold < raceTime[i]; buttonHold++) {
                    if (buttonHold * (raceTime[i] - buttonHold) > raceDist[i]) {
                        //beat dist
                        possibleWins++;
                    }
                }
                totalWins *= possibleWins;
            }

            print($"Total Possible Wins *= {totalWins}");

        }


        void Update() {
            if (run) {
                run = false;
                Debug.Log("========================================================================");

                if (useTestInput) { input = InputTest(); }
                else { input = Input(); }
                Main();
            }
        }


        string InputTest() {
            return
@"replace";
        }

        string Input() {
            return
@"replace";
        }


    }
}
