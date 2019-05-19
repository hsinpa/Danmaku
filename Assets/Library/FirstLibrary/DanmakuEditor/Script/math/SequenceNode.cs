using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DanmakuEditor
{
    [CreateNodeMenu("Danmaku Editor/Formula/SequenceNode")]
    public class SequenceNode : FormulaNode
    {
        public float[] sequence_value;
    }
}