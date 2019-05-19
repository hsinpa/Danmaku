using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using System.Linq;

namespace DanmakuEditor {
    public abstract class BaseBullet : XNode.Node
    {
        public string _id;
        public string poolObjectID = "bullet_type_02";

        public Sprite sprite;
        public Vector2 scale = Vector2.one;

        public int fireNumCd;
        public float loadUpCd;

        protected T[] SortByHeight<T>(T[] nodeArray) where T : XNode.Node {
            if (nodeArray == null) return default(T[]);
            List<T> toList = nodeArray.ToList();
            return toList.OrderBy(x => x.position.y).ToArray();
        }

    }
}