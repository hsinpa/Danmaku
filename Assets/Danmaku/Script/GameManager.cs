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
    private TileMapBuilder tileMapBuilder;

    private Transform ProjectileHolder;

    private void Awake()
    {
        tileMapBuilder = GetComponentInChildren<TileMapBuilder>();
        tilemapReader = GetComponentInChildren<TilemapReader>();
        pathfinding = GetComponentInChildren<Pathfinding>();
        levelManager = GetComponent<LevelManager>();

        ProjectileHolder = this.transform.Find("Projectile");

        tileMapBuilder.SetUp();
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        tileMapBuilder.Generate();

        tilemapReader.SetUp();
        pathfinding.SetUp(tilemapReader);
        levelManager.SetUp(tilemapReader, tileMapBuilder);
    }

}
