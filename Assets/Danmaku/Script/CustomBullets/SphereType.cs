using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereType : BaseBullet
{
    
    public float angular_velocity;
    public float radius;

    public float numberOfBullet;

    void Update()
    {
        if (Time.time > recordTime)
        {


            for (int b = 0; b < numberOfBullet; b++)
            {
                //var projectile = CreateProjectile(startAngle + (incrementalAngle * b));
                //projectiles.Add(projectile);
            }

            recordTime += frequency;
        }

        for (int i = 0; i < projectiles.Count; i++)
        {
            if (projectiles.Count > i)
            {
                projectiles[i].transform.position += projectiles[i].transform.right * velocity * Time.deltaTime;

            }
        }
    }

}
