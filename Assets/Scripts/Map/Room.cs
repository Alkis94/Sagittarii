using UnityEngine;

public class Room
{
    public RoomType RoomType { get; set; } = RoomType.NoRoom;
    public RoomOpenings RoomOpenings { get; set; } = RoomOpenings.None;
    public Vector2 RoomArrayCoords { get; set; }
    public string RoomName { get; set; } = null;
    public GameObject Icon { get; set; } = null;
    public bool HasTreasure { get; set; } = false;
    public bool IsUnexplored { get; set; } = true;
    public bool HasOpeningNorth => RoomOpenings is RoomOpenings.N or RoomOpenings.NS or RoomOpenings.NE or RoomOpenings.NW or RoomOpenings.NSE or RoomOpenings.NSW or RoomOpenings.NWE or RoomOpenings.NSWE;
    public bool HasOpeningSouth => RoomOpenings is RoomOpenings.S or RoomOpenings.NS or RoomOpenings.SE or RoomOpenings.SW or RoomOpenings.NSE or RoomOpenings.NSW or RoomOpenings.SWE or RoomOpenings.NSWE;
    public bool HasOpeningEast => RoomOpenings is RoomOpenings.E or RoomOpenings.NE or RoomOpenings.SE or RoomOpenings.WE or RoomOpenings.NSE or RoomOpenings.NWE or RoomOpenings.SWE or RoomOpenings.NSWE;
    public bool HasOpeningWest => RoomOpenings is RoomOpenings.W or RoomOpenings.NW or RoomOpenings.SW or RoomOpenings.WE or RoomOpenings.NSW or RoomOpenings.NWE or RoomOpenings.SWE or RoomOpenings.NSWE;
}
