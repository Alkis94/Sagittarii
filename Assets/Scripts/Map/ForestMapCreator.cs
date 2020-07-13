using UnityEngine;
using System.Collections.Generic;


public class ForestMapCreator : MapCreator
{
    private static ForestMapCreator instance = null;
    private const int Forest_Length = 20;

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
        base.Start();
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
        mapRooms[2, 0] = "ForestEntrance";
        CreatePathToBoss(Forest_Length, new Vector2Int(4, 0), false, false, false, true);

        int randomNumber;
        int numberOfForestRooms = RoomTracker.ForestRooms.Count;
        for (int i = 4; i < 40; i++)
        {
            if(mapLayout[i,0] == (int)RoomType.normalRoom)
            {
                randomNumber = Random.Range(0, numberOfForestRooms);
                mapRooms[i, 0] = RoomTracker.ForestRooms[randomNumber];
                normalRoomArrayCoordinates.Add(new Vector2Int(i, 0));
            }
            else if (mapLayout[i, 0] == (int)RoomType.bossRoom)
            {
                mapRooms[i, 0] = "ForestBossDoor";
            }
        }

        AddTreasures(2, normalRoomArrayCoordinates);
    }


}
