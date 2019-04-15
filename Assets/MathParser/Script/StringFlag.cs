using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MathExpParser
{
    public class StringFlag
    {
        public class RegexSyntax {
            public const string IgnoreSpace = "\\s";

            public const string IsNumber = "\\d";
            public const string IsOperator = "\\+|-|\\*|\\/|\\^";
            public const string IsVariable = "[a-z]";
        }
    }
}