using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danmaku_Trial : MonoBehaviour {

    public GameObject prefab;
    public Sprite bullet_sprite;
    public float angle;

    public float velocity;
    public float angular_velocity;
    public float delay;

    private float recordTime;

    private List<BaseProjectile> projectiles;


	// Use this for initialization
	void Start () {
        projectiles = new List<BaseProjectile>();
    }

    // Update is called once per frame
    void Update () {
        if (Time.time > recordTime) {
            var projectile = CreateProjectile(angle);
            projectiles.Add(projectile);

            recordTime += delay;
        }

        for (int i = 0; i < projectiles.Count; i++) {
            if (projectiles.Count > i) {
                projectiles[i].transform.position += projectiles[i].transform.right * velocity * Time.deltaTime;

                var eulerAngles = projectiles[i].transform.rotation.eulerAngles;

                projectiles[i].angularVelocity = Mathf.Sin(Time.time) * Time.deltaTime * angular_velocity;


                projectiles[i].transform.rotation = Quaternion.Euler(0, 0, (eulerAngles.z + projectiles[i].angularVelocity)  );
            }
        }
        Debug.Log(projectiles[0].angularVelocity);
    }

    private BaseProjectile CreateProjectile(float p_angle) {
        GameObject projectile = Instantiate(prefab, this.transform);
        BaseProjectile baseProjectile = projectile.GetComponent<BaseProjectile>();
        projectile.transform.rotation = Quaternion.Euler(0, 0, p_angle);
        projectile.transform.position = transform.position;

        SpriteRenderer spriteRenderer = projectile.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = this.bullet_sprite;

        return baseProjectile;
    }
}
