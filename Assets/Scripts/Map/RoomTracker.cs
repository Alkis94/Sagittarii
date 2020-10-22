using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RoomTracker
{
    public static List<string> ForestRooms { get; private set; }
    public static List<string> ForestSpawnRooms { get; private set; }

    public static List<string> CaveRoomsNSWE { get; private set; }
    public static List<string> CaveRoomsNWE { get; private set; }
    public static List<string> CaveRoomsNSE { get; private set; }
    public static List<string> CaveRoomsNSW { get; private set; }
    public static List<string> CaveRoomsSWE { get; private set; }
    public static List<string> CaveRoomsNS { get; private set; }
    public static List<string> CaveRoomsNW { get; private set; }
    public static List<string> CaveRoomsNE { get; private set; }
    public static List<string> CaveRoomsSW { get; private set; }
    public static List<string> CaveRoomsSE { get; private set; }
    public static List<string> CaveRoomsWE { get; private set; }
    public static List<string> CaveRoomsN { get; private set; }
    public static List<string> CaveRoomsS { get; private set; }
    public static List<string> CaveRoomsW { get; private set; }
    public static List<string> CaveRoomsE { get; private set; }

    static int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CalculateRoomLists()
    {
        ForestRooms = new List<string>();
        ForestSpawnRooms = new List<string>();

        CaveRoomsNSWE = new List<string>();
        CaveRoomsNWE = new List<string>();
        CaveRoomsNSE = new List<string>();
        CaveRoomsNSW = new List<string>();
        CaveRoomsSWE = new List<string>();
        CaveRoomsNS = new List<string>();
        CaveRoomsNW = new List<string>();
        CaveRoomsNE = new List<string>();
        CaveRoomsSW = new List<string>();
        CaveRoomsSE = new List<string>();
        CaveRoomsWE = new List<string>();
        CaveRoomsN = new List<string>();
        CaveRoomsS = new List<string>();
        CaveRoomsW = new List<string>();
        CaveRoomsE = new List<string>();

        string scene;
        for (int i = 0; i < sceneCount; i++)
        {
            scene = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
            
            if(scene.StringStartsWith("ForestRoom"))
            {
                ForestRooms.Add(scene);
            }
            else if(scene.StringStartsWith("ForestSpawn"))
            {
                ForestSpawnRooms.Add(scene);
            }
            else if (scene.StringStartsWith("CaveRoom"))
            { 
                if (scene.StringStartsWith("CaveRoomNSWE"))
                {
                    CaveRoomsNSWE.Add(scene);
                }
                else if (scene.StringStartsWith("CaveRoomNWE"))
                {
                    CaveRoomsNWE.Add(scene);
                }
                else if (scene.StringStartsWith("CaveRoomNSE"))
                {
                    CaveRoomsNSE.Add(scene);
                }
                else if (scene.StringStartsWith("CaveRoomNSW"))
                {
                    CaveRoomsNSW.Add(scene);
                }
                else if (scene.StringStartsWith("CaveRoomSWE"))
                {
                    CaveRoomsSWE.Add(scene);
                }
                else if (scene.StringStartsWith("CaveRoomNS"))
                {
                    CaveRoomsNS.Add(scene);
                }
                else if (scene.StringStartsWith("CaveRoomNE"))
                {
                    CaveRoomsNE.Add(scene);
                }
                else if (scene.StringStartsWith("CaveRoomNW"))
                {
                    CaveRoomsNW.Add(scene);
                }
                else if (scene.StringStartsWith("CaveRoomSW"))
                {
                    CaveRoomsSW.Add(scene);
                }
                else if (scene.StringStartsWith("CaveRoomSE"))
                {
                    CaveRoomsSE.Add(scene);
                }
                else if (scene.StringStartsWith("CaveRoomWE"))
                {
                    CaveRoomsWE.Add(scene);
                }
                else if (scene.StringStartsWith("CaveRoomN"))
                {
                    CaveRoomsN.Add(scene);
                }
                else if (scene.StringStartsWith("CaveRoomS"))
                {
                    CaveRoomsS.Add(scene);
                }
                else if (scene.StringStartsWith("CaveRoomW"))
                {
                    CaveRoomsW.Add(scene);
                }
                else if (scene.StringStartsWith("CaveRoomE"))
                {
                    CaveRoomsE.Add(scene);
                }
            }
        }
    }
 
}
