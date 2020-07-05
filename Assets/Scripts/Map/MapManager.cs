using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class MapManager : SerializedMonoBehaviour
{
    private static MapManager instance = null;
    public static event Action<MapType,string,RoomType> OnRoomLoaded = delegate { };

    [SerializeField]
    private bool renderFullMap = false;

    private bool caveMapExists = false;
    private bool forestMapExists = false;
    private bool currentPlayerLocationInitialized = false;

    private int[,] mapLayout;
    private bool[,] mapLayoutIsUnexplored;
    private string[,] mapRooms;
    private GameObject[,] mapIcons;
    private Vector2Int[] mapRoomsWithTreasure;

    private int[,] forestMapLayout = new int[40, 1];
    private bool[,] forestMapLayoutIsUnexplored = new bool[40, 1];
    private string[,] forestMapRooms = new string[40, 1];
    private GameObject[,] forestMapIcons = new GameObject[40, 1];
    private Vector2Int [] forestMapRoomsWithTreasure;

    private int[,] caveMapLayout = new int[20, 40];
    private bool[,] caveMapLayoutIsUnexplored = new bool[20, 40];
    private string[,] caveMapRooms = new string[20, 40];
    private GameObject[,] caveMapIcons = new GameObject[20, 40];
    private Vector2Int[] caveMapRoomsWithTreasure;

    private readonly Vector2Int forestFirstRoomCoordinates = new Vector2Int(2, 0);
    private readonly Vector2Int forestCaveDoorRoomCoordinates = new Vector2Int(4, 0);
    private readonly Vector2Int caveFirstRoomCoordinates = new Vector2Int(10, 0);

    private Vector2Int currentMapCoords = new Vector2Int(-1, 0);

    [NonSerialized, OdinSerialize]
    private List<List<GameObject>> rooms;

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
        ExtensionMethods.SetBoolArrayToTrue(ref forestMapLayoutIsUnexplored);
        ExtensionMethods.SetBoolArrayToTrue(ref caveMapLayoutIsUnexplored);

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        RoomChanger.OnRoomChangerEntered += ChangeRoom;
        MapChanger.OnMapChangerEntered += ChangeMap;
        MapCreator.OnMapCreated += SetMap;
        BossDoor.DoorEntered += OnDoorEntered;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        RoomChanger.OnRoomChangerEntered -= ChangeRoom;
        MapChanger.OnMapChangerEntered -= ChangeMap;
        MapCreator.OnMapCreated -= SetMap;
        BossDoor.DoorEntered -= OnDoorEntered;
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
                ES3.DeleteDirectory("Levels/");
                ES3.DeleteDirectory("Levels/");
            }
            
        }
        else if (currentMap == MapType.town && nextMap == MapType.forest)
        {
            currentMapCoords = forestFirstRoomCoordinates;
            this.currentMap = MapType.forest;

            mapLayout = forestMapLayout;
            mapLayoutIsUnexplored = forestMapLayoutIsUnexplored;
            mapRooms = forestMapRooms;
            mapIcons = forestMapIcons;
            mapRoomsWithTreasure = forestMapRoomsWithTreasure;

            SceneManager.LoadScene(mapRooms[forestFirstRoomCoordinates.x, forestFirstRoomCoordinates.y]);
            OnRoomChangeRenderMapPart();
            MoveCurrentPlayerPositionAndCenterMap();

            //We put an extra road to connect forest and town, and avoid having a room that collides on the town.
            Vector3 mapCoordinates = ConvertArrayCoordinates(0, 0);
            ExtensionMethods.InstantiateAtLocalPosition(rooms[(int)this.currentMap - 1] [(int)RoomType.horizontalRoad], mapTransform, mapCoordinates);

            if(renderFullMap)
            {
                RenderMap();
            }

        }
        else if (currentMap == MapType.forest && nextMap == MapType.cave)
        {
            this.currentMap = MapType.cave;
            currentMapCoords = caveFirstRoomCoordinates;

            mapLayout = caveMapLayout;
            mapLayoutIsUnexplored = caveMapLayoutIsUnexplored;
            mapRooms = caveMapRooms;
            mapIcons = caveMapIcons;
            mapRoomsWithTreasure = caveMapRoomsWithTreasure;

            //We put this road to connect forest and caves. This roads is not inside mapLayout. We do it this way so
            //no rooms of caves collide with forrest rooms.
            Vector2 mapCoordinates = new Vector2(120, -20);
            ExtensionMethods.InstantiateAtLocalPosition(rooms[(int)this.currentMap - 1][(int)RoomType.verticalRoad], mapTransform, mapCoordinates);

            SceneManager.LoadScene(mapRooms[caveFirstRoomCoordinates.x, caveFirstRoomCoordinates.y]);
            OnRoomChangeRenderMapPart();
            MoveCurrentPlayerPositionAndCenterMap();

            if (renderFullMap)
            {
                RenderMap();
            }
        }
        else if (currentMap == MapType.cave && nextMap == MapType.forest)
        {
            this.currentMap = MapType.forest;
            currentMapCoords = forestCaveDoorRoomCoordinates;

            mapLayout = forestMapLayout;
            mapLayoutIsUnexplored = forestMapLayoutIsUnexplored;
            mapRooms = forestMapRooms;
            mapIcons = forestMapIcons;
            mapRoomsWithTreasure = forestMapRoomsWithTreasure;

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
            OnRoomChangeRenderMapPart();
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

    public void SetMap(int[,] mapLayout, string[,] mapRooms, MapType mapType, Vector2Int [] roomsWithTreasure)
    {
        if (mapType == MapType.forest && !forestMapExists)
        {
            Array.Copy(mapLayout, 0, forestMapLayout, 0, mapLayout.Length);
            Array.Copy(mapRooms, 0, forestMapRooms, 0, mapRooms.Length);
            forestMapRoomsWithTreasure = new Vector2Int[roomsWithTreasure.Length];
            Array.Copy(roomsWithTreasure, 0, forestMapRoomsWithTreasure, 0, roomsWithTreasure.Length);
            forestMapExists = true;
        }
        else if (mapType == MapType.cave && !caveMapExists)
        {
            Array.Copy(mapLayout, 0, caveMapLayout, 0, mapLayout.Length);
            Array.Copy(mapRooms, 0, caveMapRooms, 0, mapRooms.Length);
            caveMapRoomsWithTreasure = new Vector2Int[roomsWithTreasure.Length];
            Array.Copy(roomsWithTreasure, 0, caveMapRoomsWithTreasure, 0, roomsWithTreasure.Length);
            caveMapExists = true;
        }
    }

    private void OnRoomChangeRenderMapPart()
    {
        Vector2 mapCoordinates = Vector2.zero;
        if (mapLayoutIsUnexplored[currentMapCoords.x, currentMapCoords.y])
        {
            mapLayoutIsUnexplored[currentMapCoords.x, currentMapCoords.y] = false;
            PlaceMapPart(currentMapCoords.x, currentMapCoords.y);
            RenderNeighborUnexploredRooms();
        }
    }

    private GameObject PlaceMapPart(int coordX, int coordY)
    {
        Vector2 mapCoordinates = ConvertArrayCoordinates(coordX, coordY);
        GameObject room = ExtensionMethods.InstantiateAtLocalPosition(rooms[(int)currentMap - 1][mapLayout[coordX, coordY]], mapTransform, mapCoordinates);
        if (mapLayout[coordX, coordY] > 2)
        {
            if (mapLayoutIsUnexplored[coordX, coordY])
            {
                room.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            }
            else
            {
                Destroy(mapIcons[coordX, coordY]);
            }
        }
        mapIcons[coordX, coordY] = room;
        return room;
    }

    private void RenderNeighborUnexploredRooms()
    {
         //Draw East
        if (currentMapCoords.x + 1 < mapLayout.GetLength(0))
        {
            if(mapLayout[currentMapCoords.x + 1,currentMapCoords.y] > 0)
            {
                if(mapLayoutIsUnexplored[currentMapCoords.x + 2, currentMapCoords.y])
                {
                    PlaceMapPart(currentMapCoords.x + 1, currentMapCoords.y);
                    PlaceMapPart(currentMapCoords.x + 2, currentMapCoords.y);
                }
            }
        }
        //Draw West
        if (currentMapCoords.x - 1 >= 0)
        {
            if (mapLayout[currentMapCoords.x - 1, currentMapCoords.y] > 0)
            {
                if (mapLayoutIsUnexplored[currentMapCoords.x - 2, currentMapCoords.y])
                {
                    PlaceMapPart(currentMapCoords.x - 1, currentMapCoords.y);
                    PlaceMapPart(currentMapCoords.x - 2, currentMapCoords.y);
                }
            }
        }
        //Draw South
        if (currentMapCoords.y + 1 < mapLayout.GetLength(1))
        {
            if (mapLayout[currentMapCoords.x, currentMapCoords.y + 1] > 0)
            {
                if (mapLayoutIsUnexplored[currentMapCoords.x, currentMapCoords.y + 1])
                {
                    PlaceMapPart(currentMapCoords.x, currentMapCoords.y + 1);
                    PlaceMapPart(currentMapCoords.x, currentMapCoords.y + 2);
                }
            }
        }
        //Draw North
        if (currentMapCoords.y - 1 >= 0)
        {
            if (mapLayout[currentMapCoords.x, currentMapCoords.y - 1] > 0)
            {
                if (mapLayoutIsUnexplored[currentMapCoords.x, currentMapCoords.y - 1])
                {
                    PlaceMapPart(currentMapCoords.x, currentMapCoords.y - 1);
                    PlaceMapPart(currentMapCoords.x, currentMapCoords.y - 2);
                }
            }
        }
    }

    private void ResetMap()
    {
        mapTransform.DestroyAllChildren();
        ExtensionMethods.InstantiateAtLocalPosition(townIcon, mapTransform, Vector2Int.zero);
        ExtensionMethods.SetBoolArrayToTrue(ref forestMapLayoutIsUnexplored);
        ExtensionMethods.SetBoolArrayToTrue(ref caveMapLayoutIsUnexplored);
        caveMapExists = false;
        forestMapExists = false;
        currentPlayerLocationInitialized = false;
        mapLayout = null;
        mapRooms = null;
        mapLayoutIsUnexplored = null;
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

                    GameObject room = rooms[(int)currentMap - 1][mapLayout[i, j]];
                    room.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
                    ExtensionMethods.InstantiateAtLocalPosition(room, mapTransform, mapCoordinates);
                }
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(mapLayout == null)
        {
            return;
        }

        for(int i = 0; i < mapRoomsWithTreasure.Length; i++)
        {
            if(currentMapCoords == mapRoomsWithTreasure[i])
            {
                //Debug.Log("Treasure should spawn!");
                TreasureChest treasureChest = FindObjectOfType<TreasureChest>();
                if (treasureChest != null)
                {
                    treasureChest.EnableChest();
                }
            }
        }

        string roomKey = currentMapCoords.x.ToString() + currentMapCoords.y.ToString();
        OnRoomLoaded?.Invoke(currentMap, roomKey,(RoomType)mapLayout[currentMapCoords.x,currentMapCoords.y]);
    }
}
