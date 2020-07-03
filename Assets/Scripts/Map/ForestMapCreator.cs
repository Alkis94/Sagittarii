using UnityEngine;
using System.Collections.Generic;


public class ForestMapCreator : MapCreator
{

    private static ForestMapCreator instance = null;
    private const int Forest_Length = 3;


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

    protected override void Start()
    {
        mapRooms = new string[40, 1];
        mapLayout = new int[40, 1];
        numberOfTreasures = 2;
        CreateMap();
        MapCreated(MapType.forest);
    }

    protected override void CreateMap()
    {
        mapLayout[0, 0] = (int)RoomType.horizontalRoad;
        mapLayout[1, 0] = (int)RoomType.horizontalRoad;
        mapLayout[2, 0] = (int)RoomType.normalRoom;
        mapLayout[3, 0] = (int)RoomType.horizontalRoad;
        mapLayout[4, 0] = (int)RoomType.normalRoom;
        mapLayout[5, 0] = (int)RoomType.horizontalRoad;
        mapRooms[2, 0] = "ForestEntranceRoom";
        mapRooms[4, 0] = "ForestToCave";
        CreatePathToBoss(Forest_Length, new Vector2Int(6, 0), false, false, false, true);

        int randomNumber;
        int numberOfForestRooms = RoomTracker.ForestRooms.Count;
        roomArrayCoordinates.Clear();
        for (int i = 6; i < 40; i++)
        {
            if(mapLayout[i,0] == (int)RoomType.normalRoom)
            {
                randomNumber = Random.Range(0, numberOfForestRooms);
                mapRooms[i, 0] = RoomTracker.ForestRooms[randomNumber];
                roomArrayCoordinates.Add(new Vector2Int(i, 0));
            }
            else if(mapLayout[i, 0] == (int)RoomType.bossRoom)
            {
                mapRooms[i, 0] = "ForestBossDoor";
            }
        }

        for(int i = 0; i < 2; i++)
        {
            randomNumber = Random.Range(0, roomArrayCoordinates.Count);
            roomsWithTreasure[i] = roomArrayCoordinates[randomNumber];
        }
    }
            

}
