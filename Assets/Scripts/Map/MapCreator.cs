using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public abstract class MapCreator : MonoBehaviour
{

    private const int distanceBetweenRooms = 40;
    private const int distanceBetweenRoomAndRoad = 20;

    protected int[,] mapLayout;
    protected string[,] mapRooms;

    protected List<Vector2Int> unexploredRoomArrayCoordinates;
    protected abstract void CreateMap();
    public static event Action<int[,], string[,],MapType> OnMapCreated = delegate { };

    //[SerializeField]
    //protected List<GameObject> rooms;
    //[SerializeField]
    //protected Transform mapTransform;


    protected void CreatePath(int roadLength, Vector2Int startCoordinates, bool directRoadNorth = true, bool directRoadSouth = true, bool directRoadWest = true, bool directRoadEast = true)
    {
        int i = 1;
        List<int> availableDirections = new List<int>();

        Vector2Int currentCoordinates = startCoordinates;
        mapLayout[currentCoordinates.x, currentCoordinates.y] = (int)RoomType.unexploredRoom;


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
                mapLayout[currentCoordinates.x - 1, currentCoordinates.y] = (int)RoomType.horizontalRoad;
                mapLayout[currentCoordinates.x - 2, currentCoordinates.y] = (int)RoomType.unexploredRoom;
                currentCoordinates.x -= 2;
                unexploredRoomArrayCoordinates.Add(new Vector2Int(currentCoordinates.x, currentCoordinates.y));
                i++;
            }
            else if (randomDirection == (int)Direction.east)
            {
                mapLayout[currentCoordinates.x + 1, currentCoordinates.y] = (int)RoomType.horizontalRoad;
                mapLayout[currentCoordinates.x + 2, currentCoordinates.y] = (int)RoomType.unexploredRoom;
                currentCoordinates.x += 2;
                unexploredRoomArrayCoordinates.Add(new Vector2Int(currentCoordinates.x, currentCoordinates.y));
                i++;
            }
            else if (randomDirection == (int)Direction.north)
            {
                mapLayout[currentCoordinates.x, currentCoordinates.y - 1] = (int)RoomType.verticalRoad;
                mapLayout[currentCoordinates.x, currentCoordinates.y - 2] = (int)RoomType.unexploredRoom;
                currentCoordinates.y -= 2;
                unexploredRoomArrayCoordinates.Add(new Vector2Int(currentCoordinates.x, currentCoordinates.y));
                i++;
            }
            else if (randomDirection == (int)Direction.south)
            {
                mapLayout[currentCoordinates.x, currentCoordinates.y + 1] = (int)RoomType.verticalRoad;
                mapLayout[currentCoordinates.x, currentCoordinates.y + 2] = (int)RoomType.unexploredRoom;
                currentCoordinates.y += 2;
                unexploredRoomArrayCoordinates.Add(new Vector2Int(currentCoordinates.x, currentCoordinates.y));
                i++;
            }

            availableDirections.Clear();
        }
    }

    protected void CreateRandomSmallPaths()
    {
        int randomAmountOfSmallRoads = UnityEngine.Random.Range(5, 10);
        int i = 0;
        while ( i < randomAmountOfSmallRoads)
        {
            int randomRoadLength = UnityEngine.Random.Range(1, 8);
            int randomUnexploredRoomID = UnityEngine.Random.Range(0, unexploredRoomArrayCoordinates.Count);
            Vector2Int randomUnexploredRoom = unexploredRoomArrayCoordinates[randomUnexploredRoomID];

            if(HasSpaceAroundForRoom(randomUnexploredRoom))
            {
                CreatePath(randomRoadLength, randomUnexploredRoom);
                i++;
            }
            
        }
    }

    protected void CreatePathToBoss(int pathSize, Vector2Int pathStartingRoom, bool directRoadNorth = true, bool directRoadSouth = true, bool directRoadWest = true, bool directRoadEast = true)
    {
        CreatePath(pathSize, pathStartingRoom, directRoadNorth, directRoadSouth, directRoadWest, directRoadEast);
        Vector2Int bossRoomCoordinates = unexploredRoomArrayCoordinates[unexploredRoomArrayCoordinates.Count - 1];
        unexploredRoomArrayCoordinates.RemoveAt(unexploredRoomArrayCoordinates.Count - 1);
        mapLayout[bossRoomCoordinates.x, bossRoomCoordinates.y] = (int)RoomType.bossRoom;
    }

    protected void CreateRandomPaths()
    {
        int randomAmountOfPaths = UnityEngine.Random.Range(3, 5);
        int i = 0;
        while (i < randomAmountOfPaths)
        {
            int randomRoadLength = UnityEngine.Random.Range(1, 8);
            int randomUnexploredRoomID = UnityEngine.Random.Range(0, unexploredRoomArrayCoordinates.Count);
            Vector2Int randomUnexploredRoom = unexploredRoomArrayCoordinates[randomUnexploredRoomID];


            if (HasSpaceAroundForRoom(randomUnexploredRoom))
            {
                CreatePath(randomRoadLength, randomUnexploredRoom);
                i++;
            }
        }
    }

    //protected void RenderMap()
    //{
    //    for (int i = 0; i < mapLayout.GetLength(0); i++)
    //    {
    //        for (int j = 0; j < mapLayout.GetLength(1); j++)
    //        {
    //            if (mapLayout[i, j] != 0)
    //            {
    //                Vector2 mapCoordinates = ConvertArrayCoordinates(i, j);
    //                ExtensionMethods.InstantiateAtLocalPosition(rooms[mapLayout[i, j]], mapTransform, mapCoordinates);
    //            }
    //        }
    //    }
    //    //Debug.Log("Rendered Map");
    //}


    protected bool HasSpaceForRoomWest(Vector2Int room)
    {
        if (room.x - 2 >= 0)
        {
            if (mapLayout[room.x - 2, room.y] == 0)
            {
                return true;
            }
        }
        return false;
    }

    protected bool HasSpaceForRoomEast(Vector2Int room)
    {
        if (room.x + 2 < mapLayout.GetLength(0))
        {
            if (mapLayout[room.x + 2, room.y] == 0)
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
            if (mapLayout[room.x, room.y - 2] == 0)
            {
                return true;
            }
        }
        return false;
    }

    protected bool HasSpaceForRoomSouth(Vector2Int room)
    {
        if (room.y + 2 < mapLayout.GetLength(1))
        {
            if (mapLayout[room.x, room.y + 2] == 0)
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

    //protected void RenderMap(int offsetX = 0, int offsetY = 0)
    //{
    //    for (int i = 0; i < mapLayout.GetLength(0); i++)
    //    {
    //        for (int j = 0; j < mapLayout.GetLength(1); j++)
    //        {
    //            if (mapLayout[i, j] != 0)
    //            {
    //                Vector2 mapCoordinates = ConvertArrayCoordinates(i, j, offsetX, offsetY);
    //                ExtensionMethods.InstantiateAtLocalPosition(rooms[mapLayout[i, j]], mapTransform, mapCoordinates);
    //            }
    //        }
    //    }
    //    //Debug.Log("Rendered Map");
    //}

    protected void MapCreated(MapType mapType)
    {
        OnMapCreated.Invoke(mapLayout, mapRooms, mapType);
    }
}
