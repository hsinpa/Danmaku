using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathExpParser;

public class MathParserRouter : MonoBehaviour
{

    Dictionary<string, List<MathExpParser.Token>> formulaCache = new Dictionary<string, List<MathExpParser.Token>>();
    Dictionary<string, float> answerCache = new Dictionary<string, float>();
    Dictionary<string, float> _keywordCache = new Dictionary<string, float>();
    public Dictionary<string, float> keywordCache
    {
        get
        {
            return _keywordCache;
        }
    }

    MathExpParser.MathParser mathParser;

    private static MathParserRouter s_Instance;
    public static MathParserRouter Instance
    {
        get
        {
            if (s_Instance != null)
            {
                return s_Instance;
            }

            s_Instance = FindObjectOfType<MathParserRouter>();
            if (s_Instance != null)
            {
                return s_Instance;
            }

            return null;
        }
    }
    public void Start()
    {
        mathParser = new MathExpParser.MathParser();
    }

    public float CalculateAnswer(string p_formula)
    {
        List<MathExpParser.Token> tokens = new List<Token>(GetCacheToken(p_formula));
        //Debug.Log("formula " + p_formula + ", angular_velocity " + tokens.Count);

        if (!answerCache.ContainsKey(p_formula))
        {
            var answer = mathParser.ParseShuntingYardToken(tokens, _keywordCache);
            answerCache.Add(p_formula, answer);
        }

        return answerCache[p_formula];
    }

    public List<MathExpParser.Token> GetCacheToken(string p_math_expression)
    {
        if (!formulaCache.ContainsKey(p_math_expression))
        {
            var tokens = mathParser.ParseMathExpression(p_math_expression);
            formulaCache.Add(p_math_expression, tokens);

        }
        //if (p_math_expression != "0")
        //    mathParser.TokenToStringLog(formulaCache[p_math_expression]);
        return formulaCache[p_math_expression];
    }

    public Dictionary<string, float> GetUniversalKeyword()
    {
        EditKeyValue("t", Time.time);
        EditKeyValue("d", Time.deltaTime);

        return _keywordCache;
    }

    public void EditKeyValue(string p_key, float p_value)
    {
        if (!_keywordCache.ContainsKey(p_key))
        {
            _keywordCache.Add(p_key, p_value);
        }
        else
        {
            _keywordCache[p_key] = p_value;
        }
    }

    public void Refresh()
    {
        _keywordCache = GetUniversalKeyword();
        answerCache.Clear();
    }

}
