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

        private MapGenerator.DungeonLayout roomLayout;


        private void Start()
        {
            GenerateLeftRightRoom(new MapGenerator.RoomInfo(5, 5));
        }

        public void SetUp(MapGenerator.DungeonLayout roomLayout) {
            this.roomLayout = roomLayout;
        }

        public void GenerateTile(MapGenerator.RoomStyle roomStyle)
        {


        }

        private void GenerateLeftRightRoom(MapGenerator.RoomInfo roomInfo) {
            var roomDecoration = new RoomDecoration(roomRadius.x, roomRadius.y);

            roomDecoration = CreateWall(new Vector2Int(0, roomDecoration.height - 1), roomDecoration, Vector2Int.right, roomDecoration.width, false);
            roomDecoration = CreateWall(new Vector2Int(0, 0), roomDecoration, Vector2Int.right, roomDecoration.width, false);
            roomDecoration = CreateWall(new Vector2Int(0, roomDecoration.height - 1), roomDecoration, Vector2Int.down, roomDecoration.height, true);
            roomDecoration = CreateWall(new Vector2Int(roomDecoration.width-1, roomDecoration.height - 1), roomDecoration, Vector2Int.down, roomDecoration.height, true);

            RenderRoom(roomDecoration);
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

        private void RenderRoom(RoomDecoration p_roomDecoration)
        {
            string renderString = "";
            for (int y = p_roomDecoration.height - 1; y >= 0; y--)
            {
                for (int x = 0; x < p_roomDecoration.width; x++)
                {
                    renderString += p_roomDecoration.decorationCode[x, y];

                    if (p_roomDecoration.decorationCode[x, y] == "0") {
                        var pObject = Instantiate(EmptyPrefab);
                        pObject.transform.position = new Vector2(x, y);
                    }


                    if (p_roomDecoration.decorationCode[x, y] == "1")
                    {
                        var pObject = Instantiate(BrickPrefab);
                        pObject.transform.position = new Vector2(x, y);

                    }

                    if (x == p_roomDecoration.width - 1)
                        renderString += "\n";
                }
            }

            Debug.Log(renderString);
        }

        public struct RoomDecoration {
            public string[,] decorationCode;
            public int width, height;

            public RoomDecoration(int width, int height)
            {
                this.width = width;
                this.height = height;
                this.decorationCode = new string[width, height];

                for (int y = height - 1; y >= 0; y--)
                {
                    for (int x = 0; x < width; x++)
                    {
                        decorationCode[x, y] = "0";
                    }
                }
            }
        }

    }
}