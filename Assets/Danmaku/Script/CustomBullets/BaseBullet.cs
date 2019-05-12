using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBullet : MonoBehaviour {
    public GameObject prefab;
    public float velocity;
    public float frequency;

    [HideInInspector]
    public Transform projectileHolder;

    protected float recordTime;

    protected List<BulletObject> projectiles = new List<BulletObject>();
    protected BaseCharacter baseCharacter;

    [SerializeField]
    protected bool testMode;


    public void SetUp(BaseCharacter baseCharacter) {
        this.baseCharacter = baseCharacter;
    }
    
    public virtual void Fire(Vector3 direction) {

    }

    protected void DestroyBullet(int bulletIndex) {
        Destroy(projectiles[bulletIndex].gameObject);
        projectiles.RemoveAt(bulletIndex);
    }

    protected virtual void OnBulletDestroy(BulletObject p_projectile, Collider2D p_collider2D)
    {
        int findIndex = projectiles.IndexOf(p_projectile);

        if (findIndex >= 0 && findIndex < projectiles.Count)
            DestroyBullet(findIndex);
    }

    protected BulletObject CreateProjectile(float p_angle)
    {
        GameObject projectile = Instantiate(prefab, (projectileHolder != null)  ? projectileHolder : this.transform);
        BulletObject baseProjectile = projectile.GetComponent<BulletObject>();
        projectile.transform.rotation = Quaternion.Euler(0, 0, p_angle);
        projectile.transform.position = transform.position;

        ProjectBehavior projectBehavior = projectile.GetComponent<ProjectBehavior>();

        baseProjectile.fromCharacter = this.baseCharacter;
        baseProjectile.spawnTime = Time.time;

        projectBehavior.OnProjectileDestroy += OnBulletDestroy;

        return baseProjectile;
    }
}
