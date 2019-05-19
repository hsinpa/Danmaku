using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : BaseCharacter
{

    [SerializeField]
    private SpriteRenderer weaponSprite;
    private SpriteRenderer unitSprite;

    private Vector2 spriteBoundSize;

    private float angleOffset = 45;

    private Animator weaponAnim;
    private SwordInteractor swordInteractor;

    public void SetUp(Transform p_projectileHolder)
    {
        projectileHolder = p_projectileHolder;

        this.unitSprite = GetComponent<SpriteRenderer>();
        this.spriteBoundSize = this.unitSprite.bounds.size;

        if (weaponSprite != null) {
            weaponAnim = weaponSprite.GetComponentInChildren<Animator>();
            swordInteractor = weaponSprite.GetComponentInChildren<SwordInteractor>();
            swordInteractor.SetUp(this, ReverseBullet);
        }

        base.Init();
    }


    private void Update()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 worldPoint = new Vector3((point.x), (point.y), 0);

        Vector3 distance = (worldPoint - transform.position);
        Vector3 direction = distance.normalized;

        float angle =  Utility.MathUtility.NormalizeAngle(Utility.MathUtility.VectorToAngle(direction)) - angleOffset;

        //transform.rotation = Quaternion.Euler(0, 0, angle);

        ControlSwordAngle(angle, angleOffset, direction, distance);

        if (Input.GetMouseButton(0))
        {
            //if (danmakuReader != null)
            //    danmakuReader.Fire(direction);

            SwingSword(direction);
        }
    }

    public void ReverseBullet(Vector3 direction)
    {
        if (danmakuReader != null)
            danmakuReader.Fire(direction);
    }

    private void ControlSwordAngle(float p_angle, float offset, Vector3 direction, Vector3 distance) {
        if (weaponSprite != null) {
            weaponSprite.sortingOrder = (p_angle + offset >= 0 && p_angle + offset < 180) ? 3 : 5;
            weaponSprite.transform.rotation = Quaternion.Euler(0,0,p_angle);

            float extendLenght = Mathf.Clamp(distance.magnitude, 0, spriteBoundSize.magnitude / 2);
            weaponSprite.transform.localPosition = direction * extendLenght;
        }
    }

    private void SwingSword(Vector3 direction) {
        if (weaponAnim != null) {
            weaponAnim.SetTrigger("Swing");

            swordInteractor.UpdateAttackTime();
        }
    }
    

    private void FixedUpdate()
    {
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");

        actions.Move(new Vector2(horizontalAxis, verticalAxis));
    }
}
