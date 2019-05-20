using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class BaseActions
{
    public float speed = 0;

    private Transform _transform;
    private Rigidbody2D rigidBody;

    private Vector2 move, oldPosition;

    private float pixelPerUnit;

    public BaseActions(Transform transform, Rigidbody2D rigidBody, float speed, float pixelperUnit) {
        this._transform = transform;
        this.rigidBody = rigidBody;
        this.speed = speed;
        this.pixelPerUnit = pixelperUnit;
    }

    public void Fire() {

    }

    public void Move(Vector3 direction)
    {
        if (_transform == null) return;

        move = GeneralUtility.PixelPerfectClamp(Time.deltaTime * speed * direction, this.pixelPerUnit);
        oldPosition = GeneralUtility.PixelPerfectClamp(_transform.position, this.pixelPerUnit);

        this.rigidBody.MovePosition(move + oldPosition);

        //_transform.Translate(Time.deltaTime * speed * direction);
    }

}
