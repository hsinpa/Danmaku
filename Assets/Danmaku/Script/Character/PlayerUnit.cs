using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : BaseCharacter
{

    public void SetUp(Transform p_projectileHolder)
    {
        projectileHolder = p_projectileHolder;

        base.Init();
    }


    private void Update()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 worldPoint = new Vector3((point.x), (point.y), 0);

        Vector3 direction = (worldPoint - transform.position).normalized;
        //float angle =  Utility.MathUtility.NormalizeAngle(Utility.MathUtility.VectorToAngle(direction)) - 90;

        //transform.rotation = Quaternion.Euler(0, 0, angle);

        if (Input.GetMouseButton(0))
        {
            if (baseBullet != null)
                baseBullet.Fire(direction);
        }
    }

    private void FixedUpdate()
    {
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");

        actions.Move(new Vector2(horizontalAxis, verticalAxis));
    }
}
