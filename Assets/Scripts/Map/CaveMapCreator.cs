using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaveMapCreator : MapCreator
{
    private static CaveMapCreator instance = null;
    private Vector2Int bossRoomCoordinates;
    private List<List<string>> caveRooms;

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
        caveRooms = new List<List<string>>();
    }

    protected override void Start()
    {
        base.Start();
        map = new Room[20, 40];
        FillArrayWithRooms();
        AddCaveMapLists();
        CreateMap();
        MapCreated(MapType.cave);
    }

    protected override void CreateMap()
    {
        map[10, 0].RoomType = RoomType.normalRoom;
        map[10, 1].RoomType = RoomType.verticalRoad;
        bossRoomCoordinates = CreatePathToBoss(8,new Vector2Int(10,2),false);
        CreateRandomPaths(3,5);
        AssignRooms();
        AssignBossRoom();
        AddTreasures(3, normalRoomArrayCoordinates);

        map[10, 0].RoomName = "CaveToForest";
    }

    private void AssignRooms()
    {
        bool north = false;
        bool south = false;
        bool east = false;
        bool west = false;

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                //mapLayout[i, j] > 2 check means that inside the map there is a room, not a road or no room.
                if ((int)map[i, j].RoomType > 2)
                {
                    FindConnectedRoadDirections(ref north,ref south,ref east,ref west, i, j);
                    int openings = ReturnCorrectRoomOpenings(north, south, east, west);
                    map[i, j].RoomName = AssignCorrectRoom(openings);
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
        map[bossRoomCoordinates.x, bossRoomCoordinates.y].RoomName = ReturnCorrectBossRoom(north, south, east, west, "Mushroom");
    }

    private void AddCaveMapLists()
    {
        caveRooms.Add(RoomTracker.CaveRoomsNSWE);
        caveRooms.Add(RoomTracker.CaveRoomsNSW);
        caveRooms.Add(RoomTracker.CaveRoomsNSE);
        caveRooms.Add(RoomTracker.CaveRoomsNWE);
        caveRooms.Add(RoomTracker.CaveRoomsSWE);
        caveRooms.Add(RoomTracker.CaveRoomsNS);
        caveRooms.Add(RoomTracker.CaveRoomsNW);
        caveRooms.Add(RoomTracker.CaveRoomsNE);
        caveRooms.Add(RoomTracker.CaveRoomsSW);
        caveRooms.Add(RoomTracker.CaveRoomsSE);
        caveRooms.Add(RoomTracker.CaveRoomsWE);
        caveRooms.Add(RoomTracker.CaveRoomsN);
        caveRooms.Add(RoomTracker.CaveRoomsS);
        caveRooms.Add(RoomTracker.CaveRoomsW);
        caveRooms.Add(RoomTracker.CaveRoomsE);
    }

    private string AssignCorrectRoom(int openings)
    {
        int randomNumber = Random.Range(0, caveRooms[openings].Count);
        return caveRooms[openings][randomNumber];
    }
}
