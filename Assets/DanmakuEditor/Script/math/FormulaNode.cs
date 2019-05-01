using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DanmakuEditor
{
    public class FormulaNode : XNode.Node
    {
        [Output(connectionType = ConnectionType.Multiple)] public string node;

        [TextArea(3, 10)]
        public string formula_expression;

        public override object GetValue(NodePort port)
        {
            // Check which output is being requested. 
            // In this node, there aren't any other outputs than "result".
            if (port.fieldName == "node")
            {
                // Return input value + 1
                return formula_expression;
            }

            return null;
        }

    }
}