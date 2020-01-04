using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaveMapCreator : MapCreator
{
    private Vector2Int startingRoom = new Vector2Int(10, 0);



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
        mapLayout[10, 0] = (int)RoomType.verticalRoad;
        CreatePathToBoss(15,new Vector2Int(10,1),false);
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
                    if(mapLayout[i,j - 1] > 0)
                    {
                        north = true;
                    }
                    if (mapLayout[i, j + 1] > 0)
                    {
                        south = true;
                    }
                    if (mapLayout[i - 1, j] > 0)
                    {
                        west = true;
                    }
                    if(mapLayout[i + 1, j] > 0)
                    {
                        east = true;
                    }

                    mapRooms[i, j] = ReturnCorrectRoom(north, south, east, west);
                }
            }
        }
    }

    private string ReturnCorrectRoom(bool north,bool south,bool east,bool west)
    {
        if(north && south && east && west)
        {
            return "TestCaveAllFour";
        }
        else if (north && south && !east && west)
        {
            return "TestCaveNoEast";
        }
        else if (north && south && east && !west)
        {
            return "TestCaveNoWest";
        }
        else if (!north && south && east && west)
        {
            return "TestCaveNoNorth";
        }
        else if (north && !south && east && west)
        {
            return "TestCaveNoSouth";
        }
        else if (north && !south && !east && !west)
        {
            return "TestCaveNorth";
        }
        else if (!north && south && !east && !west)
        {
            return "TestCaveSouth";
        }
        else if (!north && !south && !east && west)
        {
            return "TestCaveWest";
        }
        if (!north && !south && east && !west)
        {
            return "TestCaveEast";
        }
        else if (north && !south && east && !west)
        {
            return "TestCaveNorthEast";
        }
        else if (north && south && !east && !west)
        {
            return "TestCaveNorthSouth";
        }
        else if (north && !south && !east && west)
        {
            return "TestCaveNorthWest";
        }
        else if (!north && south && east && !west)
        {
            return "TestCaveSouthEast";
        }
        else if (!north && south && !east && west)
        {
            return "TestCaveSouthWest";
        }
        else if (!north && !south && east && west)
        {
            return "TestCaveEastWest";
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
