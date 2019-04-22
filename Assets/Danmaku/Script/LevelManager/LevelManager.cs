using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private List<Wave> waves;

    private int waveIndex;

    private TilemapReader _tilemapReader;

    public void SetUp(TilemapReader tilemapReader) {
        _tilemapReader = tilemapReader;
        waveIndex = 0;

    }

    public void Spawn() {
        
    }

    public struct Wave {

        public EnemyCombination[] enemySegments;

        public float remaining_spawn_point;
    }

    public struct EnemyCombination {
        public EnemySegment[] enemySegment;
    }

    public struct EnemySegment {
        public GameObject prefab;
        public int spawn_number;


    }
}
