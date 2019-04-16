using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathExpParser
{
    public struct Token
    {
        public string _value;

        public Types _type;

        public Token(string value, Types type)
        {
            _value = value;
            _type = type;
        }

        public enum Types {
            Number,
            Variable,
            Operator,
            LeftParenthesis,
            RightParenthesis,
            ArgumentSeperator,
            Function
        }


    }
}

