using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PCG.SpelunkyMap
{
    public class TileGenerator : MonoBehaviour
    {
        [SerializeField]
        private Vector2Int roomRadius;

        [SerializeField]
        private GameObject BrickPrefab;

        [SerializeField]
        private GameObject EmptyPrefab;

        private MapGenerator.DungeonLayout dungeonLayout;


        private void Start()
        {
            GenerateBottomRoom(new MapGenerator.RoomInfo(5, 5));
        }

        public void SetUp(MapGenerator.DungeonLayout dungeonLayout) {
            this.dungeonLayout = dungeonLayout;
            GenerateTile();
        }

        public void GenerateTile()
        {
            for (int y = this.dungeonLayout.height - 1; y >= 0; y--)
            {
                for (int x = 0; x < this.dungeonLayout.width; x++)
                {
                    switch (this.dungeonLayout.layout[x, y].roomStyle) {
                        case MapGenerator.RoomStyle.Bottom:
                            GenerateBottomRoom(this.dungeonLayout.layout[x, y]);
                            break;
                        case MapGenerator.RoomStyle.FreeStyle:
                            GenerateLeftRightRoom(this.dungeonLayout.layout[x, y]);
                            break;
                        case MapGenerator.RoomStyle.LeftRightOnly:
                            GenerateLeftRightRoom(this.dungeonLayout.layout[x, y]);
                            break;
                        case MapGenerator.RoomStyle.Up:
                            GenerateUpRoom(this.dungeonLayout.layout[x, y]);
                            break;
                    }
                }
            }
        }


        private void GenerateBottomRoom(MapGenerator.RoomInfo roomInfo)
        {
            var roomDecoration = new RoomDecoration(roomRadius.x, roomRadius.y, new Vector2Int(roomInfo.x, roomInfo.y));

            roomDecoration = CreateWall(new Vector2Int(0, roomDecoration.height - 1), roomDecoration, Vector2Int.right, roomDecoration.width, false);
            roomDecoration = CreateWall(new Vector2Int(0, 0), roomDecoration, Vector2Int.right, roomDecoration.width, true);
            roomDecoration = CreateWall(new Vector2Int(0, roomDecoration.height - 1), roomDecoration, Vector2Int.down, roomDecoration.height, true);
            roomDecoration = CreateWall(new Vector2Int(roomDecoration.width - 1, roomDecoration.height - 1), roomDecoration, Vector2Int.down, roomDecoration.height, true);

            RenderRoom(roomInfo, roomDecoration);
        }

        private void GenerateUpRoom(MapGenerator.RoomInfo roomInfo)
        {
            var roomDecoration = new RoomDecoration(roomRadius.x, roomRadius.y, new Vector2Int(roomInfo.x, roomInfo.y));

            roomDecoration = CreateWall(new Vector2Int(0, roomDecoration.height - 1), roomDecoration, Vector2Int.right, roomDecoration.width, true);
            roomDecoration = CreateWall(new Vector2Int(0, 0), roomDecoration, Vector2Int.right, roomDecoration.width, false);
            roomDecoration = CreateWall(new Vector2Int(0, roomDecoration.height - 1), roomDecoration, Vector2Int.down, roomDecoration.height, true);
            roomDecoration = CreateWall(new Vector2Int(roomDecoration.width - 1, roomDecoration.height - 1), roomDecoration, Vector2Int.down, roomDecoration.height, true);

            RenderRoom(roomInfo, roomDecoration);
        }

        private void GenerateLeftRightRoom(MapGenerator.RoomInfo roomInfo) {
            var roomDecoration = new RoomDecoration(roomRadius.x, roomRadius.y, new Vector2Int(roomInfo.x, roomInfo.y));

            roomDecoration = CreateWall(new Vector2Int(0, roomDecoration.height - 1), roomDecoration, Vector2Int.right, roomDecoration.width, false);
            roomDecoration = CreateWall(new Vector2Int(0, 0), roomDecoration, Vector2Int.right, roomDecoration.width, false);
            roomDecoration = CreateWall(new Vector2Int(0, roomDecoration.height - 1), roomDecoration, Vector2Int.down, roomDecoration.height, true);
            roomDecoration = CreateWall(new Vector2Int(roomDecoration.width - 1, roomDecoration.height - 1), roomDecoration, Vector2Int.down, roomDecoration.height, true);

            RenderRoom(roomInfo ,roomDecoration);
        }

        private RoomDecoration CreateWall(Vector2Int startPoint, RoomDecoration roomDecoration, Vector2Int axis, int length, bool door) {
            int doorSize = Random.Range(1, Mathf.RoundToInt(length / 1.8f) + 1);
            int doorStartIndex = Random.Range(1, length - doorSize);

            for (int i = 0; i < length; i++) {
                Vector2Int newIndex = startPoint + (axis * i);

                if (door && i >= doorStartIndex && i < doorStartIndex + doorSize)
                {
                    roomDecoration.decorationCode[newIndex.x, newIndex.y] = "D";
                }
                else {
                    roomDecoration.decorationCode[newIndex.x, newIndex.y] = "1";
                }
            }
            return roomDecoration;
        }

        private Vector2 FindCenterWorldPosition() {
            return Vector2.zero + roomRadius;
        }

        private void RenderRoom(MapGenerator.RoomInfo p_roomInfo, RoomDecoration p_roomDecoration)
        {

           int xOffset = roomRadius.x * p_roomInfo.x, yOffset = roomRadius.y * p_roomInfo.y;
            
            string renderString = "";
            for (int y = p_roomDecoration.height - 1; y >= 0; y--)
            {
                for (int x = 0; x < p_roomDecoration.width; x++)
                {
                    renderString += p_roomDecoration.decorationCode[x, y];

                    //if (p_roomDecoration.decorationCode[x, y] == "0") {
                    //    var pObject = Instantiate(EmptyPrefab);
                    //    pObject.transform.position = new Vector2(x, y);
                    //}


                    if (p_roomDecoration.decorationCode[x, y] == "1")
                    {
                        var pObject = Instantiate(BrickPrefab);
                        pObject.transform.position = new Vector2(xOffset + x, yOffset + y);
                    }

                    if (x == p_roomDecoration.width - 1)
                        renderString += "\n";
                }
            }

            //Debug.Log(renderString);
        }

        public struct RoomDecoration {
            public string[,] decorationCode;
            public int width, height;
            public Vector2Int index;

            public List<DoorLayout> doorList;

            private DoorLayout NoDoorData;

            public RoomDecoration(int width, int height, Vector2Int index)
            {
                this.width = width;
                this.height = height;
                this.index = index;
                this.decorationCode = new string[width, height];

                for (int y = height - 1; y >= 0; y--)
                {
                    for (int x = 0; x < width; x++)
                    {
                        decorationCode[x, y] = "0";
                    }
                }

                doorList = new List<DoorLayout>();
                NoDoorData = new DoorLayout(0, 0, Vector2Int.zero);
            }

            public DoorLayout FindDoor(DoorLayout.DoorDir doorDir) {
                if (doorList.Count <= 0) {
                    return this.NoDoorData;
                }

                foreach (var door in doorList) {
                    if (door.doorDirection == doorDir)
                        return door;
                }

                return this.NoDoorData;
            }
        }

        public struct DoorLayout {
            public enum DoorDir {
                None, Left, Right, Down,Up
            }

            public int length, startPoint;
            public DoorDir doorDirection;

            public DoorLayout(int length, int startPoint, Vector2Int doorAxis)
            {
                this.length = length;
                this.startPoint = startPoint;
                this.doorDirection = DoorDir.None;
            }


        }

    }
}