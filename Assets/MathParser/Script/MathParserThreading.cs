using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if !UNITY_WEBGL
using System.Threading;
#endif

namespace MathExpParser
{
    public class MathParserThreading : MonoBehaviour
    {
        #region Parameter

        //Singleton
        private static MathParserThreading s_Instance;
        public static MathParserThreading Instance
        {
            get
            {
                if (s_Instance != null)
                {
                    return s_Instance;
                }

                s_Instance = FindObjectOfType<MathParserThreading>();
                if (s_Instance != null)
                {
                    return s_Instance;
                }

                return null;
            }
        }

        private MathParser mathParser;
        private Queue<TaskResult> results = new Queue<TaskResult>();

        #endregion

        void Start()
        {
            mathParser = new MathParser();
        }

        public void CalculateAsyn(string math_pression, System.Action<ParseResult> p_callback, Dictionary<string, float> customLookUpTable = null)
        {
#if !UNITY_WEBGL
            RunAyns(delegate { Calculate(math_pression, p_callback, customLookUpTable); });
#else
            Calculate(math_pression, p_callback, customLookUpTable);
#endif
        }

        public void FastCalculateAsyn(List<Token> tokens, System.Action<ParseResult> p_callback, Dictionary<string, float> customLookUpTable = null)
        {
#if !UNITY_WEBGL
            RunAyns(delegate { FastCalculate(tokens, p_callback, customLookUpTable); });
#else
            FastCalculate(tokens, p_callback, customLookUpTable);
#endif
        }

        private void Calculate(string math_pression, System.Action<ParseResult> p_callback, Dictionary<string, float> customLookUpTable = null)
        {

            List<Token> parsedTokens = mathParser.ParseMathExpression(math_pression);
            float answer = mathParser.ParseShuntingYardToken(parsedTokens, customLookUpTable);

            ParseResult parsedResult = new ParseResult(answer, parsedTokens);

            results.Enqueue(new TaskResult(parsedResult, p_callback));
        }

        private void FastCalculate(List<Token> tokens, System.Action<ParseResult> p_callback, Dictionary<string, float> customLookUpTable = null) {
            float answer = mathParser.ParseShuntingYardToken(tokens, customLookUpTable);

            ParseResult parsedResult = new ParseResult(answer, tokens);

            results.Enqueue(new TaskResult(parsedResult, p_callback));
        }

#if !UNITY_WEBGL
        private void RunAyns(System.Action p_task)
        {
            Thread t = new Thread(new ThreadStart(p_task));
            t.Start();
        }
#endif

        void Update()
        {
            if (results.Count > 0)
            {
                int itemsInQueue = results.Count;
                lock (results)
                {
                    for (int i = 0; i < itemsInQueue; i++)
                    {
                        TaskResult result = results.Dequeue();

                        if (result.callback != null)
                            result.callback(result.parseResult);
                    }
                }
            }
        }

        #region Data Structure
        private struct TaskResult
        {
            public System.Action<ParseResult> callback;
            public ParseResult parseResult;
            public TaskResult(ParseResult p_parseResult, System.Action<ParseResult> p_callback)
            {
                parseResult = p_parseResult;
                callback = p_callback;
            }
        }


        public struct ParseResult
        {
            public float answer;
            public List<Token> tokens;

            public ParseResult(float answer, List<Token> tokens)
            {
                this.answer = answer;
                this.tokens = tokens;
            }
        }
        #endregion

    }
}