using UnityEngine;

public class Room
{
    public RoomType RoomType { get; set; } = RoomType.noRoom;
    public RoomOpenings RoomOpenings { get; set; }
    public Vector2 RoomArrayCoords { get; set; }
    public string RoomName { get; set; } = null;
    public GameObject Icon { get; set; } = null;
    public bool HasTreasure { get; set; } = false;
    public bool IsUnexplored { get; set; } = true;
}
