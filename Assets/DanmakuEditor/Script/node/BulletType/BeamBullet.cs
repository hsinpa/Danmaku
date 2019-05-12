using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DanmakuEditor
{
    public class BeamBullet : BaseBullet
    {
        [Output(connectionType = ConnectionType.Multiple)] public BeamBullet node;

        public float beamLength;
        public float beamWidth;

        public float damage;
        public float damageFrequency;
        public LayerMask collideMask;

        [Header("Spawn Properties")]
        public float angle = 0;
        public bool angleOnTarget;

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
            poolObjectID = "bullet_type_02";

        }

    }
}