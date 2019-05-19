using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DanmakuEditor
{
    [NodeWidth(250)]
    public class BulletPath : XNode.Node
    {

        [Output(connectionType = ConnectionType.Multiple)] public BulletPath node;

        public Type type = Type.Continuous;

        [Header("Spawn Properties")]
        public string angle_formula;

        public bool angleOnTarget;
        public float start_delay;
        public float transition;

        public float range;
        public float radius;
        public float numberOfBullet;

        public Vector3 spawnOffset;

        [Header("Continuous Properties")]
        public float velocity;

        public string angular_velocity_formula;

        public bool followTarget;
        public float lerpPercent;

        public float frequency;

        [Header("Fallout Properties")]
        public float duration;

        [TextArea(3, 10)]
        public string constraint;

        public enum Type {
            Generative,
            Continuous
        }


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
        }

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            Refresh(to);
        }

        public override void OnRemoveConnection(NodePort port)
        {
            Refresh(port);
        }

        private void Refresh(NodePort port) {

        }


    }
}