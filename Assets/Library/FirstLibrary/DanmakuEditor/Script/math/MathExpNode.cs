using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DanmakuEditor
{
    [CreateNodeMenu("Danmaku Editor/Formula/MathExpNode")]
    public class MathExpNode : FormulaNode
    {
        [TextArea(3, 10)]
        public string formula;
    }
}