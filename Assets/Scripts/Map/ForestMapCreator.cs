using UnityEngine;
using System.Collections.Generic;


public class ForestMapCreator : MapCreator
{

    
    private List<string> forestRooms = new List<string>();


    private static ForestMapCreator instance = null;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        
    }

    // Use this for initialization
    void Start()
    {
        for (int i = 1; i < 11; i++)
        {
            forestRooms.Add("Forest" + i.ToString());
        }

        mapRooms = new string[40, 1];
        mapLayout = new int[40, 1];
        unexploredRoomArrayCoordinates = new List<Vector2Int>();
        CreateMap();
        MapCreated(MapType.forest);

    }

    protected override void CreateMap()
    {
        
        mapLayout[0, 0] = (int)RoomType.horizontalRoad;
        mapLayout[1, 0] = (int)RoomType.horizontalRoad;
        mapLayout[2, 0] = (int)RoomType.exploredRoom;
        mapLayout[3, 0] = (int)RoomType.horizontalRoad;
        mapLayout[4, 0] = (int)RoomType.exploredRoom;
        mapLayout[5, 0] = (int)RoomType.horizontalRoad;
        mapRooms[2, 0] = "ForestFirstRoom";
        mapRooms[4, 0] = "ForestToCave";
        CreatePathToBoss(20, new Vector2Int(6, 0), false, false, false, true);
        int randomNumber;
        for (int i = 6; i < 40; i++)
        {
            if(mapLayout[i,0] == (int)RoomType.unexploredRoom)
            {
                randomNumber = Random.Range(0, forestRooms.Count);
                mapRooms[i, 0] = forestRooms[randomNumber];
            }
        }
    }

}
