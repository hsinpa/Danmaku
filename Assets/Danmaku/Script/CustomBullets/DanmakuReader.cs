﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DanmakuEditor;

namespace Danmaku.BulletLauncher {
    public class DanmakuReader : MonoBehaviour
    {
        [SerializeField]
        private BulletStateCtrl _projectileHandler;

        [SerializeField]
        private BeamStateCtrl _beamStateController;

        [SerializeField]
        private DanmakuGraph _bulletGraph;


        private DanmakuEditor.BulletPattern _currentPattern;
        private BulletWave _bulletWave;

        private BulletLauncher normalTypeLauncher;
        private BeamLauncher beamLauncher;
        private BaseCharacter baseCharacter;

        public System.Action<BeamObject> OnBulletCreate;

        protected float recordTime;
        private Dictionary<string, float> RecordTimeTable = new Dictionary<string, float>();

        private void Start()
        {
            baseCharacter = GetComponent<BaseCharacter>();

            if (_projectileHandler != null)
                SetUp(_projectileHandler);
        }

        public void SetUp(BulletStateCtrl p_projectileHandler)
        {
            _projectileHandler = p_projectileHandler;

            if (_bulletGraph != null)
            {
                _bulletWave = _bulletGraph.bulletWave;

                _currentPattern = _bulletWave.GetDefaultPattern();
            }

            SetBulletLauncher();
        }

        private void SetBulletLauncher() {
            if (normalTypeLauncher == null) {
                normalTypeLauncher = new BulletLauncher();
                normalTypeLauncher.OnBulletCreate += OnBulletAdded;
            }

            if (beamLauncher == null) {
                beamLauncher = new BeamLauncher();
                beamLauncher.OnBeamCreate += OnBeamAdded;
            }

            normalTypeLauncher.Reset();
            beamLauncher.Reset();

            normalTypeLauncher.SetUp(baseCharacter);
            beamLauncher.SetUp(baseCharacter);
        }

        private void OnBulletAdded(BulletObject projectile) {
            projectile.fromCharacter = baseCharacter;

            _projectileHandler.AddProjectile(projectile);
        }

        private void OnBeamAdded(BeamObject projectile)
        {
            _beamStateController.AddProjectile(projectile);
        }


        public void Fire(Vector3 direction)
        {
            if (_currentPattern == null) return;
            //float time = Time.time;

            for (int i = 0; i < _currentPattern.bulletType.Length; i++)
            {
                //DanmakuEditor.BaseBullet baseBullet = _currentPattern.bulletType[i];

                if (_currentPattern.bulletType[i].GetType() == typeof(NormalBullet))
                {
                    normalTypeLauncher.Fire(_currentPattern.bulletType[i], direction);
                }
                else if (_currentPattern.bulletType[i].GetType() == typeof(BeamBullet)) {
                    beamLauncher.Fire(_currentPattern.bulletType[i], direction);
                }

            }
        }

        public void Reset()
        {
            RecordTimeTable.Clear();
        }

        private void OnDestroy() { 
        
            if (normalTypeLauncher != null)
                normalTypeLauncher.OnBulletCreate -= OnBulletAdded;

            if (beamLauncher != null)
                beamLauncher.OnBeamCreate -= OnBeamAdded;
        }
    }
}
