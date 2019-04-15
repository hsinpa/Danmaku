using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
namespace MathExpParser
{
    public class Tokenizer
    {

        public List<Token> Parse(string p_raw_expression) {
            List<Token> tokens = new List<Token>();

            p_raw_expression = Regex.Replace(p_raw_expression, StringFlag.RegexSyntax.IgnoreSpace, "");
            UnityEngine.Debug.Log(p_raw_expression);


            for (int i = 0; i < p_raw_expression.Length; i++) {
                string part = p_raw_expression[i].ToString();

                if (IsNumber(part))
                {
                    tokens.Add(new Token(part, Token.Types.Number));
                }
                else if (IsVariable(part))
                {
                    tokens.Add(new Token(part, Token.Types.Variable));
                }
                else if (IsOperator(part))
                {
                    tokens.Add(new Token(part, Token.Types.Operator));
                }
                else if (isLeftParenthesis(part))
                {
                    tokens.Add(new Token(part, Token.Types.LeftParenthesis));
                }
                else if (isRightParenthesis(part))
                {
                    tokens.Add(new Token(part, Token.Types.RightParenthesis));
                }
                else if (IsComma(part))
                {
                    tokens.Add(new Token(part, Token.Types.ArgumentSeperator));
                }
            }
            UnityEngine.Debug.Log(tokens.Count);

            return tokens;
        }


        #region Qualifier Method
        private bool IsComma(string p_char)
        {
            return (p_char == ",");
        }

        private bool IsNumber(string p_char)
        {
            UnityEngine.Debug.Log(p_char);

            return Regex.IsMatch(p_char, StringFlag.RegexSyntax.IsNumber);
        }

        private bool IsVariable(string p_char)
        {
            return Regex.IsMatch(p_char, StringFlag.RegexSyntax.IsVariable);
        }

        private bool IsOperator(string p_char)
        {
            return Regex.IsMatch(p_char, StringFlag.RegexSyntax.IsOperator);
        }

        private bool isRightParenthesis(string p_char)
        {
            return (p_char == ")");
        }

        private bool isLeftParenthesis(string p_char)
        {
            return (p_char == "(");
        }

        #endregion

        public void Debug(List<Token> tokens) {
            foreach(Token t in tokens) {
                UnityEngine.Debug.Log(t._type.ToString("g") +" , " + t._value);
            }
        }
    }
}