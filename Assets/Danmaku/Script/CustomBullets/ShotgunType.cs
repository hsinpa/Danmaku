using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunType : BaseBullet {

    public float angle;
    public float range;
    public float radius;

    public float angular_velocity;
    public float numberOfBullet;

    void Update()
    {
        if (Time.time > recordTime)
        {
            float startAngle = angle - (range / 2);
            float incrementalAngle = range / numberOfBullet;

            for (int b = 0; b < numberOfBullet; b++) {
                var projectile = CreateProjectile(startAngle + (incrementalAngle * b));
                projectiles.Add(projectile);
            }

            recordTime += frequency;
        }

        for (int i = 0; i < projectiles.Count; i++)
        {
            if (projectiles.Count > i)
            {
                var eulerAngles = projectiles[i].transform.rotation.eulerAngles;

                //projectiles[i].angle = Mathf.Sin(Time.time) * Time.deltaTime * angular_velocity;

                projectiles[i].angle = Time.deltaTime * angular_velocity;
                projectiles[i].transform.rotation = Quaternion.Euler(0, 0, (eulerAngles.z + projectiles[i].angle));


                projectiles[i].transform.position += projectiles[i].transform.right * velocity * Time.deltaTime;

            }
        }
    }


}
