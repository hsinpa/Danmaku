using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class TilemapReader : MonoBehaviour
{
    [SerializeField]
    private Transform grid;

    private Dictionary<Vector3, Node> _nodes = new Dictionary<Vector3, Node>();

    public int MaxTileSize {
        get {
            if (_nodes == null) return 0;

            return _nodes.Count;
        }
    }
    private Vector3 gridSize;

    public Vector3 tileAnchor {
        get {
            return _tileAnchor;
        }
    }
    private Vector3 _tileAnchor;

    public void SetUp()
    {
        gridSize = GetMaxGridSize(grid);
        _tileAnchor = GetAnchorSize(grid);

        _nodes = ReadGrid(grid);
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    var worldPoint = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), 0);
        //    Node _node;

        //    if (_nodes.TryGetValue(worldPoint, out _node))
        //    {
        //        print("Tile worldPoint " + _node.worldPosition +", " + _node.walkable);
        //    }
        //}
    }

    private Vector3 GetAnchorSize(Transform p_grid) {
        if (p_grid.childCount > 0) {
            return p_grid.GetChild(0).GetComponent<Tilemap>().tileAnchor;
        }
        return Vector3.zero;
    }

    private Vector2 GetMaxGridSize(Transform p_grid) {
        Vector2 gridSize = Vector2.zero;

        for (int i = 0; i < p_grid.childCount; i++)
        {
            Tilemap tile = p_grid.GetChild(i).GetComponent<Tilemap>();

            if (tile.size.x > gridSize.x)
                gridSize.x = tile.size.x;

            if (tile.size.y > gridSize.y)
                gridSize.y = tile.size.y;
        }

        return gridSize;
    }

    public Dictionary<Vector3, Node> ReadGrid(Transform p_grid) {
        var tiles = new Dictionary<Vector3, Node>();

        for (int i = 0; i < p_grid.childCount; i++) {
            Tilemap tile = p_grid.GetChild(i).GetComponent<Tilemap>();
            int layer = tile.gameObject.layer;

            if (tile == null) continue;

            foreach (Vector3Int pos in tile.cellBounds.allPositionsWithin)
            {
                var localPlace = new Vector3Int(pos.x, pos.y, pos.z);

                if (!tile.HasTile(localPlace)) continue;
                var key = tile.CellToWorld(localPlace);
                var node = new Node
                (
                    _walkable: (layer == VariableFlag.LayerMask.barrierLayer) ? false : true,
                    _worldPos: key + _tileAnchor,
                    _tilebase: tile.GetTile(localPlace),
                    _tilemap: tile,
                    _gridX: localPlace.x,
                    _gridY: localPlace.y,
                    _penalty: 1
                );

                if (tiles.ContainsKey(key))
                {
                    tiles[key] = node;
                }
                else {
                    tiles.Add(key, node);
                }
            }
        }

        return tiles;
    }

    public Node NodeFromWorldPoint(Vector3 p_key) {
        var worldPoint = new Vector3Int(Mathf.FloorToInt(p_key.x), Mathf.FloorToInt(p_key.y), 0);
        Node node;

        if (_nodes.TryGetValue(worldPoint, out node))
        {
            //print( string.Format("Tile ({0}, {1}) costs: {2}", node.gridX, node.gridY, node.movementPenalty) );
            return node;
        }

        return null;
    }


    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        Vector2 NeighbourPos = Vector2.zero;
        //for (int x = -1; x <= 1; x++)
        //{
        //    for (int y = -1; y <= 1; y++)
        //    {
        //        if (x == 0 && y == 0)
        //            continue;

        //        int checkX = node.gridX + x;
        //        int checkY = node.gridY + y;
        //        NeighbourPos.Set(node.worldPosition.x + x, node.worldPosition.y + y);

        //        var neighbourNode = NodeFromWorldPoint(NeighbourPos);
        //        if (neighbourNode == null) continue;

        //        neighbours.Add(neighbourNode);

        //    }
        //}

        Vector2[] directionSet = new Vector2[] { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0) };
        foreach (Vector2 dirSet in directionSet)
        {
            NeighbourPos.Set(node.worldPosition.x + dirSet.x, node.worldPosition.y + dirSet.y);

            var neighbourNode = NodeFromWorldPoint(NeighbourPos);
            if (neighbourNode == null) continue;

            neighbours.Add(neighbourNode);
        }
        return neighbours;
    }

    public List<Node> GetEmptyNode() {
        var nodelist = _nodes.Values.ToList();
        var freeSpaceNode = new List<Node>();

        freeSpaceNode = nodelist.FindAll(x => x.walkable);

        return freeSpaceNode;
    }

}
