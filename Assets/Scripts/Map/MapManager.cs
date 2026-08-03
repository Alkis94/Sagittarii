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

    private static readonly Vector2Int[] neighborDirections =
    {
        new(1, 0), new(-1, 0), new(0, 1), new(0, -1)
    };

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
        if (scene.name == SceneNames.Town)
        {
            ExtensionMethods.InstantiateAtLocalPosition(townIcon, mapTransform, Vector2Int.zero);
            CurrentMapInfo.PlayerLocation = null;
            CurrentMapInfo.SetCurrentMap(MapType.Town);

            if (GameManager.Instance.IsNewRun)
            {
                ForestMapCreator.Instance.CreateMap();
                CaveMapCreator.Instance.CreateMap();
            }
            else
            {
                ForestMapCreator.Instance.LoadMap(MapNames.Forest);
                CaveMapCreator.Instance.LoadMap(MapNames.Cave);
                ForestMapCreator.Instance.LoadMapState(MapNames.Forest);
                CaveMapCreator.Instance.LoadMapState(MapNames.Cave);
                RedrawMaps();
            }
        }

        if (CurrentMapInfo.Map == null)
        {
            return;
        }

        if (CurrentMapInfo.CurrentCell.CellType == MapCellType.Room && CurrentMapInfo.CurrentCell.Room.HasTreasure)
        {
            var treasureChest = FindObjectOfType<TreasureChest>();

            if (treasureChest != null)
            {
                treasureChest.EnableChest();
            }
        }

        var roomKey = CurrentMapInfo.Coords.x.ToString() + CurrentMapInfo.Coords.y.ToString();
        OnRoomLoaded?.Invoke(CurrentMapInfo.Type, roomKey, CurrentMapInfo.CurrentCell.Room.Type);
    }

    private void ChangeMap(MapType currentMap, MapType nextMap)
    {
        if (nextMap == MapType.Town)
        {
            CurrentMapInfo.SetCurrentMap(MapType.Town);
            SceneFader.Instance.LoadSceneWithFade(SceneNames.Town);
            UIManager.Instance.ShowLocation(LocationNames.Town);
        }
        else if (nextMap == MapType.Forest)
        {
            CurrentMapInfo.SetCurrentMap(MapType.Forest, ForestMapCreator.Instance.Map, ForestMapCreator.Instance.ForestFirstRoomCoordinates, ForestMapCreator.Instance.Icons);

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

            SceneFader.Instance.LoadSceneWithFade(CurrentMapInfo.CurrentCell.Room.Name);
            UIManager.Instance.ShowLocation(LocationNames.Forest);
        }
        else if (nextMap == MapType.Cave)
        {
            CurrentMapInfo.SetCurrentMap(MapType.Cave, CaveMapCreator.Instance.Map, CaveMapCreator.Instance.CaveFirstRoomCoordinates, CaveMapCreator.Instance.Icons);

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

            SceneFader.Instance.LoadSceneWithFade(CurrentMapInfo.CurrentCell.Room.Name);
            UIManager.Instance.ShowLocation(LocationNames.Cave);
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

        var targetRoom = CurrentMapInfo.CurrentCell.Room;

        if (targetRoom != null)
        {
            SceneFader.Instance.LoadSceneWithFade(targetRoom.Name);
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
            SceneFader.Instance.LoadSceneWithFade(CurrentMapInfo.CurrentCell.Room.Name);
        }
        else
        {
            SceneFader.Instance.LoadSceneWithFade(levelToLoad);
        }
    }

    private void MoveCurrentPlayerPositionAndCenterMap()
    {
        var mapCoordinates = ConvertArrayCoordinates(CurrentMapInfo.Type, CurrentMapInfo.Coords.x, CurrentMapInfo.Coords.y);

        if (CurrentMapInfo.PlayerLocation != null)
        {
            CurrentMapInfo.PlayerLocation.transform.localPosition = mapCoordinates;
        }
        else
        {
            CurrentMapInfo.PlayerLocation = ExtensionMethods.InstantiateAtLocalPosition(playerCurrentMapLocationPrefab, mapTransform, mapCoordinates);
        }

        mapTransform.localPosition = new Vector3(-mapCoordinates.x, -mapCoordinates.y, 0);
    }

    private void OnRoomChangeRenderMapPart()
    {
        if (CurrentMapInfo.CurrentCell.Room != null && CurrentMapInfo.CurrentCell.Room.Unexplored)
        {
            CurrentMapInfo.CurrentCell.Room.Unexplored = false;
            CurrentMapInfo.CurrentCell.Icon.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1);
            DrawNeighborUnexploredRooms();
        }
    }

    private GameObject ReturnCorrectMapIcon(MapCell cell, MapIcons icons)
    {
        if (cell.CellType == MapCellType.Road)
        {
            switch (cell.RoadType)
            {
                case RoadType.Horizontal:
                    return icons.HorizontalRoad;
                case RoadType.Vertical:
                    return icons.VerticalRoad;
            }
        }
        else if (cell.CellType == MapCellType.Room)
        {
            switch (cell.Room.Type)
            {
                case RoomType.StartingRoom:
                case RoomType.NormalRoom:
                    return icons.NormalRoom;
                case RoomType.BossRoom:
                    return icons.BossRoom;
            }
        }

        return null;
    }

    private void DrawNeighborUnexploredRooms()
    {
        foreach (var direction in neighborDirections)
        {
            var roadCoords = CurrentMapInfo.Coords + direction;
            var roomCoords = CurrentMapInfo.Coords + direction * 2;

            if (roomCoords.x < 0 || roomCoords.x >= CurrentMapInfo.Map.GetLength(0) ||
                roomCoords.y < 0 || roomCoords.y >= CurrentMapInfo.Map.GetLength(1))
            {
                continue;
            }

            if (CurrentMapInfo.Map[roadCoords.x, roadCoords.y].CellType == MapCellType.Road &&
                CurrentMapInfo.Map[roomCoords.x, roomCoords.y].CellType == MapCellType.Room)
            {
                DrawMapPart(roadCoords.x, roadCoords.y);
                DrawMapPart(roomCoords.x, roomCoords.y);
            }
        }

        SaveMapChanges();
    }

    private Vector2 ConvertArrayCoordinates(MapType mapType, int x, int y)
    {
        if (mapType == MapType.Forest)
        {
            var mapCoordinates = new Vector2(40 + x * 20, 0);
            return mapCoordinates;
        }
        else if (mapType == MapType.Cave)
        {
            var mapCoordinates = new Vector2(-120 + x * 20, -20 - y * 20);
            return mapCoordinates;
        }
        else
        {
            Debug.LogError("Error not correct maptype: Function ConvertArrayCoordinates: MapManager");
            return Vector2.zero;
        }
    }

    private void DrawMapPart(int coordX, int coordY)
    {
        if (CurrentMapInfo.Map[coordX, coordY].Drawn)
        {
            return;
        }

        if (RenderMapPart(CurrentMapInfo.Map, CurrentMapInfo.Type, CurrentMapInfo.Icons, coordX, coordY))
        {
            CurrentMapInfo.Map[coordX, coordY].Drawn = true;
        }
    }

    private void DrawFullMap()
    {
        for (var i = 0; i < CurrentMapInfo.Map.GetLength(0); i++)
        {
            for (var j = 0; j < CurrentMapInfo.Map.GetLength(1); j++)
            {
                if (CurrentMapInfo.Map[i, j].CellType != MapCellType.None)
                {
                    DrawMapPart(i, j);
                }
            }
        }
    }

    private void RedrawMaps()
    {
        RedrawMap(ForestMapCreator.Instance.Map, MapType.Forest, ForestMapCreator.Instance.Icons);
        RedrawMap(CaveMapCreator.Instance.Map, MapType.Cave, CaveMapCreator.Instance.Icons);
    }

    private void RedrawMap(MapCell[,] map, MapType mapType, MapIcons icons)
    {
        for (var i = 0; i < map.GetLength(0); i++)
        {
            for (var j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j].CellType != MapCellType.None && map[i, j].Drawn)
                {
                    RenderMapPart(map, mapType, icons, i, j);
                }
            }
        }
    }

    /// <summary>
    /// Instantiates the icon for a map cell and greys it out if it's an unexplored room.
    /// Returns whether an icon was actually rendered (cell types like empty/no-icon roads return false).
    /// </summary>
    private bool RenderMapPart(MapCell[,] map, MapType mapType, MapIcons icons, int coordX, int coordY)
    {
        var correctIcon = ReturnCorrectMapIcon(map[coordX, coordY], icons);

        if (correctIcon == null)
        {
            return false;
        }

        var mapCoordinates = ConvertArrayCoordinates(mapType, coordX, coordY);
        map[coordX, coordY].Icon = ExtensionMethods.InstantiateAtLocalPosition(correctIcon, mapTransform, mapCoordinates);

        if (map[coordX, coordY].CellType == MapCellType.Room && map[coordX, coordY].Room.Unexplored)
        {
            map[coordX, coordY].Icon.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
        }

        return true;
    }

    private void SaveMapChanges()
    {
        switch (CurrentMapInfo.Type)
        {
            case MapType.Forest:
                ForestMapCreator.Instance.SaveMapState(MapNames.Forest);
                break;
            case MapType.Cave:
                CaveMapCreator.Instance.SaveMapState(MapNames.Cave);
                break;
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

    public MapCell CurrentCell => Map?[Coords.x, Coords.y];
    public Room CurrentRoom => CurrentCell?.Room;

    public void SetCurrentMap(MapType type, MapCell[,] map = null, Vector2Int coords = default, MapIcons icons = null)
    {
        Map = map;
        Type = type;
        Coords = coords;
        Icons = icons;
    }
}
