using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private Transform childTransform;
    private Vector2Int currentRoomCoordinates;
    private int[,] mapArray = new int[29, 15];
    [SerializeField]
    private bool renderFullMapWhenCreated = false;

    

    [SerializeField]
    private List<GameObject> rooms;

    private void Awake()
    {
        childTransform = transform.GetChild(0);
        RoomFinish.OnRoomFinished += OnRoomFinishRenderMapPart;
        RoomPressed.OnRoomChosen += ChangeCurrentRoom;
    }

    private void OnDestroy()
    {
        RoomFinish.OnRoomFinished -= OnRoomFinishRenderMapPart;
        RoomPressed.OnRoomChosen -= ChangeCurrentRoom;
    }

    void Start()
    {
        RenderStartingRoom();
    }


    

    private void RenderMap()
    {
        for (int i = 0; i < mapArray.GetLength(0); i++)
        {
            for (int j = 0; j < mapArray.GetLength(1); j++)
            {
                if (mapArray[i, j] != 0)
                {
                    Vector2 mapCoordinates = ConvertArrayCoordinates(i, j);
                    ExtensionMethods.InstantiateAtLocalPosition(rooms[mapArray[i, j]], childTransform, mapCoordinates);
                }
            }
        }
    }

    private void RenderStartingRoom()
    {
        currentRoomCoordinates = new Vector2Int(13, 7);
        Vector2 mapCoordinates = ConvertArrayCoordinates(currentRoomCoordinates.x, currentRoomCoordinates.y);
        ExtensionMethods.InstantiateAtLocalPosition(rooms[(int)RoomType.exploredRoom], childTransform, mapCoordinates);
    }

    private Vector2 ConvertArrayCoordinates(int x, int y)
    {
        Vector2 mapCoordinates = new Vector2(-145 + (x + 1) * 10, -75 + (y + 1) * 10);
        return mapCoordinates;
    }

    private Vector2Int ConvertMapCoordinates(float x, float y)
    {
        Vector2Int mapCoordinates = new Vector2Int( (int)(x + 145)/10 - 1,(int) (y + 75)/10 -1);
        return mapCoordinates;
    }

    public void GetMap(int [,] mapArray)
    {
        Array.Copy(mapArray, 0, this.mapArray,0,mapArray.Length);
        if(renderFullMapWhenCreated)
        {
            RenderMap();
        }
    }

    private void OnRoomFinishRenderMapPart()
    {
        
        Vector2 mapCoordinates;
        //Check west if road and render
        if (mapArray[currentRoomCoordinates.x - 1, currentRoomCoordinates.y] != 0)
        {
            //Render room
            mapCoordinates = ConvertArrayCoordinates(currentRoomCoordinates.x - 2, currentRoomCoordinates.y);
            ExtensionMethods.InstantiateAtLocalPosition(rooms[mapArray[currentRoomCoordinates.x - 2, currentRoomCoordinates.y]], childTransform, mapCoordinates);
            //Render road
            mapCoordinates = ConvertArrayCoordinates(currentRoomCoordinates.x - 1, currentRoomCoordinates.y);
            ExtensionMethods.InstantiateAtLocalPosition(rooms[mapArray[currentRoomCoordinates.x - 1, currentRoomCoordinates.y]], childTransform, mapCoordinates);

            //we zero them so we don't render them again!
            mapArray[currentRoomCoordinates.x - 2, currentRoomCoordinates.y] = 0;
            mapArray[currentRoomCoordinates.x - 1, currentRoomCoordinates.y] = 0;
        }

        //Check east if road and render
        if (mapArray[currentRoomCoordinates.x + 1, currentRoomCoordinates.y] != 0)
        {
            mapCoordinates = ConvertArrayCoordinates(currentRoomCoordinates.x + 2, currentRoomCoordinates.y);
            ExtensionMethods.InstantiateAtLocalPosition(rooms[mapArray[currentRoomCoordinates.x + 2, currentRoomCoordinates.y]], childTransform, mapCoordinates);

            mapCoordinates = ConvertArrayCoordinates(currentRoomCoordinates.x + 1, currentRoomCoordinates.y);
            ExtensionMethods.InstantiateAtLocalPosition(rooms[mapArray[currentRoomCoordinates.x + 1, currentRoomCoordinates.y]], childTransform, mapCoordinates);

            //we zero them so we don't render them again!
            mapArray[currentRoomCoordinates.x + 2, currentRoomCoordinates.y] = 0;
            mapArray[currentRoomCoordinates.x + 1, currentRoomCoordinates.y] = 0;
        }

        //Check north if road and render
        if (mapArray[currentRoomCoordinates.x, currentRoomCoordinates.y + 1] != 0)
        {
            mapCoordinates = ConvertArrayCoordinates(currentRoomCoordinates.x, currentRoomCoordinates.y + 2);
            ExtensionMethods.InstantiateAtLocalPosition(rooms[mapArray[currentRoomCoordinates.x, currentRoomCoordinates.y + 2]], childTransform, mapCoordinates);

            mapCoordinates = ConvertArrayCoordinates(currentRoomCoordinates.x, currentRoomCoordinates.y + 1);
            ExtensionMethods.InstantiateAtLocalPosition(rooms[mapArray[currentRoomCoordinates.x, currentRoomCoordinates.y + 1]], childTransform, mapCoordinates);

            //we zero them so we don't render them again!
            mapArray[currentRoomCoordinates.x, currentRoomCoordinates.y + 2] = 0;
            mapArray[currentRoomCoordinates.x, currentRoomCoordinates.y + 1] = 0;
        }

        //Check south if road and render
        if (mapArray[currentRoomCoordinates.x, currentRoomCoordinates.y - 1] != 0)
        {
            mapCoordinates = ConvertArrayCoordinates(currentRoomCoordinates.x, currentRoomCoordinates.y - 2);
            ExtensionMethods.InstantiateAtLocalPosition(rooms[mapArray[currentRoomCoordinates.x, currentRoomCoordinates.y - 2]], childTransform, mapCoordinates);

            mapCoordinates = ConvertArrayCoordinates(currentRoomCoordinates.x, currentRoomCoordinates.y - 1);
            ExtensionMethods.InstantiateAtLocalPosition(rooms[mapArray[currentRoomCoordinates.x, currentRoomCoordinates.y - 1]], childTransform, mapCoordinates);

            //we zero them so we don't render them again!
            mapArray[currentRoomCoordinates.x, currentRoomCoordinates.y - 2] = 0;
            mapArray[currentRoomCoordinates.x, currentRoomCoordinates.y - 1] = 0;
        }
    }

    private void ChangeCurrentRoom(Vector3 newRoomCoordinates)
    {
        currentRoomCoordinates = ConvertMapCoordinates(newRoomCoordinates.x, newRoomCoordinates.y);
    }
}


