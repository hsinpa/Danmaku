using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using System.Linq;

namespace DanmakuEditor {
    public abstract class BaseBullet : XNode.Node
    {
        [Input(connectionType = ConnectionType.Multiple)] public BulletPath[] path;

        public string _id;
        public string poolObjectID = "bullet_type_02";

        public Sprite sprite;
        public Vector2 scale = Vector2.one;

        public float fireCd;
        public float loadUpCd;

        //public bool angleOnTarget;

        //public Vector3 spawnOffset;

        //public float velocity;

        //public float range;
        //public float radius;

        //public float angular_velocity;
        //public bool followTarget;
        //public float lerpPercent;

        //public float numberOfBullet;

        //public float start_delay;
        //public float frequency;
        //public float duration;


        protected T[] SortByHeight<T>(T[] nodeArray) where T : XNode.Node {
            List<T> toList = nodeArray.ToList();
            return toList.OrderBy(x => x.position.y).ToArray();
        }

    }
}