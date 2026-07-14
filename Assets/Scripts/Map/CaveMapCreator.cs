using UnityEngine;
using System.Collections.Generic;

public class CaveMapCreator : MapCreator
{
    public static CaveMapCreator Instance { get; private set; }

    public Vector2Int CaveFirstRoomCoordinates { get; private set; } = new(10, 4);
    public MapIcons CaveIcons { get; private set; } = new MapIcons();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        CaveIcons.LoadIcons("Cave");
    }

    public void CreateMap()
    {
        InitializeMapArray(20, 40);

        Map[10, 0].AddRoad(RoadType.Vertical);
        Map[10, 1].AddRoad(RoadType.Vertical);
        Map[10, 2].AddRoad(RoadType.Vertical);
        Map[10, 3].AddRoad(RoadType.Vertical);
        Map[10, 4].AddCustomRoom(new Room
        {
            Name = "CaveToForest",
            Type = RoomType.StartingRoom,
            Unexplored = false
        });
        Map[10, 5].AddRoad(RoadType.Vertical);

        var bossRoomCoordinates = CreatePathToBoss(new PathInfo(10, new(10, 6), false, true, true, true));
        Map[bossRoomCoordinates.x, bossRoomCoordinates.y].Room.Name = ReturnCorrectBossRoom(Map[bossRoomCoordinates.x, bossRoomCoordinates.y].Room.RoomOpenings, "Mushroom");
        CreateRandomPaths(6, 3);

        AssignRoomOpenings();
        AssignRooms();
        AddTreasures(3, normalRoomArrayCoordinates);
        AssignMushroomTowers();
    }

    private void AssignRooms()
    {
        for (var i = 0; i < Map.GetLength(0); i++)
        {
            for (var j = 0; j < Map.GetLength(1); j++)
            {
                if (Map[i, j].CellType == MapCellType.Room && Map[i, j].Room.Type == RoomType.NormalRoom)
                {
                    Map[i, j].Room.Name = AssignCorrectRoom(Map[i, j].Room.RoomOpenings);
                }
            }
        }
    }

    protected string AssignCorrectRoom(RoomOpenings openings)
    {
        if (openings == RoomOpenings.None)
        {
            return "";
        }

        var randomNumber = Random.Range(0, RoomTracker.CaveRoomLists[$"CaveRoom{openings}"].Count);
        return RoomTracker.CaveRoomLists[$"CaveRoom{openings}"][randomNumber];
    }

    private void AssignMushroomTowers()
    {
        var edgeRooms = new List<Room>();

        for (var i = 0; i < Map.GetLength(0); i++)
        {
            for (var j = 0; j < Map.GetLength(1); j++)
            {
                if (Map[i,j].CellType == MapCellType.Room && Map[i, j].Room.Type == RoomType.NormalRoom)
                {
                    if (Map[i, j].Room.RoomOpenings == RoomOpenings.N || Map[i, j].Room.RoomOpenings == RoomOpenings.S || 
                        Map[i, j].Room.RoomOpenings == RoomOpenings.W || Map[i, j].Room.RoomOpenings == RoomOpenings.E)
                    {
                        edgeRooms.Add(Map[i, j].Room);
                    }
                }
            }
        }

        var numberOfTowers = 3;
        if (ES3.KeyExists("MushroomsDestroyed", SaveManager.Instance.GetProfileRunPath() + SaveFolders.Bosses + "/MushroomBoss"))
        {
            var mushroomsDestroyed = ES3.Load<int>("MushroomsDestroyed", SaveManager.Instance.GetProfileRunPath() + SaveFolders.Bosses + "/MushroomBoss");
            numberOfTowers -= mushroomsDestroyed;
        }

        for (var i = 0; i < numberOfTowers; i++)
        {
            var randomNumber = Random.Range(0, edgeRooms.Count);

            edgeRooms[randomNumber].Name = edgeRooms[randomNumber].RoomOpenings switch
            {
                RoomOpenings.N => "MushroomTowerN",
                RoomOpenings.S => "MushroomTowerS",
                RoomOpenings.W => "MushroomTowerW",
                RoomOpenings.E => "MushroomTowerE",
                _ => throw new System.InvalidOperationException("Problem with assigning mushroom towers")
            };

            edgeRooms.RemoveAt(randomNumber);
        }
    }
}
