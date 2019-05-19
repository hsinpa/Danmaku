using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace STP {
    [CreateAssetMenu(fileName = "[STP]Tile", menuName = "STP/Tile", order = 1)]
    public class TileSTPObject : ScriptableObject
    {
        public string _tag;
        public Tile[] tiles;

        public Tile GetSprite() {
            if (tiles == null || tiles.Length <= 0) return null;

            return tiles[Random.Range(0, tiles.Length)];
        }
    }
}
