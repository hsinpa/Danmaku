using UnityEngine;

public interface BaseLauncher
{
    void SetUp(BaseCharacter self);
    void Fire(DanmakuEditor.BaseBullet baseBullet, Vector3 direction);
    void Reset();
}
