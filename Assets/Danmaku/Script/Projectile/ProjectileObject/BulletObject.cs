using System.Collections;
using System.Collections.Generic;
using DanmakuEditor;
using UnityEngine;

public class BulletObject : BaseProjectileObject
{
    private int pathIndex = 0;
    public DanmakuEditor.NormalBullet baseBullet;

    public DanmakuEditor.BulletPath currentBulletPath
    {
        get
        {
            return baseBullet.path[pathIndex];
        }
    }

    public override void SetUp(DanmakuEditor.BaseBullet p_baseBullet, float p_spawnTime)
    {
        base.SetUp(p_baseBullet, p_spawnTime);
        baseBullet = (DanmakuEditor.NormalBullet)p_baseBullet;
    }

    public bool IsLastPath
    {
        get
        {
            return (baseBullet.path.Length - 1 == pathIndex);
        }
    }

    public void SetNextPath(float p_timeSpawn)
    {
        spawnTime = p_timeSpawn;

        pathIndex = (pathIndex + 1);
        duration = currentBulletPath.duration;

        transform.rotation = Quaternion.Euler(0, 0, MathParserRouter.Instance.CalculateAnswer(currentBulletPath.angle_formula));
    }

    public override void Reset()
    {
        base.Reset();
        pathIndex = 0;
        baseBullet = null;
    }
}
