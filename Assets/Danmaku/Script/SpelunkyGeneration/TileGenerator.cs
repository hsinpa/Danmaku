using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PCG.SpelunkyMap
{
    public class TileGenerator : MonoBehaviour
    {
        [SerializeField]
        private Vector2Int tileSize;

        private MapGenerator.RoomLayout roomLayout;

        public void SetUp(MapGenerator.RoomLayout roomLayout) {
            this.roomLayout = roomLayout;
        }

        public void GenerateTile(MapGenerator.RoomStyle roomStyle)
        {


        }

    }
}