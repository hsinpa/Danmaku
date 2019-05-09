using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamLaserLuancher : MonoBehaviour
{

    LineRenderer lineRenderer;
    public float range;
    public float beamWidth;

    Vector3[] beamPaths;
    public Vector3 targetPosition;
    public LayerMask layerMask;

    RaycastHit2D[] raycastCache = new RaycastHit2D[256];



    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;

        beamPaths = new Vector3[2];
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
            Fire(lineRenderer.positionCount <= 0);

        if (lineRenderer.positionCount > 0)
        {
            CheckCollision(transform.position, new Vector2(beamWidth, beamWidth), transform.up, range, layerMask);
        }
    }

    private void CheckCollision(Vector2 p_oldPosition, Vector2 p_bounds, Vector2 p_direction, float p_distance, LayerMask p_laymask)
    {
        float angle = Utility.MathUtility.VectorToAngle(p_direction);
        int hits = Physics2D.BoxCastNonAlloc(p_oldPosition, p_bounds, angle, p_direction, raycastCache, p_distance, p_laymask);

        if (hits <= 0) return;
        for (var j = 0; j < hits; j++)
        {
            var collider = raycastCache[j].collider;
            Debug.Log(collider);
        }
    }

    void Fire(bool enable) {
        lineRenderer.positionCount = 0;
        if (enable) {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPositions(GetBeamPaths());
        }

        Debug.Log("Start Width " + lineRenderer.startWidth + ", End Width " + lineRenderer.endWidth);
    }

    Vector3[] GetBeamPaths()
    {
        Vector3 destination = (transform.right) * range;
        beamPaths[0].Set(0, 0, 0);
        beamPaths[1].Set(0, range, 0);
        return beamPaths;
    }
}