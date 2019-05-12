using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamStateCtrl : MonoBehaviour
{
    List<BeamObject> projectileHolder = new List<BeamObject>();

    RaycastHit2D[] raycastCache = new RaycastHit2D[64];
    RaycastHit2D[] beamLengthRaycastCache = new RaycastHit2D[32];

    private Vector3[] beamPaths = new Vector3[2];

    public void AddProjectile(BeamObject beanObject)
    {
        beanObject.spawnTime = PropertiesUtility.time;
        beanObject.duration = beanObject.baseBullet.fireNumCd;

        projectileHolder.Add(beanObject);
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //    Fire(lineRenderer.positionCount <= 0);
        for (int i = 0; i < projectileHolder.Count; i++) {
            int instanceID = projectileHolder[i].GetInstanceID();

            if (projectileHolder[i].duration + projectileHolder[i].spawnTime < PropertiesUtility.time)
            {
                projectileHolder[i].lineRenderer.positionCount = 0;
            }

            if (PropertiesUtility.time > projectileHolder[i].duration + projectileHolder[i].spawnTime + projectileHolder[i].baseBullet.loadUpCd) {
                projectileHolder[i].lineRenderer.positionCount = 2;
                projectileHolder[i].spawnTime = PropertiesUtility.time;
            }

            if (projectileHolder[i].fromCharacter == null || 
                (projectileHolder[i].fromCharacter != null && projectileHolder[i].fromCharacter.GetType() != typeof(PlayerUnit))) {

                Vector3 direction = new Vector3(Mathf.Cos(projectileHolder[i].baseBullet.angle * Mathf.Deg2Rad), Mathf.Sin(projectileHolder[i].baseBullet.angle * Mathf.Deg2Rad), 0);
                UpdateBeamState(projectileHolder[i], direction);
            }
        }
    }

    public void UpdateBeamState(BeamObject beamObject, Vector3 direction) {
        beamObject.lineRenderer.SetPositions( GetBeamPaths(beamObject, direction) );
    }

    Vector3[] GetBeamPaths(BeamObject beamObject, Vector3 direction)
    {
        Vector3 characterPos = beamObject.fromCharacter.transform.position;

        int hitsNum = Physics2D.RaycastNonAlloc(characterPos, direction, beamLengthRaycastCache, beamObject.baseBullet.beamLength);
        Vector3 default_dest = characterPos + (direction) * beamObject.baseBullet.beamLength;
        Vector3 destination = FindFirstRaycast(hitsNum, beamLengthRaycastCache, default_dest, beamObject);

        beamPaths[0].Set(characterPos.x, characterPos.y, 0);
        beamPaths[1].Set(destination.x, destination.y, 0);
        return beamPaths;
    }

    private Vector3 FindFirstRaycast(int hitLength, RaycastHit2D[] hits, Vector3 defaultDest, BeamObject beamObject)
    {
        for (int i = 0; i < hitLength; i++)
        {
            if (hits[i].collider.gameObject != beamObject.fromCharacter.gameObject)
                return hits[i].point;
        }

        return defaultDest;
    }

}
