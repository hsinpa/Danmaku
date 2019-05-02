using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pooling;

public class ProjectileHandler : MonoBehaviour
{
    [SerializeField]
    List<BaseProjectile> projectileHolder = new List<BaseProjectile>();
    List<int> projectileDestroyIndexes = new List<int>();

    [SerializeField]
    MathParserRouter mathRouter;

    public void SetUp() {

    }

    public void Enqueue(BaseProjectile p_projectile) {
        p_projectile.transform.SetParent(this.transform);
    }

    public GameObject CreateProjectile(string p_id) {
        GameObject reuseObject = Pooling.PoolManager.instance.ReuseObject(p_id);
        BaseProjectile projectile = reuseObject.GetComponent<BaseProjectile>();

        projectileHolder.Add(projectile);

        return reuseObject;
    }


    public void Update()
    {
        mathRouter.Refresh();
        HandleDestroyIndexes();
        int pLength = projectileHolder.Count;

        for (int i = 0; i < pLength; i++) {
            if (projectileHolder[i] != null && projectileHolder[i].baseBullet != null) {
                var bulletInfo = projectileHolder[i].baseBullet;
                var bulletPath = projectileHolder[i].currentBulletPath;

                var eulerAngles = projectileHolder[i].transform.rotation.eulerAngles;

                float angular_velocity = mathRouter.CalculateAnswer(bulletPath.angular_velocity_formula);
                //float angular_velocity = 0;

                //if (bulletInfo.followTarget)
                //{
                //    angular_velocity = Utility.MathUtility.VectorToAngle((target.position - projectiles[i].transform.position).normalized);

                //    angular_velocity = Mathf.LerpAngle((eulerAngles.z), NormalizeAngle(angular_velocity), bulletInfo.lerpPercent * Time.deltaTime);


                //    projectiles[i].transform.rotation = Quaternion.Euler(0, 0, (angular_velocity));
                //}
                //else
                //{
                projectileHolder[i].transform.rotation = Quaternion.Euler(0, 0, (eulerAngles.z + (Time.deltaTime * angular_velocity) ));
                //}

                projectileHolder[i].transform.position += projectileHolder[i].transform.right * bulletPath.velocity * Time.deltaTime;

                if (projectileHolder[i].duration + projectileHolder[i].spawnTime < Time.time)
                {
                    if (projectileHolder[i].IsLastPath)
                    {
                        DestroyBullet(i);
                    }
                    else {
                        projectileHolder[i].SetNextPath(Time.time);
                    }
                }

            }
        }
    }

    public void Reset()    
    {
        projectileHolder.Clear();
    }

    private void HandleDestroyIndexes() {
        int dLength = projectileDestroyIndexes.Count - 1;
        for (int i = dLength; i >= 0; i--) {
            projectileHolder.RemoveAt(projectileDestroyIndexes[i]);
        }

        projectileDestroyIndexes.Clear();
    }

    private void DestroyBullet(int bulletIndex)
    {
        //if (projectileHolder.Count > bulletIndex) {
            BaseProjectile baseProjectile = projectileHolder[bulletIndex];
            baseProjectile.Reset();
            //projectileHolder.RemoveAt(bulletIndex);
            projectileDestroyIndexes.Add(bulletIndex);
            PoolManager.instance.Destroy(baseProjectile.gameObject);
        //}
    }
}
