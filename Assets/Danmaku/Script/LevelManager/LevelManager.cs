using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using Pooling;
using PG;
[System.Serializable]
public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private PlayerUnit player;

    [SerializeField]
    private Transform projectileHolder;

    [SerializeField]
    private List<Wave> waves;

    [SerializeField]
    private Pooling.STPTheme themeObject;

    private int waveIndex;

    private TilemapReader _tilemapReader;

    private Transform enemyHolder;

    private List<AIUnit> total_aiUnit;
    private int total_aiUnit_count;

    private int totalUnitInWave {
        get {
            int total = 0;
            foreach (var segment in waves[waveIndex].enemySegments)
            {
                total += segment.spawn_number;
            }
            return total;
        }
    }

    public void SetUp(TilemapReader tilemapReader, TileMapBuilder tileMapBuilder) {
        _tilemapReader = tilemapReader;
        waveIndex = -1;
        total_aiUnit = new List<AIUnit>();

        player.SetUp(projectileHolder, tileMapBuilder.bspMap.startRoom.spaceRect.center);
        enemyHolder = transform.Find("Unit/EnemyHolder");

        PreparePoolingObject(themeObject);
        StartCoroutine( Spawn(1, PrepareWave() ) );
    }

    private void PreparePoolingObject(STPTheme p_themeObject) {
        if (p_themeObject != null && p_themeObject.stpObjectHolder != null) {
            for (int i = 0; i < p_themeObject.stpObjectHolder.Count; i++) {
                var stpObject = p_themeObject.stpObjectHolder[i];
                PoolManager.instance.CreatePool(stpObject.prefab, stpObject._id, stpObject.poolingNum);
            }
        }
    }

    public List<AIUnit> PrepareWave() {

        List<AIUnit> inactiveUnit = new List<AIUnit>();

        if (waves.Count > waveIndex + 1) {
            waveIndex++;

            Wave wave = waves[waveIndex];
            if (wave.enemySegments != null) {
                for (int s_index = 0; s_index < wave.enemySegments.Length; s_index++) {
                    EnemySegment unitSegment = wave.enemySegments[s_index];

                    for (int unit_index = 0; unit_index < unitSegment.spawn_number; unit_index++)
                    {
                        GameObject spawnUnit = InstantiateUnit(unitSegment.prefab);
                        spawnUnit.SetActive(false);

                        inactiveUnit.Add(spawnUnit.GetComponent<AIUnit>());
                    }
                    total_aiUnit_count += unitSegment.spawn_number;
                }
            }
        }

        return inactiveUnit;
    }

    public IEnumerator Spawn(float p_delayTime, List<AIUnit> p_inactiveUnits) {
        yield return new WaitForSeconds(p_delayTime);

        List<Node> emptyNodes = _tilemapReader.GetEmptyNode();
        if (emptyNodes != null && emptyNodes.Count > 0) {
            int randomSpawnUnit = Random.Range(0, 3);

            for (int i = 0; i < randomSpawnUnit; i++) {
                if (p_inactiveUnits.Count > 0) {
                    AIUnit unit = p_inactiveUnits[0];
                    Node randomNode = emptyNodes[Random.Range(0, emptyNodes.Count)];

                    unit.transform.position = randomNode.worldPosition;
                    unit.gameObject.SetActive(true);
                    unit.SetUp(waveIndex.ToString(), projectileHolder, player.transform);
                    unit.OnDestroy += OnAIUnitDestroy;

                    total_aiUnit.Add(unit);

                    p_inactiveUnits.RemoveAt(0);
                }
            }
        }

        float randomDelayTime = Random.Range(0.3f, 3f);
        if (p_inactiveUnits.Count > 0)
            StartCoroutine(Spawn(randomDelayTime, p_inactiveUnits));
    }

    private GameObject InstantiateUnit(GameObject p_prefab) {
        GameObject unit = Instantiate(p_prefab);

        if (enemyHolder != null)
            unit.transform.SetParent(enemyHolder);

        return unit;
    }

    private void OnAIUnitDestroy(BaseCharacter p_unit) {


        if (p_unit.GetType() == typeof(AIUnit))
        {

            AIUnit aiUnit = (AIUnit)p_unit;

            bool isRemove = total_aiUnit.Remove(aiUnit);
            if (isRemove) {
                Destroy(p_unit.gameObject);

                if (total_aiUnit.Count <= 0 || total_aiUnit_count / (float)totalUnitInWave <= waves[waveIndex].remaining_spawn_point) {
                    StartCoroutine(Spawn(1, PrepareWave()));
                }
            }
        }
    }

    [System.Serializable]
    public struct Wave {

        public EnemySegment[] enemySegments;

        public float remaining_spawn_point;
    }

    [System.Serializable]
    public struct EnemySegment {
        public GameObject prefab;
        public int spawn_number;


    }
}
