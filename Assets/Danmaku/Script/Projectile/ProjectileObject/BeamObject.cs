using System.Collections;
using System.Collections.Generic;
using DanmakuEditor;
using UnityEngine;

public class BeamObject : BaseProjectileObject
{
    public DanmakuEditor.BeamBullet baseBullet;
    public LineRenderer lineRenderer;

    public override void SetUp(DanmakuEditor.BaseBullet p_baseBullet, float p_spawnTime)
    {
        base.SetUp(p_baseBullet, p_spawnTime);
        baseBullet = (DanmakuEditor.BeamBullet)p_baseBullet;
        lineRenderer = GetComponent<LineRenderer>();
    }

    public override void Reset()
    {
        base.Reset();
        baseBullet = null;
    }

}
