using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunType : BaseBullet {

    public float test_angle;
    public float range;
    public float radius;

    public float angular_velocity;
    public float numberOfBullet;

    public override void Fire(Vector3 direction)
    {
        if (Time.time > recordTime)
        {
            float startAngle = Utility.MathUtility.VectorToAngle(direction) - (range / 2);
            float incrementalAngle = range / numberOfBullet;

            for (int b = 0; b < numberOfBullet; b++)
            {
                var projectile = CreateProjectile(startAngle + (incrementalAngle * b));
                projectiles.Add(projectile);
            }

            recordTime = Time.time + frequency;
        }
    }

    void Update()
    {
        if (testMode) {
            float x = Mathf.Cos(test_angle), y = Mathf.Sin(test_angle);
            Fire(new Vector2(x, y));
        }

        for (int i = 0; i < projectiles.Count; i++)
        {
            if (projectiles.Count > i)
            {
                var eulerAngles = projectiles[i].transform.rotation.eulerAngles;

                //projectiles[i].angle = Mathf.Sin(Time.time) * Time.deltaTime * angular_velocity;

                projectiles[i].angularVelocity = Time.deltaTime * angular_velocity;
                projectiles[i].transform.rotation = Quaternion.Euler(0, 0, (eulerAngles.z + projectiles[i].angularVelocity));


                projectiles[i].transform.position += projectiles[i].transform.right * velocity * Time.deltaTime;

                if (projectiles[i].duration + projectiles[i].spawnTime < Time.time) {
                    DestroyBullet(i);
                }

            }
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < projectiles.Count; i++) {
            if (projectiles[i] != null)
                Destroy(projectiles[i].gameObject);
        }
    }


}
