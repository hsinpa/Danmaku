using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

    Vector3 target, mousePos, refVel;

    float cameraDist = 1f;
    float smoothTime = 0.3f, zStart;

    Camera _camera;

    private void Start()
    {

        _camera = GetComponent<Camera>();
        target = player.position;
        zStart = transform.position.z;
    }

    private void Update()
    {
        mousePos = CaptureMousePos();
        target = UpdateTargetPos();
        UpdateCameraPositon();
    }

    Vector3 CaptureMousePos() {
        Vector2 ret = _camera.ScreenToViewportPoint(Input.mousePosition);
        ret *= 2;
        ret -= Vector2.one;

        float max = 0.9f;
        if (Mathf.Abs(ret.x) > max || Mathf.Abs(ret.y) > max) {
            ret = ret.normalized;
        }

        return ret;
    }

    void UpdateCameraPositon() {
        Vector3 tempPos;
        tempPos = Vector3.SmoothDamp(transform.position, target, ref refVel, smoothTime);
        transform.position = tempPos;
    }

    Vector3 UpdateTargetPos() {
        Vector3 mouseOffset = mousePos * cameraDist;
        Vector3 ret = player.position + mouseOffset;

        ret.z = zStart;
        return ret;
    }

}
