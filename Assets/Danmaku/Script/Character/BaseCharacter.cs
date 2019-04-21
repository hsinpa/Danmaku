using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    protected Transform projectileHolder;

    protected BaseActions actions;
    protected Rigidbody2D rigidBody;


    // Start is called before the first frame update
    protected void Init()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        actions = new BaseActions(transform, rigidBody, moveSpeed);
    }

}
