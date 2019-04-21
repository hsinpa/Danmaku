using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUnit : BaseCharacter
{
    public Transform target;

    Vector3[] paths;
    int moveIndex = 0;

    private Vector3 targetDir {
        get {
            if (target != null) {
                return (target.position - transform.position).normalized;
            }
            return Vector3.zero;
        }
    }

    private BaseBullet baseBullet;

    public void Start()
    {
        base.Init();
        SearchPlayer();

        baseBullet = GetComponent<BaseBullet>();
        baseBullet.projectileHolder = projectileHolder;
    }

    private void SearchPlayer() {
        PathRequestManager.RequestPath(new PathRequest(this.transform.position, target.position, (Vector3[] paths, bool isSuccess) => {
            this.paths = paths;

            this.moveIndex = 0;
        }));
    }

    private void Update()
    {
        baseBullet.Fire(targetDir);
    }

    private void FixedUpdate()
    {
        if (paths != null && paths.Length > 0)
        {
            if (moveIndex == paths.Length - 1) {
                SearchPlayer();
                return;
            } else if (paths.Length > moveIndex + 0.1f) {
                float distance = Vector3.Distance(paths[moveIndex], transform.position);
                if (distance < 0.1f) {
                    moveIndex++;
                }
            }
           
            actions.Move((paths[moveIndex] - transform.position).normalized);
        }

    }
}
