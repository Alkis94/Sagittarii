using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{

    private static MapManager instance = null;

    private bool caveMapExists = false;
    private bool forestMapExists = false;
    private bool currentPlayerLocationInitialized = false;



    private int[,] mapLayout;
    private string[,] mapRooms;
    private int[,] forestMapLayout = new int[40, 1];
    private string[,] forestMapRooms = new string[40, 1];
    private int[,] caveMapLayout = new int[20, 40];
    private string[,] caveMapRooms = new string[20, 40];

    private readonly Vector2Int forestFirstRoomCoordinates = new Vector2Int(2, 0);
    private readonly Vector2Int forestCaveDoorRoomCoordinates = new Vector2Int(4, 0);
    private readonly Vector2Int caveFirstRoomCoordinates = new Vector2Int(10, 0);

    private Vector2Int currentMapCoords = new Vector2Int(-1, 0);
    [SerializeField]
    private List<GameObject> rooms;
    [SerializeField]
    private Transform mapTransform;
    [SerializeField]
    private GameObject playerCurrentMapLocationPrefab;
    [SerializeField]
    private GameObject townIcon;
    private MapType currentMap = MapType.town;
    private GameObject playerCurrentMapLocation;

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
        SceneManager.sceneLoaded += OnSceneLoaded;
        RoomChanger.OnRoomChangerEntered += ChangeRoom;
        MapChanger.OnMapChangerEntered += ChangeMap;
        MapCreator.OnMapCreated += GetMap;
        Door.DoorEntered += OnDoorEntered;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        RoomChanger.OnRoomChangerEntered -= ChangeRoom;
        MapChanger.OnMapChangerEntered -= ChangeMap;
        MapCreator.OnMapCreated -= GetMap;
        Door.DoorEntered += OnDoorEntered;
    }

    private void Start()
    {
        ExtensionMethods.InstantiateAtLocalPosition(townIcon, mapTransform, Vector2Int.zero);
    }

    private void ChangeMap(MapType currentMap, MapType nextMap)
    {

        if (nextMap == MapType.town)
        {
            this.currentMap = MapType.town;
            SceneManager.LoadScene("Town");
            ResetMap();
            if(ES3.DirectoryExists(("Levels/")))
            {
                Debug.Log("Should Delete Folder");
                ES3.DeleteDirectory("Levels/");
                ES3.DeleteDirectory("Levels/");
            }
        }
        else if (currentMap == MapType.town && nextMap == MapType.forest)
        {
            this.currentMap = MapType.forest;
            mapLayout = forestMapLayout;
            mapRooms = forestMapRooms;
            currentMapCoords = forestFirstRoomCoordinates;

            SceneManager.LoadScene(mapRooms[forestFirstRoomCoordinates.x, forestFirstRoomCoordinates.y]);
            OnRoomChangeRenderMapPart2(Direction.east);
            MoveCurrentPlayerPositionAndCenterMap();

            //We put an extra road to connect forest and town, and avoid having a room that collides on the town.
            Vector3 mapCoordinates = ConvertArrayCoordinates(0, 0);
            ExtensionMethods.InstantiateAtLocalPosition(rooms[(int)RoomType.horizontalRoad], mapTransform, mapCoordinates);

            RenderMap();
        }
        else if (currentMap == MapType.forest && nextMap == MapType.cave)
        {
            this.currentMap = MapType.cave;
            mapLayout = caveMapLayout;
            mapRooms = caveMapRooms;
            currentMapCoords = caveFirstRoomCoordinates;

            //We put this road to connect forest and caves. This roads is not inside mapLayout. We do it this way so
            //no rooms of caves collide with forrest rooms.
            Vector2 mapCoordinates = new Vector2(120, -20);
            ExtensionMethods.InstantiateAtLocalPosition(rooms[(int)RoomType.verticalRoad], mapTransform, mapCoordinates);

            SceneManager.LoadScene(mapRooms[caveFirstRoomCoordinates.x, caveFirstRoomCoordinates.y]);
            OnRoomChangeRenderMapPart2(Direction.south);
            MoveCurrentPlayerPositionAndCenterMap();

            RenderMap();
        }
        else if (currentMap == MapType.cave && nextMap == MapType.forest)
        {
            this.currentMap = MapType.forest;
            mapLayout = forestMapLayout;
            mapRooms = forestMapRooms;
            currentMapCoords = forestCaveDoorRoomCoordinates;

            SceneManager.LoadScene(mapRooms[forestCaveDoorRoomCoordinates.x, forestCaveDoorRoomCoordinates.y]);
            MoveCurrentPlayerPositionAndCenterMap();
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
            currentMapCoords = new Vector2Int(currentMapCoords.x, currentMapCoords.y - 2);
        }
        else if (doorPlacement == Direction.south)
        {
            currentMapCoords = new Vector2Int(currentMapCoords.x, currentMapCoords.y + 2);
        }
      
        if (mapRooms[currentMapCoords.x, currentMapCoords.y] != null)
        {
            SceneManager.LoadScene(mapRooms[currentMapCoords.x, currentMapCoords.y]);
            MoveCurrentPlayerPositionAndCenterMap();
            OnRoomChangeRenderMapPart2(doorPlacement);
        }
        else
        {
            Debug.LogError("Room Not Found");
            currentMapCoords = previousMapCoords;
        }
    }

    private void OnDoorEntered(string levelToLoad)
    {
        if (levelToLoad == "LastRoom")
        {
            SceneManager.LoadScene(mapRooms[currentMapCoords.x, currentMapCoords.y]);
        }
        else
        {
            SceneManager.LoadScene(levelToLoad);
        }
    }

    private void MoveCurrentPlayerPositionAndCenterMap()
    {
        //we change the location of red indication of the player in the map 
        if (currentPlayerLocationInitialized)
        {
            Vector3 mapCoordinates = ConvertArrayCoordinates(currentMapCoords.x, currentMapCoords.y);
            playerCurrentMapLocation.transform.localPosition = mapCoordinates;
            mapTransform.localPosition = new Vector3(-mapCoordinates.x, -mapCoordinates.y, 0);
        }
        else
        {
            Vector2 mapCoordinates = ConvertArrayCoordinates(currentMapCoords.x, currentMapCoords.y);
            playerCurrentMapLocation = ExtensionMethods.InstantiateAtLocalPosition(playerCurrentMapLocationPrefab, mapTransform, mapCoordinates);
            mapTransform.localPosition = new Vector3(-mapCoordinates.x, -mapCoordinates.y, 0);
            currentPlayerLocationInitialized = true;
        }
    }

    public void GetMap(int[,] mapLayout, string[,] mapRooms, MapType mapType)
    {
        if (mapType == MapType.forest && !forestMapExists)
        {
            Array.Copy(mapLayout, 0, forestMapLayout, 0, mapLayout.Length);
            Array.Copy(mapRooms, 0, forestMapRooms, 0, mapRooms.Length);
            forestMapExists = true;
        }
        else if (mapType == MapType.cave && !caveMapExists)
        {
            Array.Copy(mapLayout, 0, caveMapLayout, 0, mapLayout.Length);
            Array.Copy(mapRooms, 0, caveMapRooms, 0, mapRooms.Length);
            caveMapExists = true;
        }
    }



    private void OnRoomChangeRenderMapPart2(Direction renderDirection)
    {

        Vector2 mapCoordinates = Vector2.zero;
        if (renderDirection == Direction.west)
        {
            if (mapLayout[currentMapCoords.x, currentMapCoords.y] != 0)
            {
                //this check is so we don't go out of array bounds.
                if (currentMapCoords.x + 1 < mapLayout.GetLength(0))
                {
                    //Render road
                    mapCoordinates = ConvertArrayCoordinates(currentMapCoords.x + 1, currentMapCoords.y);
                    ExtensionMethods.InstantiateAtLocalPosition(rooms[mapLayout[currentMapCoords.x + 1, currentMapCoords.y]], mapTransform, mapCoordinates);
                    //we zero them so we don't render them again!
                    mapLayout[currentMapCoords.x + 1, currentMapCoords.y] = 0;
                }

                //Render room
                mapCoordinates = ConvertArrayCoordinates(currentMapCoords.x, currentMapCoords.y);
                ExtensionMethods.InstantiateAtLocalPosition(rooms[mapLayout[currentMapCoords.x, currentMapCoords.y]], mapTransform, mapCoordinates);
                //we zero them so we don't render them again!
                mapLayout[currentMapCoords.x, currentMapCoords.y] = 0;

            }
        }
        else if (renderDirection == Direction.east)
        {
            if (mapLayout[currentMapCoords.x, currentMapCoords.y] != 0)
            {

                if (currentMapCoords.x - 1 >= 0)
                {
                    mapCoordinates = ConvertArrayCoordinates(currentMapCoords.x - 1, currentMapCoords.y);
                    ExtensionMethods.InstantiateAtLocalPosition(rooms[mapLayout[currentMapCoords.x - 1, currentMapCoords.y]], mapTransform, mapCoordinates);
                    mapLayout[currentMapCoords.x - 1, currentMapCoords.y] = 0;
                }

                mapCoordinates = ConvertArrayCoordinates(currentMapCoords.x, currentMapCoords.y);
                ExtensionMethods.InstantiateAtLocalPosition(rooms[mapLayout[currentMapCoords.x, currentMapCoords.y]], mapTransform, mapCoordinates);
                mapLayout[currentMapCoords.x, currentMapCoords.y] = 0;
            }
        }
        else if (renderDirection == Direction.north)
        {
            if (mapLayout[currentMapCoords.x, currentMapCoords.y] != 0)
            {
                if (currentMapCoords.y + 1 < mapLayout.GetLength(1))
                {
                    mapCoordinates = ConvertArrayCoordinates(currentMapCoords.x, currentMapCoords.y + 1);
                    ExtensionMethods.InstantiateAtLocalPosition(rooms[mapLayout[currentMapCoords.x, currentMapCoords.y + 1]], mapTransform, mapCoordinates);
                    mapLayout[currentMapCoords.x, currentMapCoords.y + 1] = 0;
                }

                mapCoordinates = ConvertArrayCoordinates(currentMapCoords.x, currentMapCoords.y);
                ExtensionMethods.InstantiateAtLocalPosition(rooms[mapLayout[currentMapCoords.x, currentMapCoords.y]], mapTransform, mapCoordinates);
                mapLayout[currentMapCoords.x, currentMapCoords.y] = 0;
            }
        }
        else if (renderDirection == Direction.south)
        {
            if (mapLayout[currentMapCoords.x, currentMapCoords.y] != 0)
            {
                if (currentMapCoords.y - 1 >= 0)
                {
                    mapCoordinates = ConvertArrayCoordinates(currentMapCoords.x, currentMapCoords.y - 1);
                    ExtensionMethods.InstantiateAtLocalPosition(rooms[mapLayout[currentMapCoords.x, currentMapCoords.y - 1]], mapTransform, mapCoordinates);
                    mapLayout[currentMapCoords.x, currentMapCoords.y - 1] = 0;
                }

                mapCoordinates = ConvertArrayCoordinates(currentMapCoords.x, currentMapCoords.y);
                ExtensionMethods.InstantiateAtLocalPosition(rooms[mapLayout[currentMapCoords.x, currentMapCoords.y]], mapTransform, mapCoordinates);
                mapLayout[currentMapCoords.x, currentMapCoords.y] = 0;

            }
        }
    }

    private void ResetMap()
    {
        mapTransform.DestroyAllChildren();
        ExtensionMethods.InstantiateAtLocalPosition(townIcon, mapTransform, Vector2Int.zero);
        caveMapExists = false;
        forestMapExists = false;
        currentPlayerLocationInitialized = false;
        mapLayout = null;
        mapRooms = null;
    }


    private Vector2 ConvertArrayCoordinates(int x, int y)
    {
        if (currentMap == MapType.forest)
        {
            Vector2 mapCoordinates = new Vector2(40 + x * 20, 0);
            return mapCoordinates;
        }
        else if (currentMap == MapType.cave)
        {
            Vector2 mapCoordinates = new Vector2(-80 + x * 20, -40 - y * 20);
            return mapCoordinates;
        }
        else
        {
            Debug.Log("Error not correct maptype: Function ConvertArrayCoordinates: MapManager");
            return Vector2.zero;
        }
    }

    private void RenderMap()
    {
        for (int i = 0; i < mapLayout.GetLength(0); i++)
        {
            for (int j = 0; j < mapLayout.GetLength(1); j++)
            {
                if (mapLayout[i, j] != 0)
                {
                    Vector2 mapCoordinates = ConvertArrayCoordinates(i, j);
                    ExtensionMethods.InstantiateAtLocalPosition(rooms[mapLayout[i, j]], mapTransform, mapCoordinates);
                }
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EnemiesSerializer enemiesSerializer = FindObjectOfType<EnemiesSerializer>();
        if(enemiesSerializer != null)
        {
            enemiesSerializer.mapType = currentMap;
            enemiesSerializer.roomKey = currentMapCoords.x.ToString() + currentMapCoords.y.ToString();
        }
    }
}
