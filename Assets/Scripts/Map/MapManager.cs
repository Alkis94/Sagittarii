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

    private Room[,] map;
    private Room[,] forestMap = new Room[40, 1];
    private Room[,] caveMap = new Room[20, 40];


    private readonly Vector2Int forestFirstRoomCoordinates = new Vector2Int(2, 0);
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
        }
        else if (currentMap == MapType.town && nextMap == MapType.forest)
        {
            currentMapCoords = forestFirstRoomCoordinates;
            this.currentMap = MapType.forest;
            map = forestMap;


            SceneManager.LoadScene(map[forestFirstRoomCoordinates.x, forestFirstRoomCoordinates.y].RoomName);
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
            map = caveMap;

            //We put this road to connect forest and caves. This roads is not inside mapLayout. We do it this way so
            //no rooms of caves collide with forrest rooms.
            Vector2 mapCoordinates = new Vector2(80, -20);
            ExtensionMethods.InstantiateAtLocalPosition(rooms[(int)this.currentMap - 1][(int)RoomType.verticalRoad], mapTransform, mapCoordinates);

            SceneManager.LoadScene(map[caveFirstRoomCoordinates.x, caveFirstRoomCoordinates.y].RoomName);
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
            currentMapCoords = forestFirstRoomCoordinates;
            map = forestMap;
            SceneManager.LoadScene(map[forestFirstRoomCoordinates.x, forestFirstRoomCoordinates.y].RoomName);
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
      
        if (map[currentMapCoords.x, currentMapCoords.y].RoomName != null)
        {
            SceneManager.LoadScene(map[currentMapCoords.x, currentMapCoords.y].RoomName);
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
            SceneManager.LoadScene(map[currentMapCoords.x, currentMapCoords.y].RoomName);
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

    public void SetMap(Room[,] map, MapType mapType)
    {
        if (mapType == MapType.forest && !forestMapExists)
        {
            Array.Copy(map, 0, forestMap, 0, map.Length);
            forestMapExists = true;
        }
        else if (mapType == MapType.cave && !caveMapExists)
        {
            Array.Copy(map, 0, caveMap, 0, map.Length);
            caveMapExists = true;
        }
    }

    private void OnRoomChangeRenderMapPart()
    {
        Vector2 mapCoordinates = Vector2.zero;
        if (map[currentMapCoords.x, currentMapCoords.y].IsUnexplored)
        {
            map[currentMapCoords.x, currentMapCoords.y].IsUnexplored = false;
            PlaceMapPart(currentMapCoords.x, currentMapCoords.y);
            RenderNeighborUnexploredRooms();
        }
    }

    private GameObject PlaceMapPart(int coordX, int coordY)
    {
        Vector2 mapCoordinates = ConvertArrayCoordinates(coordX, coordY);
        GameObject roomIcon = ExtensionMethods.InstantiateAtLocalPosition(rooms[(int)currentMap - 1][(int)map[coordX, coordY].RoomType], mapTransform, mapCoordinates);
        if ((int)map[coordX, coordY].RoomType > 2)
        {
            if (map[coordX, coordY].IsUnexplored)
            {
                roomIcon.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            }
            else
            {
                Destroy(map[coordX, coordY].Icon);
            }
        }
        map[coordX, coordY].Icon = roomIcon;
        return roomIcon;
    }

    private void RenderNeighborUnexploredRooms()
    {
         //Draw East
        if (currentMapCoords.x + 1 < map.GetLength(0))
        {
            if(map[currentMapCoords.x + 1,currentMapCoords.y].RoomType > 0)
            {
                if(map[currentMapCoords.x + 2, currentMapCoords.y].IsUnexplored)
                {
                    PlaceMapPart(currentMapCoords.x + 1, currentMapCoords.y);
                    PlaceMapPart(currentMapCoords.x + 2, currentMapCoords.y);
                }
            }
        }
        //Draw West
        if (currentMapCoords.x - 1 >= 0)
        {
            if (map[currentMapCoords.x - 1, currentMapCoords.y].RoomType > 0)
            {
                if (map[currentMapCoords.x - 2, currentMapCoords.y].IsUnexplored)
                {
                    PlaceMapPart(currentMapCoords.x - 1, currentMapCoords.y);
                    PlaceMapPart(currentMapCoords.x - 2, currentMapCoords.y);
                }
            }
        }
        //Draw South
        if (currentMapCoords.y + 1 < map.GetLength(1))
        {
            if (map[currentMapCoords.x, currentMapCoords.y + 1].RoomType > 0)
            {
                if (map[currentMapCoords.x, currentMapCoords.y + 1].IsUnexplored)
                {
                    PlaceMapPart(currentMapCoords.x, currentMapCoords.y + 1);
                    PlaceMapPart(currentMapCoords.x, currentMapCoords.y + 2);
                }
            }
        }
        //Draw North
        if (currentMapCoords.y - 1 >= 0)
        {
            if (map[currentMapCoords.x, currentMapCoords.y - 1].RoomType > 0)
            {
                if (map[currentMapCoords.x, currentMapCoords.y - 1].IsUnexplored)
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
        caveMapExists = false;
        forestMapExists = false;
        currentPlayerLocationInitialized = false;
        map = null;
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
            Vector2 mapCoordinates = new Vector2(-120 + x * 20, -40 - y * 20);
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
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j].RoomType != RoomType.noRoom)
                {
                    Vector2 mapCoordinates = ConvertArrayCoordinates(i, j);

                    GameObject room = rooms[(int)currentMap - 1][(int)map[i, j].RoomType];
                    room.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
                    ExtensionMethods.InstantiateAtLocalPosition(room, mapTransform, mapCoordinates);
                }
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Town")
        {
            ResetMap();
            if (ES3.DirectoryExists(("Levels/")))
            {
                ES3.DeleteDirectory("Levels/");
                ES3.DeleteDirectory("Levels/");
            }
        }

        if (map == null)
        {
            return;
        }

        if(map[currentMapCoords.x, currentMapCoords.y].HasTreasure)
        {
            TreasureChest treasureChest = FindObjectOfType<TreasureChest>();
            if (treasureChest != null)
            {
                treasureChest.EnableChest();
            }
        }

        string roomKey = currentMapCoords.x.ToString() + currentMapCoords.y.ToString();
        OnRoomLoaded?.Invoke(currentMap, roomKey, map[currentMapCoords.x,currentMapCoords.y].RoomType);
    }
}
