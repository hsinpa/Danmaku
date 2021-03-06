﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BossType : MonoBehaviour {

    [HideInInspector]
    public Transform projectileHolder;

    protected float recordTime;
    protected float patternStartTime = 0;

    protected List<BulletObject> projectiles = new List<BulletObject>();
    protected BaseCharacter baseCharacter;

    [SerializeField]
    protected bool testMode;


    private int bulletPatternIndex = 0;
    BulletPattern bulletPattern;
    BulletPattern.BulletType bulletType {
        get {
            return bulletPattern.bulletType[bulletPatternIndex];
        }
    }

    public Transform target;

    private Animator customAnimator;
    private bool isDead = false;

    private void Start()
    {
        customAnimator = this.GetComponentInChildren<Animator>();
        bulletPattern = GetComponent<BulletPattern>();
        patternStartTime = Time.time;
    }

    public void Fire(Vector3 direction)
    {
        if (Time.time > recordTime)
        {

            float startAngle = Utility.MathUtility.VectorToAngle(direction) - (bulletType.range / 2);
            float incrementalAngle = bulletType.range / bulletType.numberOfBullet;

            for (int b = 0; b < bulletType.numberOfBullet; b++)
            {
                var projectile = CreateProjectile(startAngle + (incrementalAngle * b));

                projectile.transform.position += bulletType.bulletSpawnOffset;
                projectiles.Add(projectile);
            }

            recordTime = Time.time + bulletType.frequency;
        }
    }

    private void ChangeBulletPattern(string p_pattern_id)
    {
        int findIndex = bulletPattern.bulletType.FindIndex(x => x._id == p_pattern_id);

        if (findIndex >= 0) {
            patternStartTime = Time.time;
            bulletPatternIndex = findIndex;
        } else {
            for (int i = 0; i < projectiles.Count; i++)
                DestroyBullet(i);

            PerformDeadAnimation();
        }
    }

    private void PerformDeadAnimation() {
        if (!isDead) {
            isDead = true;

            if (customAnimator != null)
                customAnimator.SetTrigger("Explode");

            StartCoroutine(Utility.GeneralUtility.DoDelayWork(1, () =>
            {
                GetComponent<SpriteRenderer>().enabled = false;
                this.enabled = false;
            }));
        }
    }

    void Update()
    {
        if (bulletPatternIndex < 0)
            return;

        if (Time.time > bulletType.duration + patternStartTime && bulletPatternIndex >= 0) {
            ChangeBulletPattern(bulletType.onDurationEvent);
        }

        if (testMode && bulletPatternIndex >= 0) {

            Vector3 direction = new Vector3(Mathf.Cos(bulletType.angle * Mathf.Deg2Rad), Mathf.Sin(bulletType.angle * Mathf.Deg2Rad), 0);
            if (bulletType.angleOnTarget && target != null)
                direction = (target.position - transform.position).normalized;

            Fire(direction);
        }

        for (int i = 0; i < projectiles.Count; i++)
        {
            if (projectiles.Count > i)
            {
                var eulerAngles = projectiles[i].transform.rotation.eulerAngles;

                //projectiles[i].angle = Mathf.Sin(Time.time) * Time.deltaTime * bulletType.angular_velocity;

                float angular_velocity = bulletType.angular_velocity;

                if (bulletType.followTarget)
                {
                    angular_velocity = Utility.MathUtility.VectorToAngle((target.position - projectiles[i].transform.position).normalized);

                    angular_velocity = Mathf.LerpAngle((eulerAngles.z), NormalizeAngle(angular_velocity), bulletType.lerpPercent * Time.deltaTime);


                    projectiles[i].transform.rotation = Quaternion.Euler(0, 0, (angular_velocity));
                }
                else {
                    projectiles[i].angularVelocity = Time.deltaTime * angular_velocity;
                    projectiles[i].transform.rotation = Quaternion.Euler(0, 0, (eulerAngles.z + projectiles[i].angularVelocity));
                }

                //projectiles[i].transform.rotation = Quaternion.Euler(0, 0, (angular_velocity));


                projectiles[i].transform.position += projectiles[i].transform.right * bulletType.velocity * Time.deltaTime;

                if (projectiles[i].duration + projectiles[i].spawnTime < Time.time)
                {
                    DestroyBullet(i);
                }
            }
        }
    }

    public float NormalizeAngle(float angle)
    {
        float result = angle / 360;

        result = result - Mathf.FloorToInt(result);
        result = result * 360;
        if (result < 0) {
            result = 360 - result;
        }
        return result;
    }


    protected BulletObject CreateProjectile(float p_angle)
    {
        GameObject projectile = Instantiate(bulletType.prefab, (projectileHolder != null) ? projectileHolder : this.transform);
        BulletObject baseProjectile = projectile.GetComponent<BulletObject>();

        projectile.transform.rotation = Quaternion.Euler(0, 0, p_angle);
        projectile.transform.position = transform.position;

        ProjectBehavior projectBehavior = projectile.GetComponent<ProjectBehavior>();

        baseProjectile.spawnTime = Time.time;

        return baseProjectile;
    }

    protected void DestroyBullet(int bulletIndex)
    {
        Destroy(projectiles[bulletIndex].gameObject);
        projectiles.RemoveAt(bulletIndex);
    }

}
