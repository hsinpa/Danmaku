using System.Collections;
using System.Collections.Generic;
using DanmakuEditor;
using UnityEngine;

namespace Danmaku.BulletLauncher
{
    public class BeamLauncher : BaseLauncher
    {
        private BaseCharacter _self;
        private BeamObject beamObject;
        public System.Action<BeamObject> OnBeamCreate;


        public void Fire(DanmakuEditor.BaseBullet baseBullet, Vector3 p_direction)
        {
            DanmakuEditor.BeamBullet beamInfo = (DanmakuEditor.BeamBullet)baseBullet;

            if (beamObject == null)
            {
                beamObject = CreateBeam(beamInfo);
                OnBeamCreate(beamObject);
            }
        }

        private BeamObject CreateBeam(BeamBullet beamBullet)
        {
            GameObject projectile = Pooling.PoolManager.instance.ReuseObject(beamBullet.poolObjectID);
            BeamObject baseProjectile = projectile.GetComponent<BeamObject>();

            baseProjectile.SetUp(beamBullet, PropertiesUtility.time);
            baseProjectile.lineRenderer.startWidth = beamBullet.beamWidth;
            baseProjectile.lineRenderer.endWidth = beamBullet.beamWidth;
            baseProjectile.fromCharacter = _self;
            projectile.transform.localPosition = Vector3.zero;

            return baseProjectile;
        }

        public void Reset()
        {
            beamObject = null;
        }


        public void SetUp(BaseCharacter self)
        {
            _self = self;
        }
    }
}