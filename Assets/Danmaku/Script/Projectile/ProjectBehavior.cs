using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectBehavior : MonoBehaviour
{
    BaseProjectile baseProjectile;

    public System.Action<BaseProjectile, Collider2D> OnProjectileDestroy;

    private void Start()
    {
        baseProjectile = this.GetComponent<BaseProjectile>();
    }

    private void OnCollideEvent(Collider2D collision)
    {
        if (baseProjectile.fromCharacter == null) return;

        switch (collision.gameObject.layer) {

            case VariableFlag.LayerMask.unitLayer:

                BaseCharacter baseCharacter = collision.gameObject.GetComponent<BaseCharacter>();
                if (baseProjectile.fromCharacter.team != baseCharacter.team) {
                    baseCharacter.OnHit(baseProjectile);
                    if (OnProjectileDestroy != null)
                        OnProjectileDestroy(baseProjectile, collision);
                }

                break;

            case VariableFlag.LayerMask.barrierLayer:
                if (OnProjectileDestroy != null)
                    OnProjectileDestroy(baseProjectile, collision);
            break;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        OnCollideEvent(collision);
    }

}
