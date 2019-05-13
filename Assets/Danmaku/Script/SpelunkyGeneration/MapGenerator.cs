using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG.SpelunkyMap {
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField]
        Vector2Int size;

        public enum RoomType {
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

        RoomLayout Generate(int width, int height) {
            RoomLayout roomLayout = new RoomLayout(width, height);

            roomLayout = GenerateMainPath(roomLayout);

            return roomLayout;
        }

        RoomLayout GenerateMainPath(RoomLayout roomLayout) {
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

                int dirX = 0, dirY = 0;

                if (nextMove == MoveDir.Down)
                {
                    dirY = lastRoom.y -1;
                }

                if (nextMove == MoveDir.Left || nextMove == MoveDir.Right) {
                    dirX = lastRoom.x + ((nextMove == MoveDir.Left) ? -1 : 1);
                }

                roomLayout.layout[dirX, dirY] = new RoomInfo(lastRoom.x, lastRoom.y - 1);
                roomLayout.layout[dirX, dirY].roomState = RoomInfo.RoomState.MainPath;
                roomLayout.layout[dirX, dirY].roomType = RoomType.LeftRightOnly;

                if (nextMove == MoveDir.Down)
                    roomLayout.layout[dirX, dirY].roomType = RoomType.Bottom;

                if (lastRoom.roomType == RoomType.Bottom)
                {
                    RoomType randamType = (RoomType)Random.Range(2, 4);

                    roomLayout.layout[dirX, dirY].roomType = randamType;
                }


                lastRoom = roomLayout.layout[dirX, dirY];
            }

            return roomLayout;
        }

        RoomInfo CreateRoom(int x, int y, RoomInfo.RoomState roomState, RoomType roomType) {
            var roomInfo = new RoomInfo(x, y);
            roomInfo.roomState = roomState;
            roomInfo.roomType = roomType;
            return roomInfo;
        }

        RoomInfo CreateDownSlotRoom(RoomInfo previousRoom) {
            if (previousRoom.y == 0)
            {
                previousRoom.roomState = RoomInfo.RoomState.EndPoint;
                return previousRoom;
            }
            else
            {
                return CreateRoom(previousRoom.x, previousRoom.y - 1, RoomInfo.RoomState.MainPath, RoomType.Up);
            }
        }

        RoomType[,] GenerateOtherRoom(RoomType[,] mainPathLayout) {
            return mainPathLayout;
        }

        MoveDir FindNextRoom(RoomLayout roomLayout, RoomInfo previousRoom) {
            //1,2 = Left, 3,4 = Right, 5 = Down
            int diceValue = Random.Range(1, 6);

            if (diceValue == 1 || diceValue == 2)
            {
                if (previousRoom.x == 0)
                {
                    return CheckDownValidity(previousRoom);
                }
                Debug.Log(previousRoom.x +" , " + previousRoom.y +", " +previousRoom.roomState.ToString("g"));
                if (!roomLayout.layout[previousRoom.x - 1, previousRoom.y].Equals(default(RoomInfo)) &&
                    roomLayout.layout[previousRoom.x + 1, previousRoom.y].Equals(default(RoomInfo)))
                {
                    return MoveDir.Right;
                }
                else if (!roomLayout.layout[previousRoom.x - 1, previousRoom.y].Equals(default(RoomInfo)) &&
                  !roomLayout.layout[previousRoom.x + 1, previousRoom.y].Equals(default(RoomInfo)))
                {
                    return CheckDownValidity(previousRoom);
                }

                return MoveDir.Left;
            }

            if (diceValue == 3 || diceValue == 4)
            {
                if (previousRoom.x == roomLayout.width - 1)
                {
                     return CheckDownValidity(previousRoom);
                }
                Debug.Log(previousRoom.x + " , " + previousRoom.y + ", " + previousRoom.roomState.ToString("g"));

                if (!roomLayout.layout[previousRoom.x + 1, previousRoom.y].Equals(default(RoomInfo)) &&
                    roomLayout.layout[previousRoom.x - 1, previousRoom.y].Equals(default(RoomInfo)))
                {
                    return MoveDir.Left;
                }
                else if (!roomLayout.layout[previousRoom.x + 1, previousRoom.y].Equals(default(RoomInfo)) &&
                  !roomLayout.layout[previousRoom.x - 1, previousRoom.y].Equals(default(RoomInfo))) {
                    return CheckDownValidity(previousRoom);
                }

                return MoveDir.Right;
            }

            return CheckDownValidity(previousRoom);
        }

        private MoveDir CheckDownValidity(RoomInfo previousRoom) {
            if (previousRoom.y == 0)
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
            }
        }

        public struct RoomInfo {
            public int x, y;
            public RoomType roomType;
            public RoomState roomState;

            public enum RoomState {
                Undefined,
                StartPoint,
                EndPoint,
                MainPath,
                Other,
            }

            public RoomInfo(int x , int y) {
                this.x = x;
                this.y = y;
                roomType = RoomType.FreeStyle;
                roomState = RoomState.Undefined;
            }


        }

    }
}
