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

            AddRecordKey(baseBullet._id, Time.time + initBulletPath.start_delay);
            AddRecordKey(fireCDKey, Time.time + baseBullet.fireCd);

            if (Time.time > RecordTimeTable[fireCDKey]) {

                if (RecordTimeTable[fireCDKey] + baseBullet.loadUpCd < Time.time)
                    RecordTimeTable[fireCDKey] = Time.time + baseBullet.fireCd;
                continue;
            }

            if (Time.time > RecordTimeTable[baseBullet._id])
            {
                Vector3 direction = new Vector3(Mathf.Cos(initBulletPath.start_angle * Mathf.Deg2Rad), Mathf.Sin(initBulletPath.start_angle * Mathf.Deg2Rad), 0);
                if (initBulletPath.angleOnTarget && _target != null)
                    direction = (_target.position - transform.position).normalized;

                //Calculate angle range, if we want to fire multiple bullet at once
                float startAngle = Utility.MathUtility.VectorToAngle(direction) - (initBulletPath.range / 2);
                float incrementalAngle = initBulletPath.range / initBulletPath.numberOfBullet;

                for (int b = 0; b < initBulletPath.numberOfBullet; b++)
                {
                    SetProjectile(baseBullet, (startAngle + (incrementalAngle * b)));
                }

                RecordTimeTable[baseBullet._id] = Time.time + initBulletPath.frequency;
            }

        }
       

    }

    public void Update()
    {
        Fire();
    }

    private void SetProjectile(DanmakuEditor.BaseBullet baseBullet, float angle) {
        var pObject = _projectileHandler.CreateProjectile(baseBullet.poolObjectID);
        pObject.transform.rotation = Quaternion.Euler(0, 0, angle);
        pObject.transform.position = transform.position + baseBullet.path[0].spawnOffset;

        SpriteRenderer renderer = pObject.GetComponent<SpriteRenderer>();
        var projectile = pObject.GetComponent<BaseProjectile>();
        projectile.baseBullet = baseBullet;
        projectile.spawnTime = Time.time;

        Vector3 v = renderer.bounds.size;
        pObject.GetComponent<BoxCollider2D>().size = v;
    }

    private void AddRecordKey(string key, float value) {
        //If this key doesn't exist
        if (!RecordTimeTable.ContainsKey(key))
            RecordTimeTable.Add(key, value);
    }

    public void ChangePattern(string next_pattern_id) {

    }

    public void Reset()
    {
        RecordTimeTable.Clear();
    }
}
