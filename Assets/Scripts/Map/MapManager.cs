using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

[DefaultExecutionOrder(-99)]
public class MapManager : SerializedMonoBehaviour
{
    public static MapManager Instance = null;
    public static event Action<MapType, string,RoomType> OnRoomLoaded = delegate { };

    public CurrentMapInfo CurrentMapInfo { get; private set; } = new();

    [SerializeField]
    private bool renderFullMap = false;
    [SerializeField]
    private Transform mapTransform;
    [SerializeField]
    private GameObject playerCurrentMapLocationPrefab;
    [SerializeField]
    private GameObject townIcon;

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
        BossDoor.DoorEntered += OnDoorEntered;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        RoomChanger.OnRoomChangerEntered -= ChangeRoom;
        MapChanger.OnMapChangerEntered -= ChangeMap;
        BossDoor.DoorEntered -= OnDoorEntered;
    }

    private void Start()
    {
        ExtensionMethods.InstantiateAtLocalPosition(townIcon, mapTransform, Vector2Int.zero);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Town")
        {
            ExtensionMethods.InstantiateAtLocalPosition(townIcon, mapTransform, Vector2Int.zero);
            CurrentMapInfo.PlayerLocation = null;
            CurrentMapInfo.SetCurrentMap(MapType.Town);

            if (GameManager.Instance.IsNewGame)
            {
                ForestMapCreator.Instance.CreateMap();
                CaveMapCreator.Instance.CreateMap();
            }
        }

        if (CurrentMapInfo.Map == null)
        {
            return;
        }

        if (CurrentMapInfo.Map[CurrentMapInfo.Coords.x, CurrentMapInfo.Coords.y].CellType == MapCellType.Room &&
           CurrentMapInfo.Map[CurrentMapInfo.Coords.x, CurrentMapInfo.Coords.y].Room.HasTreasure)
        {
            var treasureChest = FindObjectOfType<TreasureChest>();
            if (treasureChest != null)
            {
                treasureChest.EnableChest();
            }
        }

        var roomKey = CurrentMapInfo.Coords.x.ToString() + CurrentMapInfo.Coords.y.ToString();
        OnRoomLoaded?.Invoke(CurrentMapInfo.Type, roomKey, CurrentMapInfo.Map[CurrentMapInfo.Coords.x, CurrentMapInfo.Coords.y].Room.Type);
    }

    private void ChangeMap(MapType currentMap, MapType nextMap)
    {
        if (nextMap == MapType.Town)
        {
            CurrentMapInfo.SetCurrentMap(MapType.Town);
            SceneFader.Instance.LoadSceneWithFade(SceneNames.Town);
            UIManager.Instance.CallLocationText(LocationNames.Town);
        }
        else if (nextMap == MapType.Forest)
        {
            CurrentMapInfo.SetCurrentMap(MapType.Forest, ForestMapCreator.Instance.Map, ForestMapCreator.Instance.ForestFirstRoomCoordinates, ForestMapCreator.Instance.ForestIcons);

            if (renderFullMap)
            {
                DrawFullMap();
            }
            else
            {
                DrawMapPart(0, 0);
                DrawMapPart(1, 0);
                DrawMapPart(2, 0);
                DrawNeighborUnexploredRooms();
            }

            MoveCurrentPlayerPositionAndCenterMap();

            SceneFader.Instance.LoadSceneWithFade(CurrentMapInfo.Map[CurrentMapInfo.Coords.x, CurrentMapInfo.Coords.y].Room.Name);
            UIManager.Instance.CallLocationText(LocationNames.Forest);
        }
        else if (nextMap == MapType.Cave)
        {
            CurrentMapInfo.SetCurrentMap(MapType.Cave, CaveMapCreator.Instance.Map, CaveMapCreator.Instance.CaveFirstRoomCoordinates, CaveMapCreator.Instance.CaveIcons);

            if (renderFullMap)
            {
                DrawFullMap();
            }
            else
            {
                DrawMapPart(10, 0);
                DrawMapPart(10, 1);
                DrawMapPart(10, 2);
                DrawMapPart(10, 3);
                DrawMapPart(10, 4);
                DrawNeighborUnexploredRooms();
            }

            MoveCurrentPlayerPositionAndCenterMap();

            SceneFader.Instance.LoadSceneWithFade(CurrentMapInfo.Map[CurrentMapInfo.Coords.x, CurrentMapInfo.Coords.y].Room.Name);
            UIManager.Instance.CallLocationText(LocationNames.Cave);
            
            if (renderFullMap)
            {
                DrawFullMap();
            }
        }
    }

    private void ChangeRoom(Direction doorPlacement)
    {
        var previousMapCoords = CurrentMapInfo.Coords;

        CurrentMapInfo.Coords = doorPlacement switch
        {
            Direction.West => new Vector2Int(CurrentMapInfo.Coords.x - 2, CurrentMapInfo.Coords.y),
            Direction.East => new Vector2Int(CurrentMapInfo.Coords.x + 2, CurrentMapInfo.Coords.y),
            Direction.North => new Vector2Int(CurrentMapInfo.Coords.x, CurrentMapInfo.Coords.y - 2),
            Direction.South => new Vector2Int(CurrentMapInfo.Coords.x, CurrentMapInfo.Coords.y + 2),
            _ => CurrentMapInfo.Coords
        };

        if (CurrentMapInfo.Map[CurrentMapInfo.Coords.x, CurrentMapInfo.Coords.y].Room != null)
        {
            SceneFader.Instance.LoadSceneWithFade(CurrentMapInfo.Map[CurrentMapInfo.Coords.x, CurrentMapInfo.Coords.y].Room.Name);
            MoveCurrentPlayerPositionAndCenterMap();
            OnRoomChangeRenderMapPart();
        }
        else
        {
            Debug.LogError("Room Not Found! CurrentMapCoords : " + CurrentMapInfo.Coords);
            CurrentMapInfo.Coords = previousMapCoords;
        }
    }

    private void OnDoorEntered(string levelToLoad)
    {
        if (levelToLoad == "LastRoom")
        {
            SceneFader.Instance.LoadSceneWithFade(CurrentMapInfo.Map[CurrentMapInfo.Coords.x, CurrentMapInfo.Coords.y].Room.Name);
        }
        else
        {
            SceneFader.Instance.LoadSceneWithFade(levelToLoad);
        }
    }

    private void MoveCurrentPlayerPositionAndCenterMap()
    {
        if (CurrentMapInfo.PlayerLocation != null)
        {
            var mapCoordinates = ConvertArrayCoordinates(CurrentMapInfo.Coords.x, CurrentMapInfo.Coords.y);
            CurrentMapInfo.PlayerLocation.transform.localPosition = mapCoordinates;
            mapTransform.localPosition = new Vector3(-mapCoordinates.x, -mapCoordinates.y, 0);
        }
        else
        {
            var mapCoordinates = ConvertArrayCoordinates(CurrentMapInfo.Coords.x, CurrentMapInfo.Coords.y);
            CurrentMapInfo.PlayerLocation = ExtensionMethods.InstantiateAtLocalPosition(playerCurrentMapLocationPrefab, mapTransform, mapCoordinates);
            mapTransform.localPosition = new Vector3(-mapCoordinates.x, -mapCoordinates.y, 0);
        }
    }

    private void OnRoomChangeRenderMapPart()
    {
        if (CurrentMapInfo.Map[CurrentMapInfo.Coords.x, CurrentMapInfo.Coords.y].Room != null && CurrentMapInfo.Map[CurrentMapInfo.Coords.x, CurrentMapInfo.Coords.y].Room.Unexplored)
        {
            CurrentMapInfo.Map[CurrentMapInfo.Coords.x, CurrentMapInfo.Coords.y].Room.Unexplored = false;
            CurrentMapInfo.Map[CurrentMapInfo.Coords.x, CurrentMapInfo.Coords.y].Icon.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1);
            DrawNeighborUnexploredRooms();
        }
    }

    private GameObject ReturnCorrectMapIcon(MapCell cell)
    {
        if (cell.CellType == MapCellType.Road)
        {
            switch (cell.RoadType)
            {
                case RoadType.Horizontal:
                    return CurrentMapInfo.Icons.HorizontalRoad;
                case RoadType.Vertical:
                    return CurrentMapInfo.Icons.VerticalRoad;
            }
        }
        else if (cell.CellType == MapCellType.Room)
        {
            switch (cell.Room.Type)
            {
                case RoomType.StartingRoom:
                case RoomType.NormalRoom:
                    return CurrentMapInfo.Icons.NormalRoom;
                case RoomType.BossRoom:
                    return CurrentMapInfo.Icons.BossRoom;
            }
        }

        return null;
    }

    private void DrawNeighborUnexploredRooms()
    {
        // Draw East
        if (CurrentMapInfo.Coords.x + 2 < CurrentMapInfo.Map.GetLength(0))
        {
            if (CurrentMapInfo.Map[CurrentMapInfo.Coords.x + 1, CurrentMapInfo.Coords.y].CellType == MapCellType.Road &&
                CurrentMapInfo.Map[CurrentMapInfo.Coords.x + 2, CurrentMapInfo.Coords.y].CellType == MapCellType.Room)
            {
                DrawMapPart(CurrentMapInfo.Coords.x + 1, CurrentMapInfo.Coords.y);
                DrawMapPart(CurrentMapInfo.Coords.x + 2, CurrentMapInfo.Coords.y);
            }
        }
        // Draw West
        if (CurrentMapInfo.Coords.x - 2 >= 0)
        {
            if (CurrentMapInfo.Map[CurrentMapInfo.Coords.x - 1, CurrentMapInfo.Coords.y].CellType == MapCellType.Road &&
                CurrentMapInfo.Map[CurrentMapInfo.Coords.x - 2, CurrentMapInfo.Coords.y].CellType == MapCellType.Room)
            {
                DrawMapPart(CurrentMapInfo.Coords.x - 1, CurrentMapInfo.Coords.y);
                DrawMapPart(CurrentMapInfo.Coords.x - 2, CurrentMapInfo.Coords.y);
            }
        }
        // Draw South
        if (CurrentMapInfo.Coords.y + 2 < CurrentMapInfo.Map.GetLength(1))
        {
            if (CurrentMapInfo.Map[CurrentMapInfo.Coords.x, CurrentMapInfo.Coords.y + 1].CellType == MapCellType.Road &&
                CurrentMapInfo.Map[CurrentMapInfo.Coords.x, CurrentMapInfo.Coords.y + 2].CellType == MapCellType.Room)
            {
                DrawMapPart(CurrentMapInfo.Coords.x, CurrentMapInfo.Coords.y + 1);
                DrawMapPart(CurrentMapInfo.Coords.x, CurrentMapInfo.Coords.y + 2);
            }
        }
        // Draw North
        if (CurrentMapInfo.Coords.y - 2 >= 0)
        {
            if (CurrentMapInfo.Map[CurrentMapInfo.Coords.x, CurrentMapInfo.Coords.y - 1].CellType == MapCellType.Road &&
                CurrentMapInfo.Map[CurrentMapInfo.Coords.x, CurrentMapInfo.Coords.y - 2].CellType == MapCellType.Room)
            {
                DrawMapPart(CurrentMapInfo.Coords.x, CurrentMapInfo.Coords.y - 1);
                DrawMapPart(CurrentMapInfo.Coords.x, CurrentMapInfo.Coords.y - 2);
            }
        }
    }

    private Vector2 ConvertArrayCoordinates(int x, int y)
    {
        if (CurrentMapInfo.Type == MapType.Forest)
        {
            var mapCoordinates = new Vector2(40 + x * 20, 0);
            return mapCoordinates;
        }
        else if (CurrentMapInfo.Type == MapType.Cave)
        {
            var mapCoordinates = new Vector2(-120 + x * 20, -20 - y * 20);
            return mapCoordinates;
        }
        else
        {
            Debug.Log("Error not correct maptype: Function ConvertArrayCoordinates: MapManager");
            return Vector2.zero;
        }
    }

    private void DrawMapPart(int coordX, int coordY)
    {
        if (!CurrentMapInfo.Map[coordX, coordY].Undrawn)
        {
            return;
        }

        var correctIcon = ReturnCorrectMapIcon(CurrentMapInfo.Map[coordX, coordY]);

        if (correctIcon != null)
        {
            var mapCoordinates = ConvertArrayCoordinates(coordX, coordY);
            CurrentMapInfo.Map[coordX, coordY].Icon = ExtensionMethods.InstantiateAtLocalPosition(correctIcon, mapTransform, mapCoordinates);

            if (CurrentMapInfo.Map[coordX, coordY].CellType == MapCellType.Room && CurrentMapInfo.Map[coordX, coordY].Room.Unexplored)
            {
                CurrentMapInfo.Map[coordX, coordY].Icon.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            }

            CurrentMapInfo.Map[coordX, coordY].Undrawn = false;
        }
    }

    private void DrawFullMap()
    {
        for (int i = 0; i < CurrentMapInfo.Map.GetLength(0); i++)
        {
            for (int j = 0; j < CurrentMapInfo.Map.GetLength(1); j++)
            {
                if (CurrentMapInfo.Map[i, j].CellType != MapCellType.None)
                {
                    DrawMapPart(i, j);
                }
            }
        }
    }
}

public class CurrentMapInfo
{
    public MapCell[,] Map { get; set; }
    public MapType Type { get; set; } = MapType.Town;
    public Vector2Int Coords { get; set; }
    public MapIcons Icons { get; set; }
    public GameObject PlayerLocation { get; set; } = null;

    public void SetCurrentMap(MapType type, MapCell[,] map = null, Vector2Int coords = default, MapIcons icons = null)
    {
        Map = map;
        Type = type;
        Coords = coords;
        Icons = icons;
    }
}
