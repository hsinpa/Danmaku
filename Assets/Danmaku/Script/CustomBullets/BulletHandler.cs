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

            //If this key doesn't exist
            if (!RecordTimeTable.ContainsKey(baseBullet._id))
                RecordTimeTable.Add(baseBullet._id, Time.time + baseBullet.start_delay);

            if (Time.time > RecordTimeTable[baseBullet._id]) {
                Vector3 direction = new Vector3(Mathf.Cos(baseBullet.start_angle * Mathf.Deg2Rad), Mathf.Sin(baseBullet.start_angle * Mathf.Deg2Rad), 0);
                if (baseBullet.angleOnTarget && _target != null)
                    direction = (_target.position - transform.position).normalized;

                //Calculate angle range, if we want to fire multiple bullet at once
                float startAngle = Utility.MathUtility.VectorToAngle(direction) - (baseBullet.range / 2);
                float incrementalAngle = baseBullet.range / baseBullet.numberOfBullet;

                for (int b = 0; b < baseBullet.numberOfBullet; b++)
                {
                    SetProjectile(baseBullet, (startAngle + (incrementalAngle * b)));
                }

                RecordTimeTable[baseBullet._id] = Time.time + baseBullet.frequency;
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
        pObject.transform.position = transform.position + baseBullet.spawnOffset;

        SpriteRenderer renderer = pObject.GetComponent<SpriteRenderer>();
        var projectile = pObject.GetComponent<BaseProjectile>();
        projectile.baseBullet = baseBullet;
        projectile.spawnTime = Time.time;

        Vector3 v = renderer.bounds.size;
        pObject.GetComponent<BoxCollider2D>().size = v;
    }

    public void ChangePattern(string next_pattern_id) {

    }

    public void Reset()
    {
        RecordTimeTable.Clear();
    }
}
