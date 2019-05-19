using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordInteractor : MonoBehaviour, BaseInteractor
{

    float attackPeriod = 0.7f;
    float lastAttackTime = 0;

    System.Action<Vector3> interactEvent;
    BaseCharacter _unit;

    public void SetUp(BaseCharacter unit, System.Action<Vector3> interactEvent) {
        this.interactEvent = interactEvent;
        _unit = unit;
    }

    public void UpdateAttackTime() {
        lastAttackTime = Time.time;
    }

    public void React(BaseProjectileObject projectile_object, System.Action projectileDestroyEvent)
    {

        if (projectile_object.fromCharacter == _unit) return;

        if (lastAttackTime + attackPeriod > Time.time) {
           
            if (this.interactEvent != null)
            {
                Vector3 dir = (projectile_object.fromCharacter.transform.position - _unit.transform.position).normalized;
                this.interactEvent(dir);
            }

            if (projectileDestroyEvent != null)
                projectileDestroyEvent();
        }
    }
}