using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathExpParser
{
    public class ASTNode
    {
        Token token;
        ASTNode leftChildNode;
        ASTNode rightChildNode;

        public ASTNode(Token token, ASTNode leftChildNode, ASTNode rightChildNode)
        {
            this.token = token;
            this.leftChildNode = leftChildNode;
            this.rightChildNode = rightChildNode;
        }
    }
}