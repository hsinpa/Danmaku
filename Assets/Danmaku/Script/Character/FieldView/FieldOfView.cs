using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
public class FieldOfView : MonoBehaviour
{
    [Range(0, 360)]
    public float viewRadius;

    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public List<Transform> visibleTargets = new List<Transform>();
    public float meshResolution;
    public int edgeResolveIteration;
    public float edgeDstThreshold;

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    private void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        StartCoroutine(FindTargetWithDelay(0.2f));

    }

    private void LateUpdate()
    {
        DrawFieldOfView();
    }

    IEnumerator FindTargetWithDelay(float delay) {
        while (true) {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    private void FindVisibleTargets() {
        visibleTargets.Clear();
        Collider2D[] targetInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetInViewRadius.Length; i++) {
            Transform target = targetInViewRadius[i].transform;

            if (target == this.transform) continue;

            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector2.Angle(transform.up, dirToTarget) < (viewAngle / 2)) {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask)) {
                    visibleTargets.Add(target);
                }
            }
        }
    }

    private void DrawFieldOfView() {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();

        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount; i++) {
            float angle = MathUtility.NormalizeAngle ( -(transform.eulerAngles.z - viewAngle / 2 + stepAngleSize * i) );
            var newCast = ViewCast(angle);

            if (i > 0) {
                bool edgeDstThresholdExceed = Mathf.Abs(oldViewCast.dst - newCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newCast.hit || (oldViewCast.hit && newCast.hit && edgeDstThresholdExceed)) {
                    EdgeInfo edge = FindEdge(oldViewCast, newCast);

                    if (edge.pointA != Vector3.zero) {
                        viewPoints.Add(edge.pointA);
                    }

                    if (edge.pointB != Vector3.zero) {
                        viewPoints.Add(edge.pointB);
                    }

                }
            }


            //Debug.DrawLine(transform.position, transform.position + MathUtility.AngleToVector3(angle) * viewRadius, Color.red);
            viewPoints.Add(newCast.point);
            oldViewCast = newCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++) {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
            //vertices[i + 1] = (viewPoints[i]);

            if (i < vertexCount - 2) {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;

        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIteration; i++) {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewcast = ViewCast(angle);

            bool edgeDstThresholdExceed = Mathf.Abs(minViewCast.dst - maxViewCast.dst) > edgeDstThreshold;

            if (newViewcast.hit == minViewCast.hit && !edgeDstThresholdExceed)
            {
                minAngle = angle;
                minPoint = newViewcast.point;
            }
            else {
                maxAngle = angle;
                maxPoint = newViewcast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    private ViewCastInfo ViewCast(float angle) {
        Vector3 dir =  MathUtility.AngleToVector3((angle));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, viewRadius, obstacleMask);

        if (hit.collider != null)
        {
            return new ViewCastInfo(true, hit.point, hit.distance, angle);
        }
        else {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, angle);
        }
    }


    public float GetLocalAngle(float p_viewAngle) {
        return p_viewAngle - transform.eulerAngles.z;
    }

    public struct ViewCastInfo {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool hit, Vector3 point, float dst, float angle)
        {
            this.hit = hit;
            this.point = point;
            this.dst = dst;
            this.angle = angle;
        }
    }

    public struct EdgeInfo {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 pointA, Vector3 pointB)
        {
            this.pointA = pointA;
            this.pointB = pointB;
        }
    }

}
