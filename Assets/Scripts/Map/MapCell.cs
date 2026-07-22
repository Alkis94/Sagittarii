using UnityEngine;

public class MapCell
{
    public MapCellType CellType = MapCellType.None;
    public Room Room = null;
    public RoadType? RoadType = null;
    public GameObject Icon = null;
    public bool Drawn = false;
    public void AddNormallRoom()
    {
        CellType = MapCellType.Room;

        Room = new Room
        {
            Type = RoomType.NormalRoom
        };
    }

    public void AddCustomRoom(Room room)
    {
        CellType = MapCellType.Room;
        Room = new Room
        {
            Type = room.Type,
            Name = room.Name,
            HasTreasure = room.HasTreasure,
            RoomOpenings = room.RoomOpenings,
            Unexplored = room.Unexplored
        };
    }

    public void AddRoad(RoadType roadType)
    {
        CellType = MapCellType.Road;
        RoadType = roadType;
    }

    public void AddBossRoom()
    {
        CellType = MapCellType.Room;
        Room = new Room
        {
            Type = RoomType.BossRoom
        };
    }

    /// <summary>
    /// Extracts the part of this cell that changes as the map is explored/drawn,
    /// so it can be saved without re-saving the (unchanging) map layout.
    /// </summary>
    public MapCellState GetState()
    {
        return new MapCellState
        {
            Drawn = Drawn,
            Unexplored = Room != null && Room.Unexplored
        };
    }

    /// <summary>
    /// Re-applies previously saved Drawn/Unexplored state onto this cell.
    /// Used after the map layout has been loaded or created.
    /// </summary>
    public void ApplyState(MapCellState state)
    {
        Drawn = state.Drawn;

        if (Room != null)
        {
            Room.Unexplored = state.Unexplored;
        }
    }
}

public class Room
{
    public RoomType Type = RoomType.NormalRoom;
    public string Name = "";
    public bool HasTreasure = false;
    public bool Unexplored = true;
    public RoomOpenings RoomOpenings = RoomOpenings.None;
}

public struct MapCellState
{
    public bool Drawn;
    public bool Unexplored;
}
