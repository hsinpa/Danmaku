using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Utility;

[CustomEditor (typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{

    private void OnSceneGUI()
    {
        FieldOfView fow = (FieldOfView) target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.forward, Vector3.up, 360, fow.viewRadius);

        Vector3 viewAngleA = MathUtility.AngleToVector3( fow.GetLocalAngle(-fow.viewAngle / 2) );
        Vector3 viewAngleB = MathUtility.AngleToVector3(fow.GetLocalAngle(fow.viewAngle / 2));

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

        int visibleCount = fow.visibleTargets.Count;
        Handles.color = Color.red;
        for (int k = 0; k < visibleCount; k++)
            Handles.DrawLine(fow.transform.position, fow.visibleTargets[k].position);
    }
}
