using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathExpParser
{
    public class SampleScript : MonoBehaviour
    {

        public string raw_math_expression;

        public void Execute() {

            MathParser mathParser = new MathParser();
            Dictionary<string, float> testDict = new Dictionary<string, float>
            {
                {"p", 5 },
                { "t", 3}
            };

            mathParser.debugMode = true;
            mathParser.SetVariableLookUpTable(testDict);
            mathParser.ParseMathExpression(raw_math_expression);

            //MathParserThreading.Instance.CalculateAsyn(raw_math_expression, (MathParserThreading.ParseResult result) =>
            //{
            //    Debug.Log("Answer "  + result.answer);
            //    Debug.Log("Length " + result.tokens.Count);
            //}, testDict);

        }

    }
}