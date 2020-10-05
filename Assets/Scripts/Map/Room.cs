using UnityEngine;

public class Room
{
    public RoomType RoomType { get; set; } = RoomType.noRoom;
    public string RoomName { get; set; } = null;
    public GameObject Icon { get; set; } = null;
    public bool HasTreasure { get; set; } = false;
    public bool IsUnexplored { get; set; } = true;
}
