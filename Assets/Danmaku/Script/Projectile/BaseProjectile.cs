using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour {
    public float angle;
    public float velocity;

    public float spawnTime;
    public float duration;

    public bool penetrateBarrier;

    [HideInInspector]
    public BaseCharacter fromCharacter;
}
