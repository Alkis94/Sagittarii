using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using Sirenix.Serialization;


[DefaultExecutionOrder(-99)]
public class MapManager : SerializedMonoBehaviour
{
    public static MapManager Instance = null;
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

    public Vector2Int CurrentMapCoords { get; private set; } = new Vector2Int(-1, 0);
    public MapType CurrentMap { get; private set; } = MapType.town;

    [NonSerialized, OdinSerialize]
    private List<List<GameObject>> roomIcons;
    [SerializeField]
    private Transform mapTransform;
    [SerializeField]
    private GameObject playerCurrentMapLocationPrefab;
    [SerializeField]
    private GameObject townIcon;
    private GameObject playerCurrentMapLocation;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
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
            CurrentMap = MapType.town;
            SceneFader.Instance.LoadSceneWithFade("Town");
            UIManager.Instance.CallLocationText("Floyd's Rest");
        }
        else if (currentMap == MapType.town && nextMap == MapType.forest)
        {
            CurrentMapCoords = forestFirstRoomCoordinates;
            CurrentMap = MapType.forest;
            map = forestMap;


            SceneFader.Instance.LoadSceneWithFade(map[forestFirstRoomCoordinates.x, forestFirstRoomCoordinates.y].RoomName);
            UIManager.Instance.CallLocationText("Woods of Astrea");
            OnRoomChangeRenderMapPart();
            MoveCurrentPlayerPositionAndCenterMap();

            //We put an extra road to connect forest and town, and avoid having a room that collides on the town.
            Vector3 mapCoordinates = ConvertArrayCoordinates(0, 0);
            ExtensionMethods.InstantiateAtLocalPosition(roomIcons[(int)CurrentMap - 1] [(int)RoomType.horizontalRoad], mapTransform, mapCoordinates);

            if(renderFullMap)
            {
                RenderMap();
            }

        }
        else if (currentMap == MapType.forest && nextMap == MapType.cave)
        {
            CurrentMap = MapType.cave;
            CurrentMapCoords = caveFirstRoomCoordinates;
            map = caveMap;

            //We put this road to connect forest and caves. This roads is not inside mapLayout. We do it this way so
            //no rooms of caves collide with forrest rooms.
            Vector2 mapCoordinates = new Vector2(80, -20);
            ExtensionMethods.InstantiateAtLocalPosition(roomIcons[(int)CurrentMap - 1][(int)RoomType.verticalRoad], mapTransform, mapCoordinates);

            SceneFader.Instance.LoadSceneWithFade(map[caveFirstRoomCoordinates.x, caveFirstRoomCoordinates.y].RoomName);
            UIManager.Instance.CallLocationText("Mushroom Caverns");
            OnRoomChangeRenderMapPart();
            MoveCurrentPlayerPositionAndCenterMap();

            if (renderFullMap)
            {
                RenderMap();
            }
        }
        else if (currentMap == MapType.cave && nextMap == MapType.forest)
        {
            CurrentMap = MapType.forest;
            CurrentMapCoords = forestFirstRoomCoordinates;
            map = forestMap;
            SceneFader.Instance.LoadSceneWithFade(map[forestFirstRoomCoordinates.x, forestFirstRoomCoordinates.y].RoomName);
            UIManager.Instance.CallLocationText("Woods of Astrea");
            MoveCurrentPlayerPositionAndCenterMap();
        }
    }

    private void ChangeRoom(Direction doorPlacement)
    {

        Vector2Int previousMapCoords = CurrentMapCoords;

        if (doorPlacement == Direction.west)
        {
            CurrentMapCoords = new Vector2Int(CurrentMapCoords.x - 2, CurrentMapCoords.y);
        }
        else if (doorPlacement == Direction.east)
        {
            CurrentMapCoords = new Vector2Int(CurrentMapCoords.x + 2, CurrentMapCoords.y);
        }
        else if (doorPlacement == Direction.north)
        {
            CurrentMapCoords = new Vector2Int(CurrentMapCoords.x, CurrentMapCoords.y - 2);
        }
        else if (doorPlacement == Direction.south)
        {
            CurrentMapCoords = new Vector2Int(CurrentMapCoords.x, CurrentMapCoords.y + 2);
        }
      
        if (map[CurrentMapCoords.x, CurrentMapCoords.y].RoomName != null)
        {
            SceneFader.Instance.LoadSceneWithFade(map[CurrentMapCoords.x, CurrentMapCoords.y].RoomName);
            MoveCurrentPlayerPositionAndCenterMap();
            OnRoomChangeRenderMapPart();
        }
        else
        {
            Debug.LogError("Room Not Found! CurrentMapCoords : " + CurrentMapCoords);
            CurrentMapCoords = previousMapCoords;
        }
    }

    private void OnDoorEntered(string levelToLoad)
    {
        if (levelToLoad == "LastRoom")
        {
            SceneFader.Instance.LoadSceneWithFade(map[CurrentMapCoords.x, CurrentMapCoords.y].RoomName);
        }
        else
        {
            SceneFader.Instance.LoadSceneWithFade(levelToLoad);
        }
    }

    private void MoveCurrentPlayerPositionAndCenterMap()
    {
        //we change the location of red indication of the player in the map 
        if (currentPlayerLocationInitialized)
        {
            Vector3 mapCoordinates = ConvertArrayCoordinates(CurrentMapCoords.x, CurrentMapCoords.y);
            playerCurrentMapLocation.transform.localPosition = mapCoordinates;
            mapTransform.localPosition = new Vector3(-mapCoordinates.x, -mapCoordinates.y, 0);
        }
        else
        {
            Vector2 mapCoordinates = ConvertArrayCoordinates(CurrentMapCoords.x, CurrentMapCoords.y);
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
        if (map[CurrentMapCoords.x, CurrentMapCoords.y].IsUnexplored)
        {
            map[CurrentMapCoords.x, CurrentMapCoords.y].IsUnexplored = false;
            PlaceMapPart(CurrentMapCoords.x, CurrentMapCoords.y);
            RenderNeighborUnexploredRooms();
        }
    }

    private GameObject PlaceMapPart(int coordX, int coordY)
    {
        GameObject roomIcon = ReturnCorrectMapIcon(coordX, coordY);
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

    private GameObject ReturnCorrectMapIcon(int coordX, int coordY)
    {
        GameObject icon = null;
        switch(map[coordX, coordY].RoomType)
        {
            case RoomType.noRoom:
                return null;
            case RoomType.horizontalRoad:
                icon = roomIcons[(int)CurrentMap - 1][(int)RoomType.horizontalRoad];
                break;
            case RoomType.verticalRoad:
                icon = roomIcons[(int)CurrentMap - 1][(int)RoomType.verticalRoad];
                break;
            case RoomType.bossRoom:
                icon = roomIcons[(int)CurrentMap - 1][(int)RoomType.bossRoom];
                break;
            default:
                icon = roomIcons[(int)CurrentMap - 1][(int)RoomType.normalRoom];
                break;
        }

        Vector2 mapCoordinates = ConvertArrayCoordinates(coordX, coordY);
        return ExtensionMethods.InstantiateAtLocalPosition(icon, mapTransform, mapCoordinates);
    }

    private void RenderNeighborUnexploredRooms()
    {
         //Draw East
        if (CurrentMapCoords.x + 2 < map.GetLength(0))
        {
            if(map[CurrentMapCoords.x + 1, CurrentMapCoords.y].RoomType > 0)
            {
                if(map[CurrentMapCoords.x + 2, CurrentMapCoords.y].IsUnexplored)
                {
                    PlaceMapPart(CurrentMapCoords.x + 1, CurrentMapCoords.y);
                    PlaceMapPart(CurrentMapCoords.x + 2, CurrentMapCoords.y);
                }
            }
        }
        //Draw West
        if (CurrentMapCoords.x - 2 >= 0)
        {
            if (map[CurrentMapCoords.x - 1, CurrentMapCoords.y].RoomType > 0)
            {
                if (map[CurrentMapCoords.x - 2, CurrentMapCoords.y].IsUnexplored)
                {
                    PlaceMapPart(CurrentMapCoords.x - 1, CurrentMapCoords.y);
                    PlaceMapPart(CurrentMapCoords.x - 2, CurrentMapCoords.y);
                }
            }
        }
        //Draw South
        if (CurrentMapCoords.y + 2 < map.GetLength(1))
        {
            if (map[CurrentMapCoords.x, CurrentMapCoords.y + 1].RoomType > 0)
            {
                if (map[CurrentMapCoords.x, CurrentMapCoords.y + 1].IsUnexplored)
                {
                    PlaceMapPart(CurrentMapCoords.x, CurrentMapCoords.y + 1);
                    PlaceMapPart(CurrentMapCoords.x, CurrentMapCoords.y + 2);
                }
            }
        }
        //Draw North
        if (CurrentMapCoords.y - 2 >= 0)
        {
            if (map[CurrentMapCoords.x, CurrentMapCoords.y - 1].RoomType > 0)
            {
                if (map[CurrentMapCoords.x, CurrentMapCoords.y - 1].IsUnexplored)
                {
                    PlaceMapPart(CurrentMapCoords.x, CurrentMapCoords.y - 1);
                    PlaceMapPart(CurrentMapCoords.x, CurrentMapCoords.y - 2);
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
        if (CurrentMap == MapType.forest)
        {
            Vector2 mapCoordinates = new Vector2(40 + x * 20, 0);
            return mapCoordinates;
        }
        else if (CurrentMap == MapType.cave)
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

                    GameObject room = ReturnCorrectMapIcon(i, j);
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

        if(map[CurrentMapCoords.x, CurrentMapCoords.y].HasTreasure)
        {
            TreasureChest treasureChest = FindObjectOfType<TreasureChest>();
            if (treasureChest != null)
            {
                treasureChest.EnableChest();
            }
        }

        string roomKey = CurrentMapCoords.x.ToString() + CurrentMapCoords.y.ToString();
        OnRoomLoaded?.Invoke(CurrentMap, roomKey, map[CurrentMapCoords.x,CurrentMapCoords.y].RoomType);
    }

    public RoomType GetMapRoomType()
    {
        return map[CurrentMapCoords.x, CurrentMapCoords.y].RoomType;
    }
}
