using DanmakuEditor;
using System.Collections.Generic;
using UnityEngine;


namespace Danmaku.BulletLauncher
{
    public class BulletLauncher : BaseLauncher
    {
        //private ProjectileHandler _projectileHandler;
        private float recordTime;
        private Dictionary<string, float> RecordTimeTable = new Dictionary<string, float>();
        private BaseCharacter _self;

        public System.Action<BulletObject> OnBulletCreate;

        public void SetUp(BaseCharacter self) {
            _self = self;
        }

        public void Fire(DanmakuEditor.BaseBullet p_baseBullet, Transform target)
        {
            DanmakuEditor.NormalBullet baseBullet = (DanmakuEditor.NormalBullet) p_baseBullet;
            DanmakuEditor.BulletPath initBulletPath = baseBullet.path[0];

            string fireCDKey = baseBullet._id + "_fireCD";
            string bulletNumKey = baseBullet._id + "_bulletNum";

            AddRecordKey(baseBullet._id, PropertiesUtility.time + initBulletPath.start_delay);
            AddRecordKey(fireCDKey, 0);

            if (PropertiesUtility.time > RecordTimeTable[baseBullet._id])
            {
                AddRecordKey(bulletNumKey, GetDictValue(bulletNumKey) + 1, true);
                float fireQueue = GetDictValue(bulletNumKey) % baseBullet.fireNumCd;

                float angle = MathParserRouter.Instance.CalculateAnswer(initBulletPath.angle_formula);
                Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
                if (initBulletPath.angleOnTarget && target != null)
                    direction = (target.position - _self.transform.position).normalized;

                //Calculate angle range, if we want to fire multiple bullet at once
                float startAngle = Utility.MathUtility.VectorToAngle(direction) - (initBulletPath.range / 2);
                float incrementalAngle = initBulletPath.range / initBulletPath.numberOfBullet;


                for (int b = 0; b < initBulletPath.numberOfBullet; b++)
                {
                    var projectile = SetProjectile(baseBullet, (startAngle + (incrementalAngle * b)));

                    float fireQueueIndex = (fireQueue == 0) ? (baseBullet.fireNumCd - 1) : (fireQueue - 1);
                    float duration_percentage = (baseBullet.fireNumCd - fireQueueIndex) / baseBullet.fireNumCd;

                    projectile.duration = initBulletPath.duration * duration_percentage;

                    if (OnBulletCreate != null)
                        OnBulletCreate(projectile);
                    //_projectileHandler.AddProjectile(projectile);
                }

                RecordTimeTable[baseBullet._id] = PropertiesUtility.time + initBulletPath.frequency;

                //Debug.Log(GetDictValue(bulletNumKey) + ", " + fireQueue + " , " + baseBullet.fireNumCd);
                if (fireQueue == 0)
                {
                    RecordTimeTable[baseBullet._id] = RecordTimeTable[baseBullet._id] + baseBullet.loadUpCd;
                }
            }
        }

        private BulletObject SetProjectile(DanmakuEditor.BaseBullet baseBullet, float angle)
        {

            GameObject reuseObject = Pooling.PoolManager.instance.ReuseObject(baseBullet.poolObjectID);
            BulletObject pObject = reuseObject.GetComponent<BulletObject>();

            pObject.transform.rotation = Quaternion.Euler(0, 0, angle);
            //pObject.transform.position = transform.position + baseBullet.path[0].spawnOffset;
            pObject.transform.position = _self.transform.position;

            SpriteRenderer renderer = pObject.GetComponent<SpriteRenderer>();
            renderer.sprite = baseBullet.sprite;
            var projectile = pObject.GetComponent<BulletObject>();
            projectile.SetUp((NormalBullet)baseBullet, Time.time);
            //projectile.baseBullet = baseBullet;
            //projectile.spawnTime = Time.time;

            pObject.boundSize = renderer.bounds.size;
            //Vector3 v = renderer.bounds.size;
            //pObject.GetComponent<BoxCollider2D>().size = v;

            return projectile;
        }

        private void AddRecordKey(string key, float value, bool overwrite = false)
        {
            bool hasKey = RecordTimeTable.ContainsKey(key);

            if (hasKey && overwrite)
                RecordTimeTable[key] = value;

            //If this key doesn't exist
            if (!hasKey)
                RecordTimeTable.Add(key, value);
        }

        private float GetDictValue(string key, float defaultValue = 0)
        {
            bool hasKey = RecordTimeTable.ContainsKey(key);

            return (hasKey) ? RecordTimeTable[key] : defaultValue;
        }

        public void Reset()
        {
            RecordTimeTable.Clear();
        }
    }
}