using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DanmakuEditor
{
    public abstract class FormulaNode : XNode.Node
    {
        [Output(connectionType = ConnectionType.Override)] public FormulaNode node;
        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "node")
            {
                return this;
            }
            return null;
        }
    }
}