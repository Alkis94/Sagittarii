using System.Collections.Generic;
using UnityEngine;

public abstract class MapCreator : MonoBehaviour
{
    public MapCell[,] Map { get; private set; }
    protected int TreasureCount = 0;

    protected List<Vector2Int> normalRoomArrayCoordinates = new();

    protected void CreatePath(PathInfo pathInfo)
    {
        var currentCoordinates = pathInfo.StartCoordinates;
        Map[currentCoordinates.x, currentCoordinates.y].AddNormallRoom();

        for (int i = 1; i < pathInfo.Length; i++)
        {
            var availableDirections = new List<Direction>();

            if (pathInfo.AllowWest && HasSpaceForRoom(currentCoordinates, Direction.West))
            {
                availableDirections.Add(Direction.West);
            }
            if (pathInfo.AllowEast && HasSpaceForRoom(currentCoordinates, Direction.East))
            {
                availableDirections.Add(Direction.East);
            }
            if (pathInfo.AllowNorth && HasSpaceForRoom(currentCoordinates, Direction.North))
            {
                availableDirections.Add(Direction.North);
            }
            if (pathInfo.AllowSouth && HasSpaceForRoom(currentCoordinates, Direction.South))
            {
                availableDirections.Add(Direction.South);
            }

            if (availableDirections.Count == 0)
            {
                break;
            }

            var randomDirection = availableDirections[Random.Range(0, availableDirections.Count)];

            switch (randomDirection)
            {
                case Direction.West:
                    Map[currentCoordinates.x - 1, currentCoordinates.y].AddRoad(RoadType.Horizontal);
                    Map[currentCoordinates.x - 2, currentCoordinates.y].AddNormallRoom();
                    currentCoordinates.x -= 2;
                    normalRoomArrayCoordinates.Add(new Vector2Int(currentCoordinates.x, currentCoordinates.y));
                    break;
                case Direction.East:
                    Map[currentCoordinates.x + 1, currentCoordinates.y].AddRoad(RoadType.Horizontal);
                    Map[currentCoordinates.x + 2, currentCoordinates.y].AddNormallRoom();
                    currentCoordinates.x += 2;
                    normalRoomArrayCoordinates.Add(new Vector2Int(currentCoordinates.x, currentCoordinates.y));
                    break;
                case Direction.North:
                    Map[currentCoordinates.x, currentCoordinates.y - 1].AddRoad(RoadType.Vertical);
                    Map[currentCoordinates.x, currentCoordinates.y - 2].AddNormallRoom();
                    currentCoordinates.y -= 2;
                    normalRoomArrayCoordinates.Add(new Vector2Int(currentCoordinates.x, currentCoordinates.y));
                    break;
                case Direction.South:
                    Map[currentCoordinates.x, currentCoordinates.y + 1].AddRoad(RoadType.Vertical);
                    Map[currentCoordinates.x, currentCoordinates.y + 2].AddNormallRoom();
                    currentCoordinates.y += 2;
                    normalRoomArrayCoordinates.Add(new Vector2Int(currentCoordinates.x, currentCoordinates.y));
                    break;
            }
        }
    }

    protected void CreateRandomPaths(int pathCount, int averagePathLength)
    {
        var uncheckedNormalRooms = new List<int>();

        for (var j = 0; j < normalRoomArrayCoordinates.Count; j++)
        {
            uncheckedNormalRooms.Add(j);
        }

        var minPathLength = averagePathLength - averagePathLength / 2;
        var maxPathLength = averagePathLength + averagePathLength / 2;

        for (var i = 0; i < pathCount; i++)
        {
            if (uncheckedNormalRooms.Count <= 0)
            {
                break;
            }

            var randomRoadLength = Random.Range(minPathLength, maxPathLength);
            var randomUncheckedRoomID = Random.Range(0, uncheckedNormalRooms.Count);
            var chosenRoomID = uncheckedNormalRooms[randomUncheckedRoomID];
            uncheckedNormalRooms.RemoveAt(randomUncheckedRoomID);
            var randomNormalRoom = normalRoomArrayCoordinates[chosenRoomID];

            if (HasSpaceAnyAroundForRoom(randomNormalRoom))
            {
                var pathParams = new PathInfo(randomRoadLength, randomNormalRoom);
                CreatePath(pathParams);
            }
        }
    }

    protected Vector2Int CreatePathToBoss(PathInfo pathInfo)
    {
        CreatePath(pathInfo);
        var bossRoomCoordinates = normalRoomArrayCoordinates[normalRoomArrayCoordinates.Count - 1];
        normalRoomArrayCoordinates.RemoveAt(normalRoomArrayCoordinates.Count - 1);
        Map[bossRoomCoordinates.x, bossRoomCoordinates.y].AddBossRoom();
        return bossRoomCoordinates;
    }

