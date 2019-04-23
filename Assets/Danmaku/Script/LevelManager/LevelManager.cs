using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

[System.Serializable]
public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private PlayerUnit player;

    [SerializeField]
    private Transform projectileHolder;

    [SerializeField]
    private List<Wave> waves;

    private int waveIndex;

    private TilemapReader _tilemapReader;

    private Transform enemyHolder;

    private List<AIUnit> total_aiUnit;

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

    public void SetUp(TilemapReader tilemapReader) {
        _tilemapReader = tilemapReader;
        waveIndex = -1;
        total_aiUnit = new List<AIUnit>();

        player.SetUp(projectileHolder);
        enemyHolder = transform.Find("Unit/EnemyHolder");

        StartCoroutine( Spawn(1, PrepareWave() ) );
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
                }
            }
        }

        return inactiveUnit;
    }

    public IEnumerator Spawn(float p_delayTime, List<AIUnit> p_inactiveUnits) {
        yield return new WaitForSeconds(p_delayTime);

        List<Node> emptyNodes = _tilemapReader.GetEmptyNode();
        if (emptyNodes != null && emptyNodes.Count > 0) {
            int randomSpawnUnit = Random.Range(0, 6);

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

        float randomDelayTime = Random.Range(0.3f, 1.5f);
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

                if (total_aiUnit.Count <= 0 || total_aiUnit.Count / totalUnitInWave <= waves[waveIndex].remaining_spawn_point) {
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
