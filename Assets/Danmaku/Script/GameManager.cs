using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum State {
        Preload,
        Start,
        End
    }

    public State state {
        get {
            return _state;
        }
    }
    private State _state;

    private Pathfinding pathfinding;
    private TilemapReader tilemapReader;
    private LevelManager levelManager;
    private PG.BSP bspManager;

    private Transform ProjectileHolder;

    private void Awake()
    {
        bspManager = GetComponentInChildren<PG.BSP>();
        tilemapReader = GetComponentInChildren<TilemapReader>();
        pathfinding = GetComponentInChildren<Pathfinding>();
        levelManager = GetComponent<LevelManager>();

        ProjectileHolder = this.transform.Find("Projectile");

    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        bspManager.SetUp();
        bspManager.GenerateMap();

        tilemapReader.SetUp();
        pathfinding.SetUp(tilemapReader);
        levelManager.SetUp(tilemapReader, bspManager);
    }

}
