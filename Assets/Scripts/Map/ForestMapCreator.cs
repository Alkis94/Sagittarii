using UnityEngine;

public class ForestMapCreator : MapCreator
{
    private static ForestMapCreator instance = null;
    private const int Forest_Length = 15;

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
        map = new Room[40, 1];
        FillArrayWithRooms();
        numberOfTreasures = 2;
        CreateMap();
        MapCreated(MapType.forest);
    }

    protected override void CreateMap()
    {
        map[0, 0].RoomType = RoomType.horizontalRoad;
        map[1, 0].RoomType = RoomType.horizontalRoad;

        map[2, 0].RoomType = RoomType.normalRoom;
        map[2, 0].RoomName = "ForestEntrance";

        map[3, 0].RoomType = RoomType.horizontalRoad;

        CreatePathToBoss(Forest_Length, new Vector2Int(4, 0), false, false, false, true);

        int randomNumber;
        int numberOfForestRooms = RoomTracker.ForestRooms.Count;
        for (int i = 4; i < 40; i++)
        {
            if(map[i,0].RoomType == RoomType.normalRoom)
            {
                randomNumber = Random.Range(0, numberOfForestRooms);
                map[i, 0].RoomName = RoomTracker.ForestRooms[randomNumber];
                normalRoomArrayCoordinates.Add(new Vector2Int(i, 0));
            }
            else if (map[i, 0].RoomType == RoomType.bossRoom)
            {
                map[i, 0].RoomName = "ForestBossDoor";
            }
        }

        AddSpawnRoom();
        AddTreasures(2, normalRoomArrayCoordinates);
    }

    private void AddSpawnRoom ()
    {
        int randomNumber = Random.Range(0, normalRoomArrayCoordinates.Count);
        int randomNumber2 = Random.Range(0, RoomTracker.ForestSpawnRooms.Count);
        
        map[normalRoomArrayCoordinates[randomNumber].x, 0].RoomName = RoomTracker.ForestSpawnRooms[randomNumber2];
        map[normalRoomArrayCoordinates[randomNumber].x, 0].RoomType = RoomType.spawnRoom;
        //Debug.Log("SpawnRoom added at randomNumber" + normalRoomArrayCoordinates[randomNumber].x);
        normalRoomArrayCoordinates.RemoveAt(randomNumber);
    }
}
