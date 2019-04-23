using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace MathExpParser
{
    public class MathParser
    {
        Tokenizer _tokenizer;
        ShuntingYard _shuntinYard;
        ShuntingYardOrigin _shuntinYardOrigin;

        public MathParser() {
            _tokenizer = new Tokenizer();
            _shuntinYard = new ShuntingYard();

            _shuntinYardOrigin = new ShuntingYardOrigin();
        }
        

        public void Parse(string raw_syntax)
        {
            var tokens = _tokenizer.Parse(raw_syntax.ToLower());
            //ToStringLog(tokens);
            //var rpn_tokens = _shuntinYard.Parse(tokens);

            var tokenList = _shuntinYardOrigin.Parse(tokens);

            RPNToStringLog(tokenList);
            //Debug.Log("Answer " + rpn_tokens.Solve());
            //rpn_tokens.Render();
        }

        public void TokenToStringLog(List<Token> tokens)
        {
            for (int i = 0; i < tokens.Count; i++)
            {
                Debug.Log(i + " => " + tokens[i]._type + "(" + tokens[i]._value + ")");
            }
        }

        public void RPNToStringLog(List<Token> tokens)
        {
            string groupString = "";
            foreach (Token t in tokens)
            {
                groupString += t._value + " ";
            }

            Debug.Log(groupString);
        }
    }
}