using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Danmaku.BulletLauncher;

public class DumbUnit : BaseCharacter
{
    private DanmakuReader _bulletHandler;

    [SerializeField]
    private Transform _target;

    private void Start()
    {
        _bulletHandler = GetComponent<DanmakuReader>();
    }


    private void Update()
    {

        Vector3 direction = Vector3.zero;
        if (_target != null)
            direction = (_target.position - this.transform.position).normalized;

        if (_bulletHandler != null)
            _bulletHandler.Fire(direction);
    }

}
