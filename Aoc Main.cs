using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace CodeTAF
{
    public class AocMain : MonoBehaviour
    {
        [SerializeField]
        private bool useTestInput = true;
        [SerializeField]
        private bool run = false;
        private string input;


        void Main() {

        }

        
        void Update()
        {
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
