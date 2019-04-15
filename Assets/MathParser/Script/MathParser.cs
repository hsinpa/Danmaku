using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathExpParser
{
    public class MathParser
    {
        Tokenizer _tokenizer;

        public MathParser() {
            _tokenizer = new Tokenizer();
        }
        

        public void Parse(string raw_syntax)
        {
            var tokens = _tokenizer.Parse(raw_syntax);
            _tokenizer.Debug(tokens);
        }
    }
}