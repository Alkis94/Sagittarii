using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaveMapCreator : MapCreator
{
    private static CaveMapCreator instance = null;
    private readonly Vector2Int startRoomCoordinates = new Vector2Int(10, 2);
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

    private void CreateMap()
    {
        //Creating map layout
        map[10, 0].RoomType = RoomType.startingRoom;
        map[10, 1].RoomType = RoomType.verticalRoad;
        bossRoomCoordinates = CreatePathToBoss(8, startRoomCoordinates, false);
        CreateRandomPaths(6, 3);

        //Assigning rooms-levels.
        AssignRoomOpenings();
        AssignRooms();
        AddTreasures(3, normalRoomArrayCoordinates);
        AssignMushroomTowers();
        map[10, 0].RoomName = "CaveToForest";

    }

    private void AssignRooms()
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                //mapLayout[i, j] > 2 check means that inside the map there is a room, not a road or no room.
                if ((int)map[i, j].RoomType > 2)
                {
                    map[i, j].RoomName = AssignCorrectRoom((int)map[i, j].RoomOpenings, caveRooms);
                }
            }
        }

        map[bossRoomCoordinates.x, bossRoomCoordinates.y].RoomName = ReturnCorrectBossRoom(map[bossRoomCoordinates.x, bossRoomCoordinates.y].RoomOpenings, "Mushroom");
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

    private void AssignMushroomTowers()
    {

        List<Room> edgeRooms = new List<Room>();

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j].RoomType == RoomType.normalRoom)
                {
                    if (map[i, j].RoomOpenings == RoomOpenings.N || map[i, j].RoomOpenings == RoomOpenings.S || map[i, j].RoomOpenings == RoomOpenings.W || map[i, j].RoomOpenings == RoomOpenings.E)
                    {
                        edgeRooms.Add(map[i, j]);
                    }
                }
            }
        }

        int numberOfTowers = 3;
        if (ES3.KeyExists("MushroomsDestroyed", "Saves/Profile" + SaveProfile.SaveID + "/Bosses/MushroomBoss"))
        {
            int mushroomsDestroyed = ES3.Load<int>("MushroomsDestroyed", "Saves/Profile" + SaveProfile.SaveID + "/Bosses/MushroomBoss");
            numberOfTowers -= mushroomsDestroyed;
        }

        for (int i = 0; i < numberOfTowers; i++)
        {
            int randomNumber = Random.Range(0, edgeRooms.Count);

            if (edgeRooms[randomNumber].RoomOpenings == RoomOpenings.N)
            {
                edgeRooms[randomNumber].RoomName = "MushroomTowerN";
            }
            else if (edgeRooms[randomNumber].RoomOpenings == RoomOpenings.S)
            {
                edgeRooms[randomNumber].RoomName = "MushroomTowerS";
            }
            else if (edgeRooms[randomNumber].RoomOpenings == RoomOpenings.W)
            {
                edgeRooms[randomNumber].RoomName = "MushroomTowerW";
            }
            else if (edgeRooms[randomNumber].RoomOpenings == RoomOpenings.E)
            {
                edgeRooms[randomNumber].RoomName = "MushroomTowerE";
            }
            else
            {
                Debug.LogError("Problem with assigning mushroom towers");
            }

            edgeRooms.RemoveAt(randomNumber);
        }
    }

}
