using UnityEngine;

public class ForestMapCreator : MapCreator
{
    private static ForestMapCreator instance = null;
    private const int forestLength = 15;
    private readonly Vector2Int startRoomCoordinates = new(4, 0);

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

    protected void Start()
    {
        map = new Room[40, 1];
        FillArrayWithRooms();
        TreasureCount = 2;
        CreateMap();
        MapCreated(MapType.forest);
    }

    private void CreateMap()
    {
        map[0, 0].RoomType = RoomType.HorizontalRoad;
        map[1, 0].RoomType = RoomType.HorizontalRoad;
        map[2, 0].RoomType = RoomType.StartingRoom;
        map[2, 0].RoomName = "ForestEntrance";
        map[3, 0].RoomType = RoomType.HorizontalRoad;

        CreatePathToBoss(new PathInfo(forestLength, startRoomCoordinates, false, false, false, true));
        AssignRoomOpenings();

        int randomNumber;
        int numberOfForestRooms = RoomTracker.ForestRooms.Count;
        for (int i = 0; i < 40; i++)
        {
            if(map[i,0].RoomType == RoomType.NormalRoom)
            {
                randomNumber = Random.Range(0, numberOfForestRooms);
                map[i, 0].RoomName = RoomTracker.ForestRooms[randomNumber];
                normalRoomArrayCoordinates.Add(new Vector2Int(i, 0));
            }
            else if (map[i, 0].RoomType == RoomType.BossRoom)
            {
                map[i, 0].RoomName = "BearBossDoor";
            }
        }

        //AddSpawnRoom();
        AddTreasures(2, normalRoomArrayCoordinates);
    }

    private void AddSpawnRoom ()
    {
        int randomNumber = Random.Range(0, normalRoomArrayCoordinates.Count);
        int randomNumber2 = Random.Range(0, RoomTracker.ForestSpawnRooms.Count);
        
        map[normalRoomArrayCoordinates[randomNumber].x, 0].RoomName = RoomTracker.ForestSpawnRooms[randomNumber2];
        map[normalRoomArrayCoordinates[randomNumber].x, 0].RoomType = RoomType.SpawnRoom;
        normalRoomArrayCoordinates.RemoveAt(randomNumber);
    }
}
