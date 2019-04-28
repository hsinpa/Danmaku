using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pooling;

public class ProjectileHandler : MonoBehaviour
{
    [SerializeField]
    List<BaseProjectile> projectileHolder = new List<BaseProjectile>();

    public void Enqueue(BaseProjectile p_projectile) {
        p_projectile.transform.SetParent(this.transform);
    }

    public GameObject CreateProjectile(string p_id) {
        GameObject reuseObject = Pooling.PoolManager.instance.ReuseObject(p_id);
        reuseObject.SetActive(true);

        BaseProjectile projectile = reuseObject.GetComponent<BaseProjectile>();

        projectileHolder.Add(projectile);

        return reuseObject;
    }


    public void Update()
    {
        for (int i = 0; i < projectileHolder.Count; i++) {
            if (projectileHolder[i] != null && projectileHolder[i].baseBullet != null) {
                var bulletInfo = projectileHolder[i].baseBullet;
                var eulerAngles = projectileHolder[i].transform.rotation.eulerAngles;

                //projectiles[i].angle = Mathf.Sin(Time.time) * Time.deltaTime * bulletType.angular_velocity;

                float angular_velocity = bulletInfo.angular_velocity;

                //if (bulletInfo.followTarget)
                //{
                //    angular_velocity = Utility.MathUtility.VectorToAngle((target.position - projectiles[i].transform.position).normalized);

                //    angular_velocity = Mathf.LerpAngle((eulerAngles.z), NormalizeAngle(angular_velocity), bulletInfo.lerpPercent * Time.deltaTime);


                //    projectiles[i].transform.rotation = Quaternion.Euler(0, 0, (angular_velocity));
                //}
                //else
                //{
                    projectileHolder[i].angularVelocity = Time.deltaTime * angular_velocity;
                    projectileHolder[i].transform.rotation = Quaternion.Euler(0, 0, (eulerAngles.z + projectileHolder[i].angularVelocity));
                //}

                //projectiles[i].transform.rotation = Quaternion.Euler(0, 0, (angular_velocity));


                projectileHolder[i].transform.position += projectileHolder[i].transform.right * bulletInfo.velocity * Time.deltaTime;

                if (projectileHolder[i].duration + projectileHolder[i].spawnTime < Time.time)
                {
                    DestroyBullet(i);
                }

            }
        }
    }

    public void Reset()    
    {
        projectileHolder.Clear();
    }

    private void DestroyBullet(int bulletIndex)
    {
        if (projectileHolder.Count > bulletIndex) {
            BaseProjectile baseProjectile = projectileHolder[bulletIndex];
            projectileHolder.RemoveAt(bulletIndex);
            PoolManager.instance.Destroy(baseProjectile.gameObject);
        }
    }
}
