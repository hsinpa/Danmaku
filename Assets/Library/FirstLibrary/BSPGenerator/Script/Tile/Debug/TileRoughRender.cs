using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG;
using STP;
using UnityEngine.Tilemaps;
using Utility;

public class TileRoughRender : MonoBehaviour
{
    [SerializeField]
    private Vector2 tile_offset;
    private Vector2Int map_offset;

    [SerializeField]
    private TileSTPSet tileSet;

    [SerializeField]
    private UnityEngine.Grid tileGrid;

    private BSP bspMap;

    string[,] mapData;

    private enum TileType {
        Background, Wall, Front
    }

    private void Awake()
    {
        bspMap = this.GetComponent<BSP>();


        if (bspMap != null)
            bspMap.OnMapBuild += RenderTile;
    }

    public void RenderTile(Vector2Int dungeonSize, List<BSPMapComponent> bspComponents) {

        map_offset = new Vector2Int(Mathf.RoundToInt( -dungeonSize.x), Mathf.RoundToInt(-dungeonSize.y));

        var dungeonFullSize = new Vector2Int(dungeonSize.x * 2, dungeonSize.y * 2);
        //1 = wall , 0 = empty
        mapData = new string[dungeonFullSize.x, dungeonFullSize.y];

        FloodFillMap(dungeonFullSize, map_offset);

        for (int i = 0; i < bspComponents.Count; i++)
        {

            for (float x = bspComponents[i].spaceRect.xMin; x < bspComponents[i].spaceRect.xMax; x++)
            {
                for (float y = bspComponents[i].spaceRect.yMin; y < bspComponents[i].spaceRect.yMax; y++)
                {
                    int xIndex = Mathf.RoundToInt(dungeonSize.x + x);
                    int yIndex = Mathf.RoundToInt(dungeonSize.y + y);
                    mapData[xIndex, yIndex] = "0";

                    if (bspComponents[i].GetType() == typeof(BSPRoom))
                    {
                        BSPRoom room = (BSPRoom)bspComponents[i];
                        for (int d = 0; d < room.doorPosition.Count; d++)
                        {
                            Vector2Int offsetPos = room.doorPosition[d] + new Vector2Int(24, 24);
                            //Debug.Log(offsetPos.x +", "+ offsetPos.y);
                            mapData[offsetPos.x, offsetPos.y] = "1";
                        }
                    }
                }
            }
        }

        RenderToWorldCanvas(map_offset, dungeonFullSize, mapData);
    }

    private void RenderToWorldCanvas(Vector2Int offset, Vector2Int dungeonSize, string[,] mapData) {

        int index = 0;

        TileInfo wallTiles = new TileInfo("1");


        for (int x = 0; x < dungeonSize.x; x++)
        {
            for (int y = 0; y < dungeonSize.y; y++)
            {
                //Debug.Log( x +", " + y +","+ mapData[x, y] +", " + (mapData[x, y] == null));

                if (mapData[x, y] == null)
                    mapData[x, y] = "1";

                var tileSTP = tileSet.GetTile(mapData[x, y]);

                switch (mapData[x, y]) {
                    case "0":
                        break;

                    case "1":
                        wallTiles.Add(x + offset.x, y + offset.y, tileSTP.GetSprite());
                        break;
                }

                //GameObject p = Instantiate(tilePrefab);
                //p.transform.position = new Vector2(x, y) + offset + tile_offset;
                //SpriteRenderer sRenderer = p.GetComponent<SpriteRenderer>();
                //var tileSTP = tileSet.GetTile(mapData[x, y]);

                //if (tileSTP != null) {
                //    sRenderer.sprite = tileSTP.GetSprite();
                //}

                index++;
            }
        }

        RenderTileInfo(wallTiles, TileType.Wall);
    }

    private void RenderTileInfo(TileInfo tileInfo, TileType tileType) {
        Tilemap tileMapObject = GetTileMap(tileType);
        if (tileMapObject != null)
        {
            tileMapObject.ClearAllTiles();

            tileMapObject.SetTiles(tileInfo.tilePosition, tileInfo.tiles);
        }
    }

    private void FloodFillMap(Vector2Int dungeonSize, Vector2Int offset) {

        Tilemap backgroundTile = GetTileMap(TileType.Background);
        Vector3Int reusePosition = Vector3Int.zero;

        if (backgroundTile != null) {
            backgroundTile.ClearAllTiles();

            int mapLength = dungeonSize.x * dungeonSize.y;
            Vector3Int[] tilePosition = new Vector3Int[mapLength];
            TileBase[] tiles = new TileBase[mapLength];
            var tileSTP = tileSet.GetTile("0");

            int index = 0;

            for (int x = 0; x < dungeonSize.x; x++)
            {
                for (int y = 0; y < dungeonSize.y; y++)
                {
                    
                    reusePosition.Set(x + offset.x, y + offset.y, 0);
                    tilePosition[index] = reusePosition;
                    tiles[index] = tileSTP.GetSprite();

                    index++;
                }
            }
            backgroundTile.SetTiles(tilePosition, tiles);
        }
    }

    private Tilemap GetTileMap(TileType p_type) {
        var tilemapObject = tileGrid.transform.GetChild((int) p_type);
        if (tilemapObject == null)
            return null;

        return tilemapObject.GetComponent<Tilemap>();
    }

    private void OnDestroy()
    {
        if (bspMap != null)
            bspMap.OnMapBuild -= RenderTile;
    }

    private struct TileInfo {
        public Vector3Int[] tilePosition {
            get {
                return _tilePosition.ToArray();
            }
        }

        public TileBase[] tiles
        {
            get
            {
                return _tiles.ToArray();
            }
        }

        List<Vector3Int> _tilePosition;
        List<TileBase> _tiles;
        public string id;

        Vector3Int reusableVector;

        public TileInfo(string id) {
            this.id = id;
            _tilePosition = new List<Vector3Int>();
            _tiles = new List<TileBase>();
            reusableVector = Vector3Int.zero;
        }

        public void Add(int x, int y, TileBase tile) {
            reusableVector.Set(x, y, 0);

            _tilePosition.Add(reusableVector);
            _tiles.Add(tile);
        }
    }
}
