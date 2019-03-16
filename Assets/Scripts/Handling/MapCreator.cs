using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{

    private const int distanceBetweenRooms = 20;
    private const int distanceBetweenRoomAndRoad = 10;

    private int[,] Map = new int[29, 15];

    private Vector2 DownLeftCorner;
    private Vector2 UpRightCorner;
    private Vector2 StartingRoom;

    private RectTransform MapBackground;

    [SerializeField]
    private GameObject UnexploredRoom;
    [SerializeField]
    private GameObject HorizontalRoad;
    [SerializeField]
    private GameObject VerticalRoad;

    void Start()
    {
        MapBackground = GetComponent<RectTransform>();
        DownLeftCorner = new Vector2(-145f, -75f);
        UpRightCorner = new Vector2(135f, 65f);
        StartingRoom = new Vector2(-5f, 5f);



       Instantiate(UnexploredRoom, StartingRoom, Quaternion.identity, MapBackground);
       ExtensionMethods.InstantiateAtLocalPosition(UnexploredRoom,MapBackground, StartingRoom);
    }


    void Update()
    {

    }

    //private GameObject InstantiateObjectAtLocalPosition()
    //{
    //    GameObject newObject = Instantiate(UnexploredRoom, MapBackground) as GameObject;
    //    newObject.transform.localPosition = StartingRoom;
    //    return newObject;
    //}

    private enum RoomType
    {
        ExploredRoom,
        UnexploredRoom,
        Road
    }
}
