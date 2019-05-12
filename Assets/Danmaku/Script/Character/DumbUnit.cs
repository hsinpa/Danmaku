﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Danmaku.BulletLauncher;

public class DumbUnit : BaseCharacter
{
    private DanmakuReader _bulletHandler;

    private void Start()
    {
        _bulletHandler = GetComponent<DanmakuReader>();
    }


    private void Update()
    {
        if (_bulletHandler != null)
            _bulletHandler.Fire();
    }

}
