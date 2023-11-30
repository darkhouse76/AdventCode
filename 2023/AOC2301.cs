using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeTAF
{
    public class AOC2301 : MonoBehaviour
    {
        [SerializeField]
        private bool run = false;

        void Main() {

        }


        void Update() {
            if (run) {
                run = false;
                Debug.Log("========================================================================");

                Main();
            }
        }


        string InputTest() {
            return "replace";
        }

        string Input() {
            return "replace";
        }


    }
}
