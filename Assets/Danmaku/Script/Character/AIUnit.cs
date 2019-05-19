using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUnit : BaseCharacter
{
    public Transform target;

    Vector3[] paths;
    int moveIndex = 0;

    public string spawnID {
        get {
            return _spawnID;
        }
    }
    private string _spawnID;

    private float forceResearchPosition = 0.5f;
    private float recordResearchTime = 0;

    [SerializeField]
    private bool autoMove = true;

    private Vector3 targetDir {
        get {
            if (target != null) {
                return (target.position - transform.position).normalized;
            }
            return Vector3.zero;
        }
    }

    public void Start()
    {
        //SetBoss(target);
    }

    public void SetUp(string p_spawnId, Transform p_projectileHolder, Transform p_target)
    {
        _spawnID = p_spawnId;
        projectileHolder = p_projectileHolder;
        target = p_target;
        base.Init();

        danmakuReader.SetUp(projectileHolder.GetComponent<BulletStateCtrl>());
        //SearchPlayer();
    }


    private void SearchPlayer() {
        PathRequestManager.RequestPath(new PathRequest(this.transform.position, target.position, (Vector3[] paths, bool isSuccess) => {
            this.paths = paths;

            this.moveIndex = 0;
        }));
    }

    private void Update()
    {
        if (danmakuReader != null)
            danmakuReader.Fire(targetDir);
    }

    private void FixedUpdate()
    {
        if (!autoMove) return;

        if (Time.time > recordResearchTime)
        {
            recordResearchTime = Time.time + forceResearchPosition;

            SearchPlayer();
            return;
        }

        if (paths != null && paths.Length > 0)
        {
            if (moveIndex == paths.Length - 1)
            {
                SearchPlayer();
                return;
            }
            else if (paths.Length > moveIndex + 0.1f)
            {
                float distance = Vector3.Distance(paths[moveIndex], transform.position);
                if (distance < 0.05f)
                {
                    moveIndex++;
                }
            }

            actions.Move((paths[moveIndex] - transform.position).normalized);
        }
    }
}
