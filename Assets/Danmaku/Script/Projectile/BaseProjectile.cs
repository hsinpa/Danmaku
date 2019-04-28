using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour {
    public float angularVelocity;
    public float velocity;

    public float spawnTime;
    public float duration;
    public DanmakuEditor.BaseBullet baseBullet;

    public bool penetrateBarrier;

    [HideInInspector]
    public BaseCharacter fromCharacter;

    public void Reset()
    {
        angularVelocity = 0;
        velocity = 0;
        spawnTime = 0;
        duration = 0;
        baseBullet = null;
        fromCharacter = null;
    }
}
