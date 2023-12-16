using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeTAF
{
    public class Test : MonoBehaviour
    {
        [SerializeField]
        private bool partTwo = false;
        [SerializeField]
        private bool useTestInput = true;
        [SerializeField]
        private bool run = false;
        private string input;

        void part1() {
            AocLib.TestPrint(input);


        }

        void part2() {

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
@"replaceTestInput";
        }

        string Input() {
            return
@"replaceInput";
        }


    }
}
