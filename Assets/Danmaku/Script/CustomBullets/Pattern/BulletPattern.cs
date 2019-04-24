using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPattern : MonoBehaviour
{

    public List<BulletType> bulletType;

    [System.Serializable]
    public struct BulletType {
        public string _id;
        public GameObject prefab;
        public float angle;
        public float velocity;
        public float frequency;
        public float duration;

        public float range;
        public float radius;

        public float angular_velocity;
        public float numberOfBullet;

        public string onDurationEvent;
        public Vector3 bulletSpawnOffset;
    }

}
