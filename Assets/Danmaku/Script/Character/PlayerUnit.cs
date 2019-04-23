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
        if (Input.GetMouseButton(0))
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 worldPoint = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), 0);

            Vector3 direction = (worldPoint - transform.position).normalized;

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
