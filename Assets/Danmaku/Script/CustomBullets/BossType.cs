using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BossType : MonoBehaviour {

    [HideInInspector]
    public Transform projectileHolder;

    protected float recordTime;
    protected float patternStartTime = 0;

    protected List<BaseProjectile> projectiles = new List<BaseProjectile>();
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

    private void Start()
    {
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

                if (bulletType.followTarget) {
                    angular_velocity = Utility.MathUtility.VectorToAngle((target.position - projectiles[i].transform.position).normalized);

                    angular_velocity = Mathf.LerpAngle((eulerAngles.z), NormalizeAngle(angular_velocity), bulletType.lerpPercent * Time.deltaTime);


                    projectiles[i].transform.rotation = Quaternion.Euler(0, 0, (angular_velocity));
                }

                //projectiles[i].angle = Time.deltaTime * angular_velocity;
                //projectiles[i].transform.rotation = Quaternion.Euler(0, 0, (eulerAngles.z + projectiles[i].angle));
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


    protected BaseProjectile CreateProjectile(float p_angle)
    {
        GameObject projectile = Instantiate(bulletType.prefab, (projectileHolder != null) ? projectileHolder : this.transform);
        BaseProjectile baseProjectile = projectile.GetComponent<BaseProjectile>();

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
