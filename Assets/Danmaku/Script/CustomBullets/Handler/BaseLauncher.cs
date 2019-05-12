using UnityEngine;

public interface BaseLauncher
{
    void SetUp(BaseCharacter self);
    void Fire(DanmakuEditor.BaseBullet baseBullet, Transform target);
    void Reset();
}
