using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pooling;

public class BulletStateCtrl : MonoBehaviour
{
    [SerializeField]
    List<BulletObject> projectileHolder = new List<BulletObject>();
    List<int> projectileDestroyIndexes = new List<int>();

    RaycastHit2D[] raycastCache = new RaycastHit2D[32];

    [SerializeField]
    MathParserRouter mathRouter;

    public void SetUp() {

    }

    public void Enqueue(BulletObject p_projectile) {
        p_projectile.transform.SetParent(this.transform);
    }

    public void AddProjectile(BulletObject projectile) {
        projectileHolder.Add(projectile);
    }

    public void Update()
    {
        mathRouter.Refresh();
        HandleDestroyIndexes();
        int pLength = projectileHolder.Count;
        float time = Time.time;
        float deltaTime = Time.deltaTime;

        for (int i = 0; i < pLength; i++) {
            if (projectileHolder[i] != null && projectileHolder[i].baseBullet != null) {
                var bulletInfo = projectileHolder[i].baseBullet;
                var bulletPath = projectileHolder[i].currentBulletPath;

                var oldPosition = projectileHolder[i].transform.position;


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
                projectileHolder[i].transform.rotation = Quaternion.Euler(0, 0, (eulerAngles.z + (deltaTime * angular_velocity) ));
                //}

                var newPosition = oldPosition + projectileHolder[i].transform.right * bulletPath.velocity * Time.deltaTime;
                var distance = newPosition - oldPosition;
                projectileHolder[i].transform.position = newPosition;

                CheckCollision(oldPosition, projectileHolder[i].boundSize, distance, distance.magnitude, projectileHolder[i].collideLayer);

                if (projectileHolder[i].duration + projectileHolder[i].spawnTime < time)
                {
                    if (projectileHolder[i].IsLastPath)
                    {
                        DestroyBullet(i);
                    }
                    else {
                        projectileHolder[i].SetNextPath(time);
                    }
                }

            }
        }
    }

    private void CheckCollision(Vector2 p_oldPosition, Vector2 p_bounds, Vector2 p_direction, float p_distance, LayerMask p_laymask)
    {
        float angle = Utility.MathUtility.VectorToAngle(p_direction);
        int hits = Physics2D.BoxCastNonAlloc(p_oldPosition, p_bounds, angle, p_direction, raycastCache, p_distance, p_laymask);

        if (hits <= 0) return;
        for (var j = 0; j < hits; j++)
        {
            var collider = raycastCache[j].collider;
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
            BulletObject baseProjectile = projectileHolder[bulletIndex];
            baseProjectile.Reset();
            //projectileHolder.RemoveAt(bulletIndex);
            projectileDestroyIndexes.Add(bulletIndex);
            PoolManager.instance.Destroy(baseProjectile.gameObject);
        //}
    }
}
