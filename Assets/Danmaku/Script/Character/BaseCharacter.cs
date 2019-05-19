using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Danmaku.BulletLauncher;
public class BaseCharacter : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    protected Transform projectileHolder;

    protected BaseActions actions;
    protected Rigidbody2D rigidBody;
    protected BaseBullet baseBullet;
    protected DanmakuReader danmakuReader;

    public System.Action<BaseCharacter> OnDestroy;

    [SerializeField]
    private bool isKillable = true;

    public enum Team {
        Team1, // Player
        Team2 // Enemy
    }

    public Team team;

    // Start is called before the first frame update
    protected void Init()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        actions = new BaseActions(transform, rigidBody, moveSpeed);

        danmakuReader = GetComponent<DanmakuReader>();

        baseBullet = GetComponent<BaseBullet>();

        if (baseBullet != null) {
            baseBullet.SetUp(this);
            baseBullet.projectileHolder = projectileHolder;
        }
    }

    public virtual void OnHit(BulletObject p_baseProjectile) {
        if (team == Team.Team2 && OnDestroy != null && isKillable)
            OnDestroy(this);
    }

}
