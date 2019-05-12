using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DanmakuEditor
{
    public class BulletPattern : XNode.Node
    {
        [Input(connectionType = ConnectionType.Multiple)] public BaseBullet[] bulletType;
        [Output(connectionType = ConnectionType.Multiple)] public BulletPattern patterns;

        public string _id = "default_id_01";

        public string name = "JustAnotherBullet";
        public float duration;
        public string next_pattern_id;

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "patterns")
            {
                return this;
            }
            return null;
        }

        protected override void Init()
        {
            base.Init();
            bulletType = GetInputValues<BaseBullet>("bulletType", null);
        }

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            bulletType = GetInputValues<BaseBullet>(to.fieldName, null);
        }

        public override void OnRemoveConnection(NodePort port) {
            bulletType = GetInputValues<BaseBullet>(port.fieldName, null);
        }


    }
}