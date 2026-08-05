using UnityEngine;

public class ForestMapCreator : MapCreator
{
    public static ForestMapCreator Instance { get; private set; }
    public Vector2Int ForestFirstRoomCoordinates { get; private set; } = new(2, 0);

    private const int forestLength = 20;

    private void Awake()
    {
        if (this.ShouldBecomeSingletonInstance(Instance))
        {
            Instance = this;
        }
    }

    private void Start()
    {
        Icons.LoadIcons("Forest");
    }

    public void CreateMap()
    {
        InitializeMapArray(forestLength * 2, 1);
        TreasureCount = 2;

        Map[0, 0].AddRoad(RoadType.Horizontal);
        Map[1, 0].AddRoad(RoadType.Horizontal);
        Map[2, 0].AddCustomRoom(new Room
        {
            Name = "ForestEntrance",
            Type = RoomType.StartingRoom,
            Unexplored = false,
        });
        Map[3, 0].AddRoad(RoadType.Horizontal);

        CreatePathToBoss(new PathInfo(forestLength, new Vector2Int(4, 0), false, false, false, true));
        AssignRoomOpenings();

        for (int i = 0; i < forestLength * 2; i++)
        {
            if (Map[i, 0].Room == null)
            {
                continue;
            }

            if (Map[i,0].Room.Type == RoomType.NormalRoom)
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

        AddTreasures(TreasureCount);

        SaveMap(MapNames.Forest);
    }
}
