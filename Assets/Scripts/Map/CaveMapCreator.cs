using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaveMapCreator : MapCreator
{
    //private Vector2Int startingRoom = new Vector2Int(10, 0);


    private static CaveMapCreator instance = null;
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

    // Use this for initialization
    void Start()
    {
        mapLayout = new int[20, 40];
        mapRooms = new string[20, 40];
        unexploredRoomArrayCoordinates = new List<Vector2Int>();
        CreateMap();
        MapCreated(MapType.cave);
    }

    protected override void CreateMap()
    {
        mapLayout[10, 0] = (int)RoomType.exploredRoom;
        mapRooms[10, 0] = "CaveFirstRoom";
        mapLayout[10, 1] = (int)RoomType.verticalRoad;
        CreatePathToBoss(15,new Vector2Int(10,2),false);
        CreateRandomPaths();
        CreateRandomSmallPaths();
        AssignRooms();
    }

    private void AssignRooms()
    {
        bool north = false;
        bool south = false;
        bool east = false;
        bool west = false;

        for (int i = 0; i < mapLayout.GetLength(0); i++)
        {
            for (int j = 0; j < mapLayout.GetLength(1); j++)
            {
                if (mapLayout[i, j] > 2)
                {
                    if(j > 0)
                    {
                        if (mapLayout[i, j - 1] > 0)
                        {
                            north = true;
                        }
                    }
                    if(j < mapLayout.GetLength(1))
                    {
                        if (mapLayout[i, j + 1] > 0)
                        {
                            south = true;
                        }
                    }
                    if (i > 0)
                    {
                        if (mapLayout[i - 1, j] > 0)
                        {
                            west = true;
                        }
                    }
                    if (i < mapLayout.GetLength(0))
                    {
                        if (mapLayout[i + 1, j] > 0)
                        {
                            east = true;
                        }
                    }
                    mapRooms[i, j] = ReturnCorrectRoom(north, south, east, west);
                    north = false;
                    south = false;
                    east = false;
                    west = false;
                }
            }
        }

        mapRooms[10, 0] = "CaveFirstRoom";
    }

    private string ReturnCorrectRoom(bool north,bool south,bool east,bool west)
    {
        if(north && south && east && west)
        {
            return "CaveAllFour";
        }
        else if (north && south && !east && west)
        {
            return "CaveNoEast";
        }
        else if (north && south && east && !west)
        {
            return "CaveNoWest";
        }
        else if (!north && south && east && west)
        {
            return "CaveNoNorth";
        }
        else if (north && !south && east && west)
        {
            return "CaveNoSouth";
        }
        else if (north && !south && !east && !west)
        {
            return "CaveNorth";
        }
        else if (!north && south && !east && !west)
        {
            return "CaveSouth";
        }
        else if (!north && !south && !east && west)
        {
            return "CaveWest";
        }
        if (!north && !south && east && !west)
        {
            return "CaveEast";
        }
        else if (north && !south && east && !west)
        {
            return "CaveNorthEast";
        }
        else if (north && south && !east && !west)
        {
            return "CaveNorthSouth";
        }
        else if (north && !south && !east && west)
        {
            return "CaveNorthWest";
        }
        else if (!north && south && east && !west)
        {
            return "CaveSouthEast";
        }
        else if (!north && south && !east && west)
        {
            return "CaveSouthWest";
        }
        else if (!north && !south && east && west)
        {
            return "CaveEastWest";
        }
        else
        {
            return "NoRoomError";
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            
        }
    }
}
