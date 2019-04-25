using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathExpParser
{
    public class ShuntingYardParser
    {

        #region Private Parameters
        private Stack<float> outputStack;

        /// <summary>
        /// string = function value
        /// int = needed input length
        /// </summary>
        private Dictionary<string, int> FunctionLookUpTable = new Dictionary<string, int> {
            { "sin", 1},
            { "cos", 1},
            { "tan", 1},
            { "arcsine", 1},
            { "arccos", 1},
            { "min", 2},
            { "max", 2},
            { "rand", 2},
        };

        #endregion

        public ShuntingYardParser() {
            outputStack = new Stack<float>();
        }

        public float Parse(List<Token> shuntingYard_tokens) {
            outputStack.Clear();

            try
            {
                foreach (Token t in shuntingYard_tokens)
                {
                    if (t._type == Token.Types.Number)
                        outputStack.Push(float.Parse(t._value));

                    else if (t._type == Token.Types.Operator)
                    {
                        float rightInput = outputStack.Pop(),
                            leftInput = outputStack.Pop();

                        outputStack.Push(ComputeOperatorToken(t, leftInput, rightInput));
                    }
                    else if (t._type == Token.Types.Function) {
                        if (FunctionLookUpTable.ContainsKey(t._value))
                        {
                            float[] inputArray = new float[FunctionLookUpTable[t._value]];
                            for (int i = 0; i < inputArray.Length; i++)
                                inputArray[i] = outputStack.Pop();

                            System.Array.Reverse(inputArray);
                            outputStack.Push(ComputeFunctionToken(t, inputArray ));
                        }
                        else {
                            Debug.LogError("Function " + t._value + " is not define");
                            break;
                        }

                    }
                }
            }
            catch {
                Debug.LogError("Encounter incorrect syntax");
            }

            return outputStack.Pop();
        }

        private float ComputeOperatorToken(Token token, float leftInput, float rightInput) {
            switch (token._value)
            {
                case "+":
                    return leftInput + rightInput;

                case "-":
                    return leftInput - rightInput;

                case "/":
                    return leftInput / rightInput;

                case "*":
                    return leftInput * rightInput;

                case "^":
                    return (float)System.Math.Pow(leftInput, rightInput);

                default:
                    return 0;
            }
        }

        private float ComputeFunctionToken(Token token, float[] input)
        {
            switch (token._value)
            {
                case "sin":
                    return Mathf.Sin(input[0]);

                case "cos":
                    return Mathf.Cos(input[0]);

                case "tan":
                    return Mathf.Tan(input[0]);

                case "arcsine ":
                    return Mathf.Asin(input[0]);

                case "arccos ":
                    return Mathf.Acos(input[0]);

                case "min":
                    return Mathf.Min(input);

                case "max":
                    return Mathf.Max(input);

                case "rand":
                    return Random.Range(input[0], input[1]);

                default:
                    return 0;
            }
        }

    }
}