    protected void AddTreasures(int TreasuresAmount, List<Vector2Int> normalRoomArrayCoordinates)
    {
        for (var i = 0; i < TreasuresAmount; i++)
        {
            var randomNumber = Random.Range(0, normalRoomArrayCoordinates.Count);
            Map[normalRoomArrayCoordinates[randomNumber].x, normalRoomArrayCoordinates[randomNumber].y].Room.HasTreasure = true;
        }
    }

    protected RoomOpenings ReturnCorrectRoomOpening(int coordinatesX, int coordinatesY)
    {
        var north = coordinatesY - 1 > 0 && Map[coordinatesX, coordinatesY - 1].CellType == MapCellType.Road;
        var south = coordinatesY + 1 < Map.GetLength(1) && Map[coordinatesX, coordinatesY + 1].CellType == MapCellType.Road;
        var west = coordinatesX - 1 > 0 && Map[coordinatesX - 1, coordinatesY].CellType == MapCellType.Road;
        var east = coordinatesX + 1 < Map.GetLength(0) && Map[coordinatesX + 1, coordinatesY].CellType == MapCellType.Road;

        return (north, south, east, west) switch
        {
            (true, true, true, true) => RoomOpenings.NSWE,
            (true, true, false, true) => RoomOpenings.NSW,
            (true, true, true, false) => RoomOpenings.NSE,
            (true, false, true, true) => RoomOpenings.NWE,
            (false, true, true, true) => RoomOpenings.SWE,
            (true, true, false, false) => RoomOpenings.NS,
            (true, false, false, true) => RoomOpenings.NW,
            (true, false, true, false) => RoomOpenings.NE,
            (false, true, false, true) => RoomOpenings.SW,
            (false, true, true, false) => RoomOpenings.SE,
            (false, false, true, true) => RoomOpenings.WE,
            (true, false, false, false) => RoomOpenings.N,
            (false, true, false, false) => RoomOpenings.S,
            (false, false, false, true) => RoomOpenings.W,
            (false, false, true, false) => RoomOpenings.E,
            _ => RoomOpenings.E
        };
    }

    /// <summary>
    ///  Should be called after creating map layout has 
    ///  finished to determine the room openings.
    /// </summary>
    protected void AssignRoomOpenings()
    {
        for (var i = 0; i < Map.GetLength(0); i++)
        {
            for (var j = 0; j < Map.GetLength(1); j++)
            {
                if (Map[i, j].CellType == MapCellType.Room)
                {
                    Map[i, j].Room.RoomOpenings = ReturnCorrectRoomOpening(i, j);
                }
            }
        }
    }

    protected string ReturnCorrectBossRoom(RoomOpenings roomOpenings, string roomPrefix)
    {
        return roomOpenings switch
        {
            RoomOpenings.N => roomPrefix + "BossDoorN",
            RoomOpenings.S => roomPrefix + "BossDoorS",
            RoomOpenings.E => roomPrefix + "BossDoorE",
            RoomOpenings.W => roomPrefix + "BossDoorW",
            _ => "Error boss room not found!"
        };
    }

    protected bool HasSpaceForRoom(Vector2Int room, Direction direction)
    {
        return direction switch
        {
            Direction.West => room.x - 2 >= 0 && Map[room.x - 2, room.y].CellType == MapCellType.None,
            Direction.East => room.x + 2 < Map.GetLength(0) && Map[room.x + 2, room.y].CellType == MapCellType.None,
            Direction.North => room.y - 2 >= 0 && Map[room.x, room.y - 2].CellType == MapCellType.None,
            Direction.South => room.y + 2 < Map.GetLength(1) && Map[room.x, room.y + 2].CellType == MapCellType.None,
            _ => false
        };
    }

    protected bool HasSpaceAnyAroundForRoom(Vector2Int room)
    {
        return HasSpaceForRoom(room, Direction.West) || HasSpaceForRoom(room, Direction.East) ||
               HasSpaceForRoom(room, Direction.North) || HasSpaceForRoom(room, Direction.South);
    }

    protected Vector2 ConvertArrayCoordinates(int x, int y, int offsetX = 0, int offsetY = 0)
    {
        return new Vector2(offsetX - 145 + (x + 1) * 20, offsetY + 75 - (y + 1) * 20);
    }

    protected void InitializeMapArray(int rows, int columns)
    {
        Map = new MapCell[rows, columns];

        for (var i = 0; i < Map.GetLength(0); i++)
        {
            for (var j = 0; j < Map.GetLength(1); j++)
            {
                Map[i, j] = new MapCell();
            }
        }
    }
}

public struct PathInfo
{
    public int Length;
    public Vector2Int StartCoordinates;
    public bool AllowNorth;
    public bool AllowSouth;
    public bool AllowWest;
    public bool AllowEast;

    public PathInfo(int length, Vector2Int startCoordinates, bool allowNorth = true, bool allowSouth = true, bool allowWest = true, bool allowEast = true)
    {
        Length = length;
        StartCoordinates = startCoordinates;
        AllowNorth = allowNorth;
        AllowSouth = allowSouth;
        AllowWest = allowWest;
        AllowEast = allowEast;
    }
}
