using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{

    private const int distanceBetweenRooms = 20;
    private const int distanceBetweenRoomAndRoad = 10;

    private int[,] map = new int[29, 15];
    private List<Vector2Int> unexploredRoomArrayCoordinates;

    private Vector2 DownLeftCorner;
    private Vector2 UpRightCorner;
    private Vector2 StartingRoom;
    private RectTransform MapBackground;

    [SerializeField]
    private GameObject exploredRoom;
    [SerializeField]
    private GameObject unexploredRoom;
    [SerializeField]
    private GameObject horizontalRoad;
    [SerializeField]
    private GameObject verticalRoad;

    void Start()
    {
        unexploredRoomArrayCoordinates = new List<Vector2Int>();
        MapBackground = GetComponent<RectTransform>();
        DownLeftCorner = new Vector2(-145f, -75f);
        UpRightCorner = new Vector2(135f, 65f);
        StartingRoom = new Vector2(-5f, 5f);

        CreateRoad(15,new Vector2Int(13,7));
        CreateRandomSmallPaths();
        RenderMap();
    }


    private void CreateRoad(int roadLength,Vector2Int startCoordinates)
    {
        int i = 1;
        List<int> availableDirections = new List<int>();

        Vector2Int currentCoordinates = startCoordinates;
        map[currentCoordinates.x, currentCoordinates.y] = (int)RoomType.unexploredRoom;


        while (i < roadLength)
        {
            if(HasSpaceForRoomWest(currentCoordinates))
            {
                availableDirections.Add((int)Direction.west);
            }
            if (HasSpaceForRoomEast(currentCoordinates))
            {
                availableDirections.Add((int)Direction.east);
            }
            if (HasSpaceForRoomNorth(currentCoordinates))
            {
                availableDirections.Add((int)Direction.north);
            }
            if (HasSpaceForRoomSouth(currentCoordinates))
            {
                availableDirections.Add((int)Direction.south);
            }

            if(availableDirections.Count == 0)
            {
                break;
            }

            int randomDirection = Random.Range(0, availableDirections.Count);
            randomDirection = availableDirections[randomDirection];

            if (randomDirection == (int)Direction.west)
            {
                map[currentCoordinates.x - 1, currentCoordinates.y] = (int)RoomType.horizontalRoad;
                map[currentCoordinates.x - 2, currentCoordinates.y] = (int)RoomType.unexploredRoom;
                currentCoordinates.x -= 2;
                unexploredRoomArrayCoordinates.Add(new Vector2Int(currentCoordinates.x, currentCoordinates.y));
                i++;
            }
            else if (randomDirection == (int)Direction.north)
            {
                map[currentCoordinates.x, currentCoordinates.y + 1] = (int)RoomType.verticalRoad;
                map[currentCoordinates.x, currentCoordinates.y + 2] = (int)RoomType.unexploredRoom;
                currentCoordinates.y += 2;
                unexploredRoomArrayCoordinates.Add(new Vector2Int(currentCoordinates.x, currentCoordinates.y));
                i++;
            }
            else if (randomDirection == (int)Direction.east)
            {
                map[currentCoordinates.x + 1, currentCoordinates.y] = (int)RoomType.horizontalRoad;
                map[currentCoordinates.x + 2, currentCoordinates.y] = (int)RoomType.unexploredRoom;
                currentCoordinates.x += 2;
                unexploredRoomArrayCoordinates.Add(new Vector2Int(currentCoordinates.x, currentCoordinates.y));
                i++;
            }
            else if (randomDirection == (int)Direction.south)
            {
                map[currentCoordinates.x, currentCoordinates.y - 1] = (int)RoomType.verticalRoad;
                map[currentCoordinates.x, currentCoordinates.y - 2] = (int)RoomType.unexploredRoom;
                currentCoordinates.y -= 2;
                unexploredRoomArrayCoordinates.Add(new Vector2Int(currentCoordinates.x, currentCoordinates.y));
                i++;
            }

            availableDirections.Clear();
        }

    }

    private void CreateRandomSmallPaths()
    {
        int randomAmountOfSmallRoads = Random.Range(5, 10);
        int i = 0;
        while ( i < randomAmountOfSmallRoads)
        {
            int randomRoadLength = Random.Range(1, 8);
            int randomUnexploredRoomID = Random.Range(0, unexploredRoomArrayCoordinates.Count);
            Vector2Int randomUnexploredRoom = unexploredRoomArrayCoordinates[randomUnexploredRoomID];

            if(!HasNoSpaceAroundForRoom(randomUnexploredRoom))
            {
                CreateRoad(randomRoadLength, randomUnexploredRoom);
                i++;
            }
            
        }
    }


    private bool HasSpaceForRoomWest(Vector2Int room)
    {
        if (room.x - 2 >= 0)
        {
            if (map[room.x - 2, room.y] == 0)
            {
                return true;
            }
        }
        return false;
    }

    private bool HasSpaceForRoomEast(Vector2Int room)
    {
        if (room.x + 2 < map.GetLength(0))
        {
            if (map[room.x + 2, room.y] == 0)
            {
                return true;
            }
        }
        return false;
    }

    private bool HasSpaceForRoomNorth(Vector2Int room)
    {
        if (room.y + 2 < map.GetLength(1))
        {
            if (map[room.x, room.y + 2] == 0)
            {
                return true;
            }
        }
        return false;
    }

    private bool HasSpaceForRoomSouth(Vector2Int room)
    {
        if (room.y - 2 >= 0)
        {
            if (map[room.x, room.y - 2] == 0)
            {
                return true;
            }
        }
        return false;
    }

    private bool HasNoSpaceAroundForRoom(Vector2Int room)
    {
        if (!HasSpaceForRoomWest(room) && !HasSpaceForRoomEast(room) &&
                    !HasSpaceForRoomNorth(room) && !HasSpaceForRoomSouth(room))
        {
            return true;
        }
        return false;
    }

    private void RenderMap()
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] != 0)
                {
                    if (map[i, j] == (int)RoomType.exploredRoom)
                    {
                        Vector2 mapCoordinates = ConvertArrayCoordinates(i, j);
                        ExtensionMethods.InstantiateAtLocalPosition(exploredRoom, MapBackground, mapCoordinates);
                    }
                    else if (map[i, j] == (int)RoomType.unexploredRoom)
                    {
                        Vector2 mapCoordinates = ConvertArrayCoordinates(i, j);
                        ExtensionMethods.InstantiateAtLocalPosition(unexploredRoom, MapBackground, mapCoordinates);
                    }
                    else if (map[i, j] == (int)RoomType.horizontalRoad)
                    {
                        Vector2 mapCoordinates = ConvertArrayCoordinates(i, j);
                        ExtensionMethods.InstantiateAtLocalPosition(horizontalRoad, MapBackground, mapCoordinates);
                    }
                    else if (map[i, j] == (int)RoomType.verticalRoad)
                    {
                        Vector2 mapCoordinates = ConvertArrayCoordinates(i, j);
                        ExtensionMethods.InstantiateAtLocalPosition(verticalRoad, MapBackground, mapCoordinates);
                    }
                }
            }
        }
    }

    private Vector2 ConvertArrayCoordinates(int x,int y)
    {
        Vector2 mapCoordinates = new Vector2(-145 + (x + 1) * 10, -75 + (y + 1) * 10);
        return mapCoordinates;
    }

    public enum RoomType
    {
        exploredRoom = 1,
        unexploredRoom,
        horizontalRoad,
        verticalRoad
    }

    public enum Direction
    {
        west,
        north,
        east,
        south
    }
}
