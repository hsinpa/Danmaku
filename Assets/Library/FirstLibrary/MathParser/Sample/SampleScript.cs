using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathExpParser
{
    public class SampleScript : MonoBehaviour
    {

        public string raw_math_expression;

        Dictionary<string, float> testDict = new Dictionary<string, float>
            {
                {"p", 5 },
                { "t", 3}
            };

        MathParser mathParser;

        public void Execute() {

            mathParser = new MathParser();

            mathParser.debugMode = false;
            mathParser.SetVariableLookUpTable(testDict);

            mathParser.CalculateAsyn(raw_math_expression, (float answer) =>
            {
                //Debug.Log("Async Answer " + answer);
            });

            float syncAnswer = mathParser.Calculate(raw_math_expression);

            Debug.Log("Sync Answer " + syncAnswer);


            //MathParserThreading.Instance.CalculateAsyn(raw_math_expression, (MathParserThreading.ParseResult result) =>
            //{
            //    Debug.Log("Answer "  + result.answer);
            //    Debug.Log("Length " + result.tokens.Count);
            //}, testDict);

        }

        private void Start()
        {
            mathParser = new MathParser();
            mathParser.SetVariableLookUpTable(testDict);
        }

        private void Update()
        {
            mathParser.CalculateAsyn(raw_math_expression, (float answer) =>
            {
                //Debug.Log("Async Answer " + answer);
            }, new Dictionary<string, float>() { { "t", Time.time } });

            //float syncAnswer = mathParser.Calculate(raw_math_expression);

            //Debug.Log("Sync Answer " + syncAnswer);

        }

    }
}