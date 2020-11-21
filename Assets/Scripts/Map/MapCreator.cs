using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class MapCreator : MonoBehaviour
{

    private const int distanceBetweenRooms = 40;
    private const int distanceBetweenRoomAndRoad = 20;

    protected Room[,] map;
    [SerializeField]
    protected int numberOfTreasures = 0;

    protected List<Vector2Int> normalRoomArrayCoordinates;
    public static event Action<Room[,],MapType> OnMapCreated = delegate { };

    protected virtual void Start()
    {
        normalRoomArrayCoordinates = new List<Vector2Int>();
    }

    protected void CreatePath(int roadLength, Vector2Int startCoordinates, bool directRoadNorth = true, bool directRoadSouth = true, bool directRoadWest = true, bool directRoadEast = true)
    {
        int i = 1;
        List<int> availableDirections = new List<int>();

        Vector2Int currentCoordinates = startCoordinates;
        map[currentCoordinates.x, currentCoordinates.y].RoomType = RoomType.normalRoom;

        while (i < roadLength)
        {
            if (HasSpaceForRoomWest(currentCoordinates) && directRoadWest)
            {
                availableDirections.Add((int)Direction.west);
            }
            if (HasSpaceForRoomEast(currentCoordinates) && directRoadEast)
            {
                availableDirections.Add((int)Direction.east);
            }
            if (HasSpaceForRoomNorth(currentCoordinates) && directRoadNorth)
            {
                availableDirections.Add((int)Direction.north);
            }
            if (HasSpaceForRoomSouth(currentCoordinates) && directRoadSouth)
            {
                availableDirections.Add((int)Direction.south);
            }

            if (availableDirections.Count == 0)
            {
                break;
            }

            int randomDirection = UnityEngine.Random.Range(0, availableDirections.Count);
            randomDirection = availableDirections[randomDirection];

            if (randomDirection == (int)Direction.west)
            {
                map[currentCoordinates.x - 1, currentCoordinates.y].RoomType = RoomType.horizontalRoad;
                map[currentCoordinates.x - 2, currentCoordinates.y].RoomType = RoomType.normalRoom;
                currentCoordinates.x -= 2;
                normalRoomArrayCoordinates.Add(new Vector2Int(currentCoordinates.x, currentCoordinates.y));
                i++;
            }
            else if (randomDirection == (int)Direction.east)
            {
                map[currentCoordinates.x + 1, currentCoordinates.y].RoomType = RoomType.horizontalRoad;
                map[currentCoordinates.x + 2, currentCoordinates.y].RoomType = RoomType.normalRoom;
                currentCoordinates.x += 2;
                normalRoomArrayCoordinates.Add(new Vector2Int(currentCoordinates.x, currentCoordinates.y));
                i++;
            }
            else if (randomDirection == (int)Direction.north)
            {
                map[currentCoordinates.x, currentCoordinates.y - 1].RoomType = RoomType.verticalRoad;
                map[currentCoordinates.x, currentCoordinates.y - 2].RoomType = RoomType.normalRoom;
                currentCoordinates.y -= 2;
                normalRoomArrayCoordinates.Add(new Vector2Int(currentCoordinates.x, currentCoordinates.y));
                i++;
            }
            else if (randomDirection == (int)Direction.south)
            {
                map[currentCoordinates.x, currentCoordinates.y + 1].RoomType = RoomType.verticalRoad;
                map[currentCoordinates.x, currentCoordinates.y + 2].RoomType = RoomType.normalRoom;
                currentCoordinates.y += 2;
                normalRoomArrayCoordinates.Add(new Vector2Int(currentCoordinates.x, currentCoordinates.y));
                i++;
            }

            availableDirections.Clear();
        }
    }

    protected void CreateRandomPaths(int numberOfPaths, int averagePathLength)
    {
        List<int> uncheckedNormalRooms = new List<int>();
        for(int j = 0; j < normalRoomArrayCoordinates.Count; j++)
        {
            uncheckedNormalRooms.Add(j);
        }

        int i = 0;
        int minPathLength = averagePathLength - averagePathLength / 2;
        int maxPathLength = averagePathLength + averagePathLength / 2;
        while ( i < numberOfPaths)
        {
            if (uncheckedNormalRooms.Count <= 0)
            {
                Debug.LogError(i + " paths created instead of " + numberOfPaths);
                break;
            }

            int randomRoadLength = UnityEngine.Random.Range(minPathLength, maxPathLength);
            int randomUncheckedRoomID = UnityEngine.Random.Range(0, uncheckedNormalRooms.Count);
            int chosenRoomID = uncheckedNormalRooms[randomUncheckedRoomID];
            uncheckedNormalRooms.RemoveAt(randomUncheckedRoomID);
            Vector2Int randomNormalRoom = normalRoomArrayCoordinates[chosenRoomID];

            if(HasSpaceAroundForRoom(randomNormalRoom))
            {
                CreatePath(randomRoadLength, randomNormalRoom);
                i++;
            }
        }
    }

    protected Vector2Int CreatePathToBoss(int pathSize, Vector2Int pathStartingRoom, bool directRoadNorth = true, bool directRoadSouth = true, bool directRoadWest = true, bool directRoadEast = true)
    {
        CreatePath(pathSize, pathStartingRoom, directRoadNorth, directRoadSouth, directRoadWest, directRoadEast);
        Vector2Int bossRoomCoordinates = normalRoomArrayCoordinates[normalRoomArrayCoordinates.Count - 1];
        normalRoomArrayCoordinates.RemoveAt(normalRoomArrayCoordinates.Count - 1);
        map[bossRoomCoordinates.x, bossRoomCoordinates.y].RoomType = RoomType.bossRoom;
        return bossRoomCoordinates;
    }

    protected void AddTreasures(int TreasuresAmount, List<Vector2Int> normalRoomArrayCoordinates)
    {
        for (int i = 0; i < TreasuresAmount; i++)
        {
            int randomNumber = UnityEngine.Random.Range(0, normalRoomArrayCoordinates.Count);
            map[normalRoomArrayCoordinates[randomNumber].x, normalRoomArrayCoordinates[randomNumber].y].HasTreasure = true;
        }
    }

    protected RoomOpenings ReturnCorrectRoomOpening(int coordinatesX, int coordinatesY)
    {
        bool north = false, south = false, east = false, west = false;

        if (coordinatesY - 1 > 0)
        {
            if (map[coordinatesX, coordinatesY - 1].RoomType > 0)
            {
                north = true;
            }
        }
        if (coordinatesY + 1 < map.GetLength(1))
        {
            if (map[coordinatesX, coordinatesY + 1].RoomType > 0)
            {
                south = true;
            }
        }
        if (coordinatesX - 1 > 0)
        {
            if (map[coordinatesX - 1, coordinatesY].RoomType > 0)
            {
                west = true;
            }
        }
        if (coordinatesX + 1 < map.GetLength(0))
        {
            if (map[coordinatesX + 1, coordinatesY].RoomType > 0)
            {
                east = true;
            }
        }

        if (north && south && east && west)
        {
            return RoomOpenings.NSWE;
        }
        else if (north && south && !east && west)
        {
            return RoomOpenings.NSW;
        }
        else if (north && south && east && !west)
        {
            return RoomOpenings.NSE;
        }
        else if (north && !south && east && west)
        {
            return RoomOpenings.NWE;
        }
        else if (!north && south && east && west)
        {
            return RoomOpenings.SWE;
        }
        else if (north && south && !east && !west)
        {
            return RoomOpenings.NS;
        }
        else if (north && !south && !east && west)
        {
            return RoomOpenings.NW;
        }
        else if (north && !south && east && !west)
        {
            return RoomOpenings.NE;
        }
        else if (!north && south && !east && west)
        {
            return RoomOpenings.SW;
        }
        else if (!north && south && east && !west)
        {
            return RoomOpenings.SE;
        }
        else if (!north && !south && east && west)
        {
            return RoomOpenings.WE;
        }
        else if (north && !south && !east && !west)
        {
            return RoomOpenings.N;
        }
        else if (!north && south && !east && !west)
        {
            return RoomOpenings.S;
        }
        else if (!north && !south && !east && west)
        {
            return RoomOpenings.W;
        }
        if (!north && !south && east && !west)
        {
            return RoomOpenings.E;
        }
        else
        {
            Debug.LogError("ReturnCorrectRoom from MapCreator failed!");
            return RoomOpenings.E;
        }
    }


    /// <summary>
    ///  Should be called after creating map layout has 
    ///  finished to determine the room openings.
    /// </summary>
    protected void AssignRoomOpenings()
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                //mapLayout[i, j] > 2 check means that inside the map there is a room, not a road or no room.
                if ((int)map[i, j].RoomType > 2)
                {
                    map[i, j].RoomOpenings = ReturnCorrectRoomOpening(i, j);
                }
            }
        }
    }

    protected string ReturnCorrectBossRoom(RoomOpenings roomOpenings, string roomPrefix)
    {
        if (roomOpenings == RoomOpenings.N)
        {
            return roomPrefix + "BossDoorN";
        }
        else if (roomOpenings == RoomOpenings.S)
        {
            return roomPrefix + "BossDoorS";
        }
        else if (roomOpenings == RoomOpenings.E)
        {
            return roomPrefix + "BossDoorE";
        }
        else if (roomOpenings == RoomOpenings.W)
        {
            return roomPrefix + "BossDoorW";
        }
        else
        {
            return "Error boss room not found!";
        }
    }

    protected bool HasSpaceForRoomWest(Vector2Int room)
    {
        if (room.x - 2 >= 0)
        {
            if (map[room.x - 2, room.y].RoomType == RoomType.noRoom)
            {
                return true;
            }
        }
        return false;
    }

    protected bool HasSpaceForRoomEast(Vector2Int room)
    {
        if (room.x + 2 < map.GetLength(0))
        {
            if (map[room.x + 2, room.y].RoomType == RoomType.noRoom)
            {
                return true;
            }
        }
        return false;
    }

    protected bool HasSpaceForRoomNorth(Vector2Int room)
    {
        if (room.y - 2 >= 0)
        {
            if (map[room.x, room.y - 2].RoomType == RoomType.noRoom)
            {
                return true;
            }
        }
        return false;
    }

    protected bool HasSpaceForRoomSouth(Vector2Int room)
    {
        if (room.y + 2 < map.GetLength(1))
        {
            if (map[room.x, room.y + 2].RoomType == RoomType.noRoom)
            {
                return true;
            }
        }
        return false;
    }

    protected bool HasSpaceAroundForRoom(Vector2Int room)
    {
        if (!HasSpaceForRoomWest(room) && !HasSpaceForRoomEast(room) &&
            !HasSpaceForRoomNorth(room) && !HasSpaceForRoomSouth(room))
        {
            return false;
        }
        return true;
    }

    protected Vector2 ConvertArrayCoordinates(int x, int y, int offsetX = 0, int offsetY = 0)
    {
        Vector2 mapCoordinates = new Vector2(offsetX - 145 + (x + 1) * 20, offsetY + 75 - (y + 1) * 20);
        return mapCoordinates;
    }

    protected void FillArrayWithRooms()
    {
        for(int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                map[i, j] = new Room();
                map[i, j].RoomArrayCoords = new Vector2(i, j);
            }
        }
    }

    protected string AssignCorrectRoom(int openings, List<List<string>> roomLevels)
    {
        int randomNumber = UnityEngine.Random.Range(0, roomLevels[openings].Count);
        return roomLevels[openings][randomNumber];
    }

    protected void MapCreated(MapType mapType)
    {
        OnMapCreated?.Invoke(map, mapType);
    }
}
