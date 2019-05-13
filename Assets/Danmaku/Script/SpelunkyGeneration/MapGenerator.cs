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

        private void Start()
        {
            var completedRoom = Generate(size.x, size.y);

            RenderRoom(completedRoom);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var completedRoom = Generate(size.x, size.y);
                RenderRoom(completedRoom);
            }
        }

        public RoomLayout Generate(int width, int height) {
            RoomLayout roomLayout = new RoomLayout(width, height);

            roomLayout = GenerateMainPath(roomLayout);

            return roomLayout;
        }

        private RoomLayout GenerateMainPath(RoomLayout roomLayout) {
            //Start from First Row
            int randX = (Random.Range(0, roomLayout.width));

            roomLayout.layout[randX, roomLayout.height - 1] = new RoomInfo(randX, roomLayout.height - 1);
            roomLayout.layout[randX, roomLayout.height - 1].roomState = RoomInfo.RoomState.StartPoint;

            RoomInfo lastRoom = roomLayout.layout[randX, roomLayout.height - 1];

            while (true) {

                MoveDir nextMove = FindNextRoom(roomLayout, lastRoom);

                if (nextMove == MoveDir.End) {
                    roomLayout.layout[lastRoom.x, lastRoom.y].roomState = RoomInfo.RoomState.EndPoint;
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

                roomLayout.layout[dirX, dirY] = new RoomInfo(dirX, dirY);
                roomLayout.layout[dirX, dirY].roomState = RoomInfo.RoomState.MainPath;
                roomLayout.layout[dirX, dirY].roomStyle = RoomStyle.LeftRightOnly;

                if (nextMove == MoveDir.Down)
                    roomLayout.layout[dirX, dirY].roomStyle = RoomStyle.Bottom;

                if (lastRoom.roomStyle == RoomStyle.Bottom)
                {
                    RoomStyle randamType = (RoomStyle)Random.Range(2, 4);

                    roomLayout.layout[dirX, dirY].roomStyle = randamType;
                }


                lastRoom = roomLayout.layout[dirX, dirY];
            }

            return roomLayout;
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

        private MoveDir FindNextRoom(RoomLayout roomLayout, RoomInfo previousRoom) {
            //1,2 = Left, 3,4 = Right, 5 = Down
            int diceValue = Random.Range(1, 6);

            //Debug.Log(previousRoom.x +", " + previousRoom.y +", " + previousRoom.roomState.ToString("g"));

            if (diceValue == 1 || diceValue == 2)
            {
                if (!IsLeftRightAvailable(roomLayout, previousRoom, -1) &&
                    IsLeftRightAvailable(roomLayout, previousRoom, 1) )
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

        private bool IsLeftRightAvailable(RoomLayout roomLayout, RoomInfo previousRoom, int axis) {

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

        private void RenderRoom(RoomLayout roomLayout)
        {
            string renderString = "";
            for (int y = roomLayout.height - 1; y >= 0; y--) {

                for (int x = 0; x < roomLayout.width; x++)
                {
                    Debug.Log("x:" + x +", y:"+y+", "+roomLayout.layout[x, y].roomState.ToString("g"));

                    if (x == roomLayout.width - 1)
                        renderString += "\n";
                }
            }
        }

        public struct RoomLayout {
            public int width, height;
            public RoomInfo[,] layout;

            public RoomLayout(int width, int height) {
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
            }


        }

    }
}
