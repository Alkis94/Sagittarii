public enum RoomType
{
    // Must be assigned in Rooms list in this exact order
    NoRoom,

    // Roads are allowed to be assigned on both odd and even numbers
    HorizontalRoad,
    VerticalRoad,

    // These type of rooms should be only assigned in even numbers on map arrays
    NormalRoom,
    BossRoom,

    SpawnRoom,
    ChallengeRoom,
    StartingRoom
}