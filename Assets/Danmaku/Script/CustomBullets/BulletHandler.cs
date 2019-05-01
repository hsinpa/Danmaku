using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DanmakuEditor;

public class BulletHandler : MonoBehaviour
{
    [SerializeField]
    private ProjectileHandler _projectileHandler;

    [SerializeField]
    private DanmakuGraph _bulletGraph;

    [SerializeField]
    private Transform _target;

    private DanmakuEditor.BulletPattern _currentPattern;
    private BulletWave _bulletWave;


    protected float recordTime;
    private Dictionary<string, float> RecordTimeTable = new Dictionary<string, float>();

    private void Start()
    {
        SetUp(_projectileHandler);
    }

    public void SetUp(ProjectileHandler p_projectileHandler) {
        _projectileHandler = p_projectileHandler;

        if (_bulletGraph != null) {
            _bulletWave = _bulletGraph.bulletWave;

            _currentPattern = _bulletWave.GetDefaultPattern();
        }
    }

    public void Fire() {
        if (_currentPattern == null) return;

        for (int i = 0; i < _currentPattern.bulletType.Length; i++)
        {
            DanmakuEditor.BaseBullet baseBullet = _currentPattern.bulletType[i];
            DanmakuEditor.BulletPath initBulletPath = baseBullet.path[0];

            string fireCDKey = baseBullet._id + "_fireCD";
            string bulletNumKey = baseBullet._id + "_bulletNum";

            AddRecordKey(baseBullet._id, Time.time + initBulletPath.start_delay);
            AddRecordKey(fireCDKey, 0);

            if (Time.time > RecordTimeTable[baseBullet._id])
            {
                float angle = MathParserRouter.Instance.CalculateAnswer(initBulletPath.angle_formula);
                Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
                if (initBulletPath.angleOnTarget && _target != null)
                    direction = (_target.position - transform.position).normalized;

                //Calculate angle range, if we want to fire multiple bullet at once
                float startAngle = Utility.MathUtility.VectorToAngle(direction) - (initBulletPath.range / 2);
                float incrementalAngle = initBulletPath.range / initBulletPath.numberOfBullet;

                AddRecordKey(bulletNumKey, GetDictValue(bulletNumKey) + 1, true);
                float fireQueue = GetDictValue(bulletNumKey) % baseBullet.fireNumCd;

                for (int b = 0; b < initBulletPath.numberOfBullet; b++)
                {
                    var projectile = SetProjectile(baseBullet, (startAngle + (incrementalAngle * b)));

                    float fireQueueIndex = (fireQueue == 0) ? (baseBullet.fireNumCd - 1) : (fireQueue - 1);
                    float duration_percentage = (baseBullet.fireNumCd - fireQueueIndex) / baseBullet.fireNumCd;

                    projectile.duration = initBulletPath.duration * duration_percentage;
                }

                RecordTimeTable[baseBullet._id] = Time.time + initBulletPath.frequency;

                //Debug.Log(GetDictValue(bulletNumKey) + ", " + fireQueue + " , " + baseBullet.fireNumCd);
                if (fireQueue == 0)
                {
                    RecordTimeTable[baseBullet._id] = RecordTimeTable[baseBullet._id] + baseBullet.loadUpCd;
                }

            }



        }
    }

    public void Update()
    {
        Fire();
    }

    private BaseProjectile SetProjectile(DanmakuEditor.BaseBullet baseBullet, float angle) {
        var pObject = _projectileHandler.CreateProjectile(baseBullet.poolObjectID);
        pObject.transform.rotation = Quaternion.Euler(0, 0, angle);
        pObject.transform.position = transform.position + baseBullet.path[0].spawnOffset;

        SpriteRenderer renderer = pObject.GetComponent<SpriteRenderer>();
        //renderer.sprite = baseBullet.sprite;
        var projectile = pObject.GetComponent<BaseProjectile>();
        projectile.baseBullet = baseBullet;
        projectile.spawnTime = Time.time;

        Vector3 v = renderer.bounds.size;
        pObject.GetComponent<BoxCollider2D>().size = v;

        return projectile;
    }

    private void AddRecordKey(string key, float value, bool overwrite = false) {
        bool hasKey = RecordTimeTable.ContainsKey(key);

        if (hasKey && overwrite)
            RecordTimeTable[key] = value;

        //If this key doesn't exist
        if (!hasKey)
            RecordTimeTable.Add(key, value);
    }

    private float GetDictValue(string key, float defaultValue = 0) {
        bool hasKey = RecordTimeTable.ContainsKey(key);

        return (hasKey) ? RecordTimeTable[key] : defaultValue;
    }

    public void ChangePattern(string next_pattern_id) {

    }

    public void Reset()
    {
        RecordTimeTable.Clear();
    }
}
