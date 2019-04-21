using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : BaseCharacter
{
    private void Start()
    {
        base.Init();
    }

    private void FixedUpdate()
    {
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");

        actions.Move(new Vector2(horizontalAxis, verticalAxis));
    }
}
