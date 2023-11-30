using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

namespace CodeTAF
{
    public interface IAoc
    {



        void Main();

        void run() {
            Main();
        }
        
    }
}
