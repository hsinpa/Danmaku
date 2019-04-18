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

            Debug.Log(rpn_tokens.token._value);
            Debug.Log(rpn_tokens.rightChildNode.token._value);
            Debug.Log(rpn_tokens.leftChildNode.token._value);

            //Debug.Log(rpn_tokens.rightChildNode.rightChildNode.token._value);
            //Debug.Log(rpn_tokens.rightChildNode.leftChildNode.token._value);

            //Debug.Log(rpn_tokens.leftChildNode.rightChildNode.token._value);
            //Debug.Log(rpn_tokens.leftChildNode.leftChildNode.token._value);

            Debug.Log("Answer " + rpn_tokens.Solve());


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