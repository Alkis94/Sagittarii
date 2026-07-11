using UnityEngine;

public class ForestMapCreator : MapCreator
{
    public static ForestMapCreator Instance { get; private set; }

    public Vector2Int ForestFirstRoomCoordinates { get; private set; } = new(2, 0);
    public MapIcons ForestIcons { get; private set; }

    private const int forestLength = 10;
    private readonly Vector2Int startRoomCoordinates = new(4, 0);

    public ForestMapCreator()
    {
        if (Instance != null)
        {
            return;
        }

        Instance = this;
    }

    public void CreateMap()
    {
        ForestIcons = new MapIcons("Forest");
        InitializeMapArray(40, 1);
        TreasureCount = 2;

        Map[0, 0].AddRoad(RoadType.Horizontal);
        Map[1, 0].AddRoad(RoadType.Horizontal);
        Map[2, 0].AddNormallRoom();
        Map[2, 0].AddCustomRoom(new Room
        {
            Name = "ForestEntrance"
        });
        Map[3, 0].AddRoad(RoadType.Horizontal);

        CreatePathToBoss(new PathInfo(forestLength, startRoomCoordinates, false, false, false, true));
        AssignRoomOpenings();

        for (int i = 0; i < 40; i++)
        {
            if (Map[i, 0].Room == null)
            {
                continue;
            }

            if(Map[i,0].Room.Type == RoomType.NormalRoom)
            {
                var randomNumber = Random.Range(0, RoomTracker.ForestRooms.Count);
                Map[i, 0].Room.Name = RoomTracker.ForestRooms[randomNumber];
                normalRoomArrayCoordinates.Add(new Vector2Int(i, 0));
            }
            else if (Map[i, 0].Room.Type == RoomType.BossRoom)
            {
                Map[i, 0].Room.Name = "BearBossDoor";
            }
        }

        AddTreasures(2, normalRoomArrayCoordinates);
    }
}
