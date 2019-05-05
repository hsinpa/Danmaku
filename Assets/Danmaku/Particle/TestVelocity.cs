using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVelocity : MonoBehaviour
{
    Rigidbody2D rigidBody2D;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(this.transform.forward);
        Vector2 velocity = this.transform.right * 15 * Time.deltaTime ;

        rigidBody2D = this.GetComponent<Rigidbody2D>();

        TestMovement(velocity, 15);
    }

    void TestMovement(Vector2 velocity, float angularVelocity) {
        rigidBody2D.velocity = ( velocity );
        //rigidBody2D.angularVelocity = angularVelocity;
    }

}
