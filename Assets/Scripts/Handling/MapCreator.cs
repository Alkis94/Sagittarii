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

        CreateRoadToBoss();
        RenderMap();

       //Instantiate(UnexploredRoom, StartingRoom, Quaternion.identity, MapBackground);
       //ExtensionMethods.InstantiateAtLocalPosition(UnexploredRoom,MapBackground, StartingRoom);
    }


    private void CreateRoadToBoss()
    {
        int i = 0;
        Vector2Int currentCoordinates = new Vector2Int(13, 7);
        map[currentCoordinates.x, currentCoordinates.y] = (int)RoomType.unexploredRoom;


        while (i < 15)
        {
            int randomDirection = Random.Range(0, 4);

            if (randomDirection == (int)Direction.west && currentCoordinates.x - 2 >= 0)
            {
                if (map[currentCoordinates.x - 2, currentCoordinates.y] == 0)
                {
                    map[currentCoordinates.x - 1, currentCoordinates.y] = (int)RoomType.horizontalRoad;
                    map[currentCoordinates.x - 2, currentCoordinates.y] = (int)RoomType.unexploredRoom;
                    currentCoordinates.x -= 2;
                    unexploredRoomArrayCoordinates.Add(new Vector2Int(currentCoordinates.x, currentCoordinates.y));
                    i++;
                }
            }
            else if (randomDirection == (int)Direction.north && currentCoordinates.y + 2 < map.GetLength(1))
            {
                if (map[currentCoordinates.x, currentCoordinates.y + 2] == 0)
                {
                    map[currentCoordinates.x, currentCoordinates.y + 1] = (int)RoomType.verticalRoad;
                    map[currentCoordinates.x, currentCoordinates.y + 2] = (int)RoomType.unexploredRoom;
                    currentCoordinates.y += 2;
                    unexploredRoomArrayCoordinates.Add(new Vector2Int(currentCoordinates.x, currentCoordinates.y));
                    i++;
                }
            }
            else if (randomDirection == (int)Direction.east && currentCoordinates.x + 2 < map.GetLength(0))
            {
                if (map[currentCoordinates.x + 2, currentCoordinates.y] == 0)
                {
                    map[currentCoordinates.x + 1, currentCoordinates.y] = (int)RoomType.horizontalRoad;
                    map[currentCoordinates.x + 2, currentCoordinates.y] = (int)RoomType.unexploredRoom;
                    currentCoordinates.x += 2;
                    unexploredRoomArrayCoordinates.Add(new Vector2Int(currentCoordinates.x, currentCoordinates.y));
                    i++;
                }
            }
            else if (randomDirection == (int)Direction.south && currentCoordinates.y - 2 >= 0)
            {
                if (map[currentCoordinates.x, currentCoordinates.y - 2] == 0)
                {
                    map[currentCoordinates.x, currentCoordinates.y - 1] = (int)RoomType.verticalRoad;
                    map[currentCoordinates.x, currentCoordinates.y - 2] = (int)RoomType.unexploredRoom;
                    currentCoordinates.y -= 2;
                    unexploredRoomArrayCoordinates.Add(new Vector2Int(currentCoordinates.x, currentCoordinates.y));
                    i++;
                }
            }
        }

    }

    private void CreateRandomSmallPaths()
    {
        int randomAmountOfSmallRoads = Random.Range(3, 6);
        int randomUnexploredRoomID = Random.Range(0, unexploredRoomArrayCoordinates.Count);
        Vector2Int randomUnexploredRoom = unexploredRoomArrayCoordinates[randomUnexploredRoomID];



    }

    //private Vector2Int GetUnexploredRoomToAttachNewRoad()
    //{
        
    //    if(randomUnexploredRoom.x - 2 >= 0)
    //    {
    //        if(map[randomUnexploredRoom.x - 2, randomUnexploredRoom.y] == 0)
    //        {

    //        }
    //    }
    //}

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
