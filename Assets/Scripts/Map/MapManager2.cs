using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MapManager2 : MonoBehaviour
{

    private static MapManager2 instance = null;


    private int[,] mapLayout; 
    private string[,] mapRooms;
    private int[,] forestMapLayout = new int[40, 1];
    private string[,] forestMapRooms = new string[40, 1];
    private int[,] caveMapLayout = new int[20, 40];
    private string[,] caveMapRooms = new string[20, 40];

    private Vector2Int currentMapCoords = new Vector2Int(-1, 0);
    [SerializeField]
    private List<GameObject> rooms;
    [SerializeField]
    private Transform mapTransform;

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

    private void OnEnable()
    {
        RoomChanger.OnRoomChangerEntered += ChangeRoom;
        MapChanger.OnMapChangerEntered += ChangeMap;
        MapCreator.OnMapCreated += GetMap;
    }

    private void OnDisable()
    {
        RoomChanger.OnRoomChangerEntered -= ChangeRoom;
        MapChanger.OnMapChangerEntered -= ChangeMap;
        MapCreator.OnMapCreated -= GetMap;
    }

    // Use this for initialization
    void Start()
    {

    }

    private void ChangeMap(MapType currentMap,MapType nextMap)
    {
        if (currentMap == MapType.town && nextMap == MapType.forest)
        {
            mapLayout = forestMapLayout;
            mapRooms = forestMapRooms;
            currentMapCoords = Vector2Int.zero;
            SceneManager.LoadScene(mapRooms[0, 0]);
            OnRoomChangeRenderMapPart();
        }
        else if(currentMap == MapType.forest && nextMap == MapType.town)
        {
            currentMapCoords = new Vector2Int(-1, 0);
            SceneManager.LoadScene("Town");
        }
        else if (currentMap == MapType.forest && nextMap == MapType.cave)
        {
            mapLayout = caveMapLayout;
            mapRooms = caveMapRooms;
            currentMapCoords = Vector2Int.zero;
            Debug.Log("Scene name: " +mapRooms[10, 1]);
            SceneManager.LoadScene(mapRooms[10, 1]);
            OnRoomChangeRenderMapPart();
        }
    }

    private void ChangeRoom(Direction doorPlacement)
    {
        
        Vector2Int previousMapCoords = currentMapCoords;
        if (doorPlacement == Direction.west)
        {
            currentMapCoords = new Vector2Int(currentMapCoords.x - 2, currentMapCoords.y);
        }
        else if (doorPlacement == Direction.east)
        {
            currentMapCoords = new Vector2Int(currentMapCoords.x + 2, currentMapCoords.y);
        }
        else if (doorPlacement == Direction.north)
        {
            currentMapCoords = new Vector2Int(currentMapCoords.x, currentMapCoords.y + 2);
        }
        else
        {
            currentMapCoords = new Vector2Int(currentMapCoords.x, currentMapCoords.y - 2);
        }

        if (mapRooms[currentMapCoords.x,currentMapCoords.y] != null)
        {
            SceneManager.LoadScene(mapRooms[currentMapCoords.x,currentMapCoords.y]);
            OnRoomChangeRenderMapPart();
        }
        else
        {
            currentMapCoords = previousMapCoords; //if we didn't change room for some reason we revert to old current map coords
        }
    }

    public void GetMap(int[,] mapLayout, string[,] mapRooms,MapType mapType)
    {
        if(mapType == MapType.forest)
        {
            Array.Copy(mapLayout, 0, forestMapLayout, 0, mapLayout.Length);
            Array.Copy(mapRooms, 0, forestMapRooms, 0, mapRooms.Length);
        }
        else if (mapType == MapType.cave)
        {
            Array.Copy(mapLayout, 0, caveMapLayout, 0, mapLayout.Length);
            Array.Copy(mapRooms, 0, caveMapRooms, 0, mapRooms.Length);
            Debug.Log("Cave Map Copied");
        }


    }

    private void OnRoomChangeRenderMapPart()
    {
        Vector2 mapCoordinates;
        bool west = true, east = true, north = true, south = true;

        if(currentMapCoords.x <= 0)
        {
            west = false;
        }
        if (currentMapCoords.x >= mapLayout.GetLength(0) - 1)
        {
            east = false;
        }
        if (currentMapCoords.y <= 0)
        {
            north = false;
        }
        if (currentMapCoords.y >= mapLayout.GetLength(1) - 1)
        {
            south = false;
        }

        //Check west if road and render
        if(west)
        {
            if (mapLayout[currentMapCoords.x - 1, currentMapCoords.y] != 0)
            {
                //Render room
                mapCoordinates = ConvertArrayCoordinates(currentMapCoords.x - 2, currentMapCoords.y);
                ExtensionMethods.InstantiateAtLocalPosition(rooms[mapLayout[currentMapCoords.x - 2, currentMapCoords.y]], mapTransform, mapCoordinates);
                //Render road
                mapCoordinates = ConvertArrayCoordinates(currentMapCoords.x - 1, currentMapCoords.y);
                ExtensionMethods.InstantiateAtLocalPosition(rooms[mapLayout[currentMapCoords.x - 1, currentMapCoords.y]], mapTransform, mapCoordinates);

                //we zero them so we don't render them again!
                mapLayout[currentMapCoords.x - 2, currentMapCoords.y] = 0;
                mapLayout[currentMapCoords.x - 1, currentMapCoords.y] = 0;
            }
        }
        
        //Check east if road and render
        if(east)
        {
            if (mapLayout[currentMapCoords.x + 1, currentMapCoords.y] != 0)
            {
                mapCoordinates = ConvertArrayCoordinates(currentMapCoords.x + 2, currentMapCoords.y);
                ExtensionMethods.InstantiateAtLocalPosition(rooms[mapLayout[currentMapCoords.x + 2, currentMapCoords.y]], mapTransform, mapCoordinates);

                mapCoordinates = ConvertArrayCoordinates(currentMapCoords.x + 1, currentMapCoords.y);
                ExtensionMethods.InstantiateAtLocalPosition(rooms[mapLayout[currentMapCoords.x + 1, currentMapCoords.y]], mapTransform, mapCoordinates);

                //we zero them so we don't render them again!
                mapLayout[currentMapCoords.x + 2, currentMapCoords.y] = 0;
                mapLayout[currentMapCoords.x + 1, currentMapCoords.y] = 0;
            }
        }
        
        //Check north if road and render
        if(north)
        {
            if (mapLayout[currentMapCoords.x, currentMapCoords.y - 1] != 0)
            {
                mapCoordinates = ConvertArrayCoordinates(currentMapCoords.x, currentMapCoords.y + 2);
                ExtensionMethods.InstantiateAtLocalPosition(rooms[mapLayout[currentMapCoords.x, currentMapCoords.y + 2]], mapTransform, mapCoordinates);

                mapCoordinates = ConvertArrayCoordinates(currentMapCoords.x, currentMapCoords.y + 1);
                ExtensionMethods.InstantiateAtLocalPosition(rooms[mapLayout[currentMapCoords.x, currentMapCoords.y + 1]], mapTransform, mapCoordinates);

                //we zero them so we don't render them again!
                mapLayout[currentMapCoords.x, currentMapCoords.y + 2] = 0;
                mapLayout[currentMapCoords.x, currentMapCoords.y + 1] = 0;
            }
        }

        //Check south if road and render
        if(south)
        {
            if (mapLayout[currentMapCoords.x, currentMapCoords.y + 1] != 0)
            {
                mapCoordinates = ConvertArrayCoordinates(currentMapCoords.x, currentMapCoords.y - 2);
                ExtensionMethods.InstantiateAtLocalPosition(rooms[mapLayout[currentMapCoords.x, currentMapCoords.y - 2]], mapTransform, mapCoordinates);

                mapCoordinates = ConvertArrayCoordinates(currentMapCoords.x, currentMapCoords.y - 1);
                ExtensionMethods.InstantiateAtLocalPosition(rooms[mapLayout[currentMapCoords.x, currentMapCoords.y - 1]], mapTransform, mapCoordinates);

                //we zero them so we don't render them again!
                mapLayout[currentMapCoords.x, currentMapCoords.y - 2] = 0;
                mapLayout[currentMapCoords.x, currentMapCoords.y - 1] = 0;
            }
        }
        
    }

    private Vector2 ConvertArrayCoordinates(int x, int y)
    {
        Vector2 mapCoordinates = new Vector2(-145 + (x + 1) * 20, -75 + (y + 1) * 20);
        return mapCoordinates;
    }

}
