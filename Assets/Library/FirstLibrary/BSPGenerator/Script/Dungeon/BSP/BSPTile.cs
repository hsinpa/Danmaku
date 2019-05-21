using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    public struct BSPTile
    {
        public int x, y, worldX, worldY;

        public BSPMapComponent bspComponent;
        public string tile_id;

        public BSPTile(int x, int y, int worldX, int worldY, string tile_id) {
            this.x = x;
            this.y = y;
            this.worldX = worldX;
            this.worldY = worldY;

            this.tile_id = tile_id;

            this.bspComponent = null;
        }
    }
}