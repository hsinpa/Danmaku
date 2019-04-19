using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour {
    public GameObject prefab;
    public Sprite bullet_sprite;
    public float velocity;
    public float frequency;

    protected float recordTime;

    protected List<BaseProjectile> projectiles = new List<BaseProjectile>();

    protected BaseProjectile CreateProjectile(float p_angle)
    {
        GameObject projectile = Instantiate(prefab, this.transform);
        BaseProjectile baseProjectile = projectile.GetComponent<BaseProjectile>();
        projectile.transform.rotation = Quaternion.Euler(0, 0, p_angle);
        projectile.transform.position = transform.position;

        SpriteRenderer spriteRenderer = projectile.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = this.bullet_sprite;

        return baseProjectile;
    }
}
