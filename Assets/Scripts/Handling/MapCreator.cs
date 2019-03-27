using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapCreator : MonoBehaviour
{

    private const int distanceBetweenRooms = 20;
    private const int distanceBetweenRoomAndRoad = 10;

    private int[,] mapArray = new int[29, 15];

    private List<Vector2Int> unexploredRoomArrayCoordinates;

    private Vector2 DownLeftCorner;
    private Vector2 UpRightCorner;
    private Vector2 StartingRoom;
    [SerializeField]
    private GameObject map;

    

    void Start()
    {
        unexploredRoomArrayCoordinates = new List<Vector2Int>();
        DownLeftCorner = new Vector2(-145f, -75f);
        UpRightCorner = new Vector2(135f, 65f);
        StartingRoom = new Vector2(-5f, 5f);

        CreateMap();

        map.GetComponent<MapManager>().GetMap(mapArray);
        SceneManager.LoadScene(Random.Range(2, 7));
    }


    private void CreateMap()
    {
        mapArray[13, 7] = (int)RoomType.exploredRoom;
        CreatePathToBoss();
        CreatePathsToTreasure();
        CreateRandomSmallPaths();
    }

    private void CreateRoad(int roadLength,Vector2Int startCoordinates)
    {
        int i = 1;
        List<int> availableDirections = new List<int>();

        Vector2Int currentCoordinates = startCoordinates;
        mapArray[currentCoordinates.x, currentCoordinates.y] = (int)RoomType.unexploredRoom;


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
                mapArray[currentCoordinates.x - 1, currentCoordinates.y] = (int)RoomType.horizontalRoad;
                mapArray[currentCoordinates.x - 2, currentCoordinates.y] = (int)RoomType.unexploredRoom;
                currentCoordinates.x -= 2;
                unexploredRoomArrayCoordinates.Add(new Vector2Int(currentCoordinates.x, currentCoordinates.y));
                i++;
            }
            else if (randomDirection == (int)Direction.north)
            {
                mapArray[currentCoordinates.x, currentCoordinates.y + 1] = (int)RoomType.verticalRoad;
                mapArray[currentCoordinates.x, currentCoordinates.y + 2] = (int)RoomType.unexploredRoom;
                currentCoordinates.y += 2;
                unexploredRoomArrayCoordinates.Add(new Vector2Int(currentCoordinates.x, currentCoordinates.y));
                i++;
            }
            else if (randomDirection == (int)Direction.east)
            {
                mapArray[currentCoordinates.x + 1, currentCoordinates.y] = (int)RoomType.horizontalRoad;
                mapArray[currentCoordinates.x + 2, currentCoordinates.y] = (int)RoomType.unexploredRoom;
                currentCoordinates.x += 2;
                unexploredRoomArrayCoordinates.Add(new Vector2Int(currentCoordinates.x, currentCoordinates.y));
                i++;
            }
            else if (randomDirection == (int)Direction.south)
            {
                mapArray[currentCoordinates.x, currentCoordinates.y - 1] = (int)RoomType.verticalRoad;
                mapArray[currentCoordinates.x, currentCoordinates.y - 2] = (int)RoomType.unexploredRoom;
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

    private void CreatePathToBoss()
    {
        CreateRoad(15, new Vector2Int(13, 7));
        Vector2Int bossRoomCoordinates = unexploredRoomArrayCoordinates[unexploredRoomArrayCoordinates.Count - 1];
        unexploredRoomArrayCoordinates.RemoveAt(unexploredRoomArrayCoordinates.Count - 1);
        mapArray[bossRoomCoordinates.x, bossRoomCoordinates.y] = (int)RoomType.bossRoom;
    }

    private void CreatePathsToTreasure()
    {
        int randomAmountOfTreasureRoads = Random.Range(1, 4);
        int i = 0;
        while (i < randomAmountOfTreasureRoads)
        {
            int randomRoadLength = Random.Range(1, 8);
            int randomUnexploredRoomID = Random.Range(0, unexploredRoomArrayCoordinates.Count);
            Vector2Int randomUnexploredRoom = unexploredRoomArrayCoordinates[randomUnexploredRoomID];


            if (!HasNoSpaceAroundForRoom(randomUnexploredRoom))
            {
                CreateRoad(randomRoadLength, randomUnexploredRoom);
                Vector2Int treasureRoomCoordinates = unexploredRoomArrayCoordinates[unexploredRoomArrayCoordinates.Count - 1];
                unexploredRoomArrayCoordinates.RemoveAt(unexploredRoomArrayCoordinates.Count - 1);
                mapArray[treasureRoomCoordinates.x, treasureRoomCoordinates.y] = (int)RoomType.treasureRoom;
                i++;
            }

            
        }

        
    }


    private bool HasSpaceForRoomWest(Vector2Int room)
    {
        if (room.x - 2 >= 0)
        {
            if (mapArray[room.x - 2, room.y] == 0)
            {
                return true;
            }
        }
        return false;
    }

    private bool HasSpaceForRoomEast(Vector2Int room)
    {
        if (room.x + 2 < mapArray.GetLength(0))
        {
            if (mapArray[room.x + 2, room.y] == 0)
            {
                return true;
            }
        }
        return false;
    }

    private bool HasSpaceForRoomNorth(Vector2Int room)
    {
        if (room.y + 2 < mapArray.GetLength(1))
        {
            if (mapArray[room.x, room.y + 2] == 0)
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
            if (mapArray[room.x, room.y - 2] == 0)
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
}
