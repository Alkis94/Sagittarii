using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaveMapCreator : MapCreator
{
    private static CaveMapCreator instance = null;
    private Vector2Int bossRoomCoordinates;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

    }


    // Use this for initialization
    void Start()
    {
        mapLayout = new int[20, 40];
        mapRooms = new string[20, 40];
        unexploredRoomArrayCoordinates = new List<Vector2Int>();
        CreateMap();
        MapCreated(MapType.cave);
    }

    protected override void CreateMap()
    {
        mapLayout[10, 0] = (int)RoomType.normalRoom;
        mapLayout[10, 1] = (int)RoomType.verticalRoad;
        bossRoomCoordinates = CreatePathToBoss(15,new Vector2Int(10,2),false);
        CreateRandomPaths();
        CreateRandomSmallPaths();
        AssignRooms();
        AssignBossRoom();
        mapRooms[10, 0] = "CaveToForest";
    }

    private void AssignRooms()
    {
        bool north = false;
        bool south = false;
        bool east = false;
        bool west = false;

        for (int i = 0; i < mapLayout.GetLength(0); i++)
        {
            for (int j = 0; j < mapLayout.GetLength(1); j++)
            {
                //mapLayout[i, j] > 2 check means that inside the mapLayout there is a room, not a road or no room.
                if (mapLayout[i, j] > 2)
                {
                    FindConnectedRoadDirections(ref north,ref south,ref east,ref west, i, j);
                    mapRooms[i, j] = ReturnCorrectRoom(north, south, east, west, MapType.cave);
                    north = false;
                    south = false;
                    east = false;
                    west = false;
                }
            }
        }
    }

    private void AssignBossRoom()
    {
        bool north = false;
        bool south = false;
        bool east = false;
        bool west = false;
        FindConnectedRoadDirections(ref north, ref south, ref east, ref west, bossRoomCoordinates.x, bossRoomCoordinates.y);
        mapRooms[bossRoomCoordinates.x, bossRoomCoordinates.y] = ReturnCorrectBossRoom(north, south, east, west,MapType.cave);
    }

}
