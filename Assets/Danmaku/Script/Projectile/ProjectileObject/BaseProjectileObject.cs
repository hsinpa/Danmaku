using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseProjectileObject : MonoBehaviour {
    public float angularVelocity;

    public float spawnTime;
    public float duration;

    public LayerMask collideLayer;
    public Vector2 boundSize;

    protected DanmakuEditor.BaseBullet _baseBullet;

    [HideInInspector]
    public BaseCharacter fromCharacter;

    public virtual void SetUp(DanmakuEditor.BaseBullet p_baseBullet, float p_spawnTime) {
        _baseBullet = p_baseBullet;
        spawnTime = p_spawnTime;
    }

    public virtual void Reset()
    {
        angularVelocity = 0;
        spawnTime = 0;
        duration = 0;
        _baseBullet = null;
        fromCharacter = null;
    }
}