using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DanmakuEditor
{
    public class NormalBullet : BaseBullet
    {
        [Input(connectionType = ConnectionType.Multiple)] public BulletPath[] path;
        [Output(connectionType = ConnectionType.Multiple)] public NormalBullet node;

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "node")
            {
                return this;
            }

            return null;
        }

        protected override void Init()
        {
            base.Init();
            poolObjectID = "bullet_type_01";

            path = SortByHeight<BulletPath>(GetInputValues<BulletPath>("path", path));
        }

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            path = GetInputValues<DanmakuEditor.BulletPath>(to.fieldName, path);
        }

        public override void OnRemoveConnection(NodePort port)
        {
            path = GetInputValues<DanmakuEditor.BulletPath>(port.fieldName, path);
        }

    }
}