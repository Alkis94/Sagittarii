using UnityEngine;
using System.Collections.Generic;


public class ForestMapCreator : MapCreator
{

    private static ForestMapCreator instance = null;
    private List<string> forestRooms = new List<string>();
    
    

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
        for (int i = 1; i < 4; i++)
        {
            forestRooms.Add("TestRoom" + i.ToString());
        }

        mapRooms = new string[40, 1];
        mapLayout = new int[40, 1];
        unexploredRoomArrayCoordinates = new List<Vector2Int>();
        CreateMap();
        MapCreated(MapType.forest);

    }

    protected override void CreateMap()
    {
        //mapLayout[10, 0] = (int)RoomType.exploredRoom;
        //mapLayout[10, 1] = (int)RoomType.verticalRoad;
        CreatePathToBoss(20, new Vector2Int(0, 0), false, false, false, true);
        int randomNumber;
        mapRooms[0, 0] = "TestStartRoom";
        for (int i = 1; i < 40; i++)
        {
            if(mapLayout[i,0] == (int)RoomType.unexploredRoom)
            {
                randomNumber = Random.Range(0, forestRooms.Count);
                mapRooms[i, 0] = forestRooms[randomNumber];
            }
        }

        mapRooms[2, 0] = "TestCaveDoor";
    }

}
