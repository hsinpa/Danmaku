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

    

    public virtual void Fire(Vector3 direction) {

    }

    protected BaseProjectile CreateProjectile(float p_angle)
    {
        GameObject projectile = Instantiate(prefab, (projectileHolder != null)  ? projectileHolder : this.transform);
        BaseProjectile baseProjectile = projectile.GetComponent<BaseProjectile>();
        projectile.transform.rotation = Quaternion.Euler(0, 0, p_angle);
        projectile.transform.position = transform.position;

        SpriteRenderer spriteRenderer = projectile.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = this.bullet_sprite;

        baseProjectile.spawnTime = Time.time;
        return baseProjectile;
    }
}
