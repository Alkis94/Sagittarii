public enum RoomType
{
    //Rooms must be assigned in  Rooms list in this exact order!
    
    noRoom,

    //Roads are allowed to be assigned on both odd and even numbers

    horizontalRoad,
    verticalRoad,

    //These type of rooms should be only assigned in even numbers on map arrays

    normalRoom,
    bossRoom,
    treasureRoom,
    challengeRoom
}