using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG.SpelunkyMap {
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField]
        Vector2Int size;

        public enum RoomStyle {
            FreeStyle = 0,
            LeftRightOnly,
            Bottom,
            Up
        }

        private enum MoveDir {
            Left, Right, Down, End
        }

        TileGenerator tileGenerator;

        private void Start()
        {
            tileGenerator = this.GetComponent<TileGenerator>();
            var dungeonLayout = Generate(size.x, size.y);

            tileGenerator.SetUp(dungeonLayout);
            RenderRoom(dungeonLayout);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var completedRoom = Generate(size.x, size.y);
                RenderRoom(completedRoom);
            }
        }

        public DungeonLayout Generate(int width, int height) {
            DungeonLayout dungeonLayout = new DungeonLayout(width, height);

            dungeonLayout = GenerateMainPath(dungeonLayout);

            return dungeonLayout;
        }

        private DungeonLayout GenerateMainPath(DungeonLayout dungeonLayout) {
            //Start from First Row
            int randX = (Random.Range(0, dungeonLayout.width));

            dungeonLayout.layout[randX, dungeonLayout.height - 1] = new RoomInfo(randX, dungeonLayout.height - 1);
            dungeonLayout.layout[randX, dungeonLayout.height - 1].roomState = RoomInfo.RoomState.StartPoint;

            RoomInfo lastRoom = dungeonLayout.layout[randX, dungeonLayout.height - 1];

            while (true) {

                MoveDir nextMove = FindNextRoom(dungeonLayout, lastRoom);

                dungeonLayout.layout[lastRoom.x, lastRoom.y].previousIndex = new Vector2Int(lastRoom.x, lastRoom.y);

                if (nextMove == MoveDir.End) {
                    dungeonLayout.layout[lastRoom.x, lastRoom.y].roomState = RoomInfo.RoomState.EndPoint;
                    break;
                }

                int dirX = lastRoom.x, dirY = lastRoom.y;

                if (nextMove == MoveDir.Down)
                {
                    dirY -= 1;
                }

                if (nextMove == MoveDir.Left || nextMove == MoveDir.Right) {
                    dirX += ((nextMove == MoveDir.Left) ? -1 : 1);
                }

                dungeonLayout.layout[dirX, dirY] = new RoomInfo(dirX, dirY);
                dungeonLayout.layout[dirX, dirY].roomState = RoomInfo.RoomState.MainPath;
                dungeonLayout.layout[dirX, dirY].roomStyle = RoomStyle.LeftRightOnly;

                if (nextMove == MoveDir.Down)
                {
                    RoomStyle randamType = (RoomStyle)Random.Range(2, 4);
                    dungeonLayout.layout[dirX, dirY].roomStyle = randamType;
                    dungeonLayout.layout[lastRoom.x, lastRoom.y].roomStyle = RoomStyle.Bottom;
                }

                lastRoom = dungeonLayout.layout[dirX, dirY];
            }

            return dungeonLayout;
        }

        private RoomInfo CreateRoom(int x, int y, RoomInfo.RoomState roomState, RoomStyle roomType) {
            var roomInfo = new RoomInfo(x, y);
            roomInfo.roomState = roomState;
            roomInfo.roomStyle = roomType;
            return roomInfo;
        }

        private RoomInfo CreateDownSlotRoom(RoomInfo previousRoom) {
            if (previousRoom.y == 0)
            {
                previousRoom.roomState = RoomInfo.RoomState.EndPoint;
                return previousRoom;
            }
            else
            {
                return CreateRoom(previousRoom.x, previousRoom.y - 1, RoomInfo.RoomState.MainPath, RoomStyle.Up);
            }
        }

        private MoveDir FindNextRoom(DungeonLayout roomLayout, RoomInfo previousRoom) {
            //1,2 = Left, 3,4 = Right, 5 = Down
            int diceValue = Random.Range(1, 6);

            //Debug.Log(previousRoom.x +", " + previousRoom.y +", " + previousRoom.roomState.ToString("g"));

            if (diceValue == 1 || diceValue == 2)
            {
                if (!IsLeftRightAvailable(roomLayout, previousRoom, -1) &&
                    IsLeftRightAvailable(roomLayout, previousRoom, 1))
                {
                    return MoveDir.Right;
                }
                else if (!IsLeftRightAvailable(roomLayout, previousRoom, -1) &&
                    !IsLeftRightAvailable(roomLayout, previousRoom, 1))
                {
                    return CheckDownValidity(previousRoom);
                }

                return MoveDir.Left;
            }

            if (diceValue == 3 || diceValue == 4)
            {

                if (!IsLeftRightAvailable(roomLayout, previousRoom, 1) &&
                    IsLeftRightAvailable(roomLayout, previousRoom, -1))
                {
                    return MoveDir.Left;
                }
                else if (!IsLeftRightAvailable(roomLayout, previousRoom, 1) &&
                    !IsLeftRightAvailable(roomLayout, previousRoom, -1))
                {
                    return CheckDownValidity(previousRoom);
                }

                return MoveDir.Right;
            }

            return CheckDownValidity(previousRoom);
        }

        private bool IsLeftRightAvailable(DungeonLayout roomLayout, RoomInfo previousRoom, int axis) {

            bool outOfBoundary = (previousRoom.x + axis >= roomLayout.width || previousRoom.x + axis < 0);

            if (!outOfBoundary) {
                return roomLayout.layout[previousRoom.x + axis, previousRoom.y].roomState == RoomInfo.RoomState.Other;
            }

            return false;
        }

        private MoveDir CheckDownValidity(RoomInfo previousRoom) {

            if (previousRoom.y <= 0)
            {
                return MoveDir.End;
            }
            else
            {
                return MoveDir.Down;
            }
        }

        private void RenderRoom(DungeonLayout dungeonLayout)
        {
            string renderString = "";
            for (int y = dungeonLayout.height - 1; y >= 0; y--) {

                for (int x = 0; x < dungeonLayout.width; x++)
                {
                    Debug.Log("x:" + x +", y:"+y+", " + dungeonLayout.layout[x, y].roomState.ToString("g") +", " + dungeonLayout.layout[x, y].roomStyle.ToString("g"));

                    if (x == dungeonLayout.width - 1)
                        renderString += "\n";
                }
            }
        }

        public struct DungeonLayout {
            public int width, height;
            public RoomInfo[,] layout;

            public DungeonLayout(int width, int height) {
                this.width = width;
                this.height = height;
                this.layout = new RoomInfo[width, height];

                for (int y = height - 1; y >= 0; y--)
                {
                    for (int x = 0; x < width; x++)
                    {
                        layout[x, y] = new RoomInfo(x, y);
                    }
                }
            }
        }

        public struct RoomInfo {
            public int x, y;
            public RoomStyle roomStyle;
            public RoomState roomState;

            public Vector2Int previousIndex;
            

            public enum RoomState {
                Other,
                StartPoint,
                EndPoint,
                MainPath,
            }

            public RoomInfo(int x , int y) {
                this.x = x;
                this.y = y;
                roomStyle = RoomStyle.FreeStyle;
                roomState = RoomState.Other;

                previousIndex = Vector2Int.left;
            }


        }

    }
}
