using UnityEngine;

public class MapCell
{
    public MapCellType CellType { get; set; } = MapCellType.None;
    public Room Room { get; set; } = null;
    public RoadType? RoadType { get; set; } = null;
    public GameObject Icon { get; set; } = null;
    public bool IconPlaced { get; set; } = false;

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
}

public class Room 
{
    public RoomType Type { get; set; } = RoomType.NormalRoom;
    public string Name { get; set; } = "";
    public bool HasTreasure { get; set; } = false;
    public bool Unexplored { get; set; } = true;
    public RoomOpenings RoomOpenings { get; set; } = RoomOpenings.None;
}
