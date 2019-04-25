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
        ShuntingYardParser _shuntinYardParser;

        public MathParser() {
            _tokenizer = new Tokenizer();
            _shuntinYard = new ShuntingYard();
            _shuntinYardParser = new ShuntingYardParser();
        }
        

        public void Parse(string raw_syntax)
        {
            var tokens = _tokenizer.Parse(raw_syntax.ToLower());

            var tokenList = _shuntinYard.Parse(tokens);

            RPNToStringLog(tokenList);
            Debug.Log("Answer " + _shuntinYardParser.Parse(tokenList));
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