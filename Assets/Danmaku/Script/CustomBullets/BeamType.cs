using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamType : BaseBullet
{

    public float range;
    public float beamWidth;

    Vector3[] beamPaths;
    public LayerMask layerMask;

    RaycastHit2D[] raycastCache = new RaycastHit2D[64];
    RaycastHit2D[] beamLengthRaycastCache = new RaycastHit2D[32];

    private List<LineRenderer> lineObjects = new List<LineRenderer>();

    // Start is called before the first frame update
    void Start()
    {

        beamPaths = new Vector3[2];
    }

    private void Update()
    {

        //if (Input.GetKeyDown(KeyCode.Space))
        //    Fire(lineRenderer.positionCount <= 0);

        if (Time.time > recordTime && lineObjects.Count > 0) {
            if (lineObjects.Count > 0)
            {
                foreach (LineRenderer l in lineObjects)
                    l.positionCount = 0;
            }
        }

        for (int i = 0; i < lineObjects.Count; i++) {
            if (lineObjects[i].positionCount > 0)
            {
                CheckCollision(transform.position, new Vector2(beamWidth, beamWidth), lineObjects[i].GetPosition(1).normalized, range, layerMask);
            }
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
            if (collider.gameObject == this.gameObject)
                continue;

            Debug.Log(collider);
        }
    }

    public override void Fire(Vector3 direction) {
        LineRenderer line = null;

        if (lineObjects.Count > 0)
        {
            line = lineObjects[0];
        }
        else {
            line = CreateBeam();
            lineObjects.Add(line);
        }

        line.positionCount = 0;
        line.positionCount = 2;


        line.SetPositions(GetBeamPaths(direction));

        recordTime = frequency + Time.time;
        //Debug.Log("Start Width " + lineRenderer.startWidth + ", End Width " + lineRenderer.endWidth);
    }

    Vector3[] GetBeamPaths(Vector3 direction)
    {
        int hitsNum = Physics2D.RaycastNonAlloc(transform.position, direction, beamLengthRaycastCache, range);

        Vector3 default_dest = (direction) * range;
        Vector3 destination = FindFirstRaycast(hitsNum, beamLengthRaycastCache, default_dest);


        beamPaths[0].Set(0, 0, 0);
        beamPaths[1].Set(destination.x, destination.y, 0);
        return beamPaths;
    }

    private Vector3 FindFirstRaycast(int hitLength ,RaycastHit2D[] hits, Vector3 defaultDest) {

        for (int i = 0; i < hitLength; i++) {
            if (hits[i].collider.gameObject != this.gameObject)
                return hits[i].point -  new Vector2(transform.position.x, transform.position.y);
        }

        return defaultDest;
    }

    private LineRenderer CreateBeam()
    {
        GameObject projectile = Instantiate(prefab, this.transform);
        LineRenderer baseProjectile = projectile.GetComponent<LineRenderer>();
        baseProjectile.startWidth = beamWidth;
        baseProjectile.endWidth = beamWidth;

        projectile.transform.localPosition = Vector3.zero;
         
        return baseProjectile;
    }
}