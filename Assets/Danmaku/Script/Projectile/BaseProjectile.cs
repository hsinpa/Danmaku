using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour {
    public float angularVelocity;
    public float velocity;

    public float spawnTime;
    public float duration;
    public int pathIndex = 0;
    public DanmakuEditor.BaseBullet baseBullet;
    public DanmakuEditor.BulletPath currentBulletPath {
        get {
            return baseBullet.path[pathIndex];
        }
    }

    public bool IsLastPath {
        get {
            return (baseBullet.path.Length - 1 == pathIndex);
        }
    }

    public bool penetrateBarrier;

    [HideInInspector]
    public BaseCharacter fromCharacter;

    public void SetNextPath(float p_timeSpawn) {
        spawnTime = p_timeSpawn;

        pathIndex = (pathIndex + 1);

        transform.rotation = Quaternion.Euler(0, 0, currentBulletPath.start_angle);
    }

    public void Reset()
    {
        pathIndex = 0;
        angularVelocity = 0;
        velocity = 0;
        spawnTime = 0;
        duration = 0;
        baseBullet = null;
        fromCharacter = null;
    }
}