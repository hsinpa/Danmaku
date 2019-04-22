using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunType : BaseBullet {

    //public float angle;
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

            recordTime += frequency;
        }
    }

    void Update()
    {
        for (int i = 0; i < projectiles.Count; i++)
        {
            if (projectiles.Count > i)
            {
                var eulerAngles = projectiles[i].transform.rotation.eulerAngles;

                //projectiles[i].angle = Mathf.Sin(Time.time) * Time.deltaTime * angular_velocity;

                projectiles[i].angle = Time.deltaTime * angular_velocity;
                projectiles[i].transform.rotation = Quaternion.Euler(0, 0, (eulerAngles.z + projectiles[i].angle));


                projectiles[i].transform.position += projectiles[i].transform.right * velocity * Time.deltaTime;

                if (projectiles[i].duration + projectiles[i].spawnTime < Time.time) {
                    DestroyBullet(i);
                }

            }
        }
    }


}
