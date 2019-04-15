using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathExpParser
{
    public class SampleScript : MonoBehaviour
    {

        public string raw_math_expression;

        public void Execute() {

            new MathParser().Parse(raw_math_expression);


        }

    }
}