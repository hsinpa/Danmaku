using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathExpParser
{
    public class ASTNode
    {
        public Token token;
        public ASTNode leftChildNode;
        public ASTNode rightChildNode;

        public ASTNode(Token token, ASTNode leftChildNode = null, ASTNode rightChildNode = null)
        {
            this.token = token;
            this.leftChildNode = leftChildNode;
            this.rightChildNode = rightChildNode;
        }

        public float Solve()
        {
            if (token._type == Token.Types.Number) {
                return float.Parse(token._value);
            }

            float leftNumber = (leftChildNode == null) ? 0 : leftChildNode.Solve();
            float rightNumber = (rightChildNode == null) ? 0 : rightChildNode.Solve();

            switch (token._value) {
                case "+":
                    return leftNumber + rightNumber;

                case "-":
                    return leftNumber - rightNumber;

                case "/":
                    return leftNumber / rightNumber;

                case "*":
                    return leftNumber * rightNumber;

                case "^":
                    return Mathf.Pow(leftNumber, rightNumber);

                default:
                    return 0;

            }
        }


    }
}