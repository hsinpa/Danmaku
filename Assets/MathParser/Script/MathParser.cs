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

        public MathParser() {
            _tokenizer = new Tokenizer();
            _shuntinYard = new ShuntingYard();
        }
        

        public void Parse(string raw_syntax)
        {
            var tokens = _tokenizer.Parse(raw_syntax);

            var rpn_tokens = _shuntinYard.Parse(tokens);

            ToStringLog(rpn_tokens);
        }

        public void ToStringLog(List<Token> tokens)
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