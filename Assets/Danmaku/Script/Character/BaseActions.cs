using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseActions
{
    public float speed = 0;

    private Transform _transform;
    private Rigidbody2D rigidBody;

    public BaseActions(Transform transform, Rigidbody2D rigidBody, float speed) {
        this._transform = transform;
        this.rigidBody = rigidBody;
        this.speed = speed;
    }

    public void Fire() {

    }

    public void Move(Vector3 direction)
    {
        if (_transform == null) return;

        this.rigidBody.MovePosition(_transform.position + Time.deltaTime * speed * direction);

        //_transform.Translate(Time.deltaTime * speed * direction);
    }

}
