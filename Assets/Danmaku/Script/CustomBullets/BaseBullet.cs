using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBullet : MonoBehaviour {
    public GameObject prefab;
    public Sprite bullet_sprite;
    public float velocity;
    public float frequency;

    [HideInInspector]
    public Transform projectileHolder;

    protected float recordTime;

    protected List<BaseProjectile> projectiles = new List<BaseProjectile>();
    protected BaseCharacter baseCharacter;

    public void SetUp(BaseCharacter baseCharacter) {
        this.baseCharacter = baseCharacter;
    }
    
    public virtual void Fire(Vector3 direction) {

    }

    protected void DestroyBullet(int bulletIndex) {
        Destroy(projectiles[bulletIndex].gameObject);
        projectiles.RemoveAt(bulletIndex);
    }

    protected virtual void OnBulletDestroy(BaseProjectile p_projectile)
    {
        int findIndex = projectiles.IndexOf(p_projectile);

        if (findIndex >= 0 && findIndex < projectiles.Count)
            DestroyBullet(findIndex);
    }

    protected BaseProjectile CreateProjectile(float p_angle)
    {
        GameObject projectile = Instantiate(prefab, (projectileHolder != null)  ? projectileHolder : this.transform);
        BaseProjectile baseProjectile = projectile.GetComponent<BaseProjectile>();
        projectile.transform.rotation = Quaternion.Euler(0, 0, p_angle);
        projectile.transform.position = transform.position;

        SpriteRenderer spriteRenderer = projectile.GetComponent<SpriteRenderer>();
        ProjectBehavior projectBehavior = projectile.GetComponent<ProjectBehavior>();

        spriteRenderer.sprite = this.bullet_sprite;
        baseProjectile.fromCharacter = this.baseCharacter;
        baseProjectile.spawnTime = Time.time;

        projectBehavior.OnProjectileDestroy += OnBulletDestroy;

        return baseProjectile;
    }
}
