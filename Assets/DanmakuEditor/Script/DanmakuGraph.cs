using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DanmakuEditor {

    [CreateAssetMenu(fileName = "DanmakuEditor", menuName = "Danmaku/Editor", order = 1)]
    public class DanmakuGraph : NodeGraph
    {
        public BulletWave bulletWave
        {
            get
            {

                if (_bulletWave == null)
                    _bulletWave = (BulletWave)nodes.Find((x) => x.GetType() == typeof(BulletWave));

                return _bulletWave;
            }
        }
        private BulletWave _bulletWave;

    }
}
