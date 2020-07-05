using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldTreeMapCreator : MapCreator
{

    private Vector2Int startingRoom = new Vector2Int(10, 0);

    protected override void Start()
    {
        base.Start();
        mapLayout = new int[20, 40];
        CreateMap();
    }

    protected override void CreateMap()
    {
        mapLayout[10, 39] = (int)RoomType.verticalRoad;
        CreatePathToBoss(15, new Vector2Int(10, 38), true,false,false,false);
    }
}
