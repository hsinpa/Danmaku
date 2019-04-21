using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class Node : IHeapItem<Node> {
	
	public bool walkable;
	public Vector3 worldPosition;
	public int gridX;
	public int gridY;
	public int movementPenalty;

	public int gCost;
	public int hCost;
	public Node parent;
	int heapIndex;

    public TileBase tileBase { get; set; }

    public Tilemap tilemapMember { get; set; }


    public Node(bool _walkable, Vector3 _worldPos, TileBase _tilebase, Tilemap _tilemap, int _gridX, int _gridY, int _penalty) {
		walkable = _walkable;
		worldPosition = _worldPos;
        tileBase = _tilebase;
        tilemapMember = _tilemap;
        gridX = _gridX;
		gridY = _gridY;
		movementPenalty = _penalty;
	}

	public int fCost {
		get {
			return gCost + hCost;
		}
	}

	public int HeapIndex {
		get {
			return heapIndex;
		}
		set {
			heapIndex = value;
		}
	}

	public int CompareTo(Node nodeToCompare) {
		int compare = fCost.CompareTo(nodeToCompare.fCost);
		if (compare == 0) {
			compare = hCost.CompareTo(nodeToCompare.hCost);
		}
		return -compare;
	}
}
