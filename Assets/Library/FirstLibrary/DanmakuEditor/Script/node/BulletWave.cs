using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using System.Linq;

namespace DanmakuEditor
{
    public class BulletWave : XNode.Node
    {

        [Input(connectionType = ConnectionType.Multiple)] public DanmakuEditor.BulletPattern[] patterns;

        [SerializeField]
        private string default_pattern_id;

        public DanmakuEditor.BulletPattern GetDefaultPattern()
        {
           return GetBulletByID(default_pattern_id);
        }

        public DanmakuEditor.BulletPattern GetBulletByID(string p_id)
        {
            if (patterns.Length <= 0) return null;
            if (string.IsNullOrEmpty(p_id)) return patterns[0];

            var patternList = patterns.ToList();
            BulletPattern s = patternList.Find(x => x._id == p_id);

            if (s == null) return patterns[0];

            return s;
        }

        protected override void Init()
        {
            base.Init();
            name = "BulletWave";
        }

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            patterns = GetInputValues<DanmakuEditor.BulletPattern>(to.fieldName, patterns);
        }

        public override void OnRemoveConnection(NodePort port)
        {
            patterns = GetInputValues<DanmakuEditor.BulletPattern>(port.fieldName, patterns);
        }

    }
}