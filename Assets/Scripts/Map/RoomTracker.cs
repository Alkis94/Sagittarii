using UnityEngine;
using System.Collections.Generic;

public class RoomTracker
{
    public static readonly Dictionary<string, List<string>> CaveRoomLists = new()
    {
        { "CaveRoomNSWE", new() },
        { "CaveRoomNWE", new() },
        { "CaveRoomNSE", new() },
        { "CaveRoomNSW", new() },
        { "CaveRoomSWE", new() },
        { "CaveRoomNS", new() },
        { "CaveRoomNE", new() },
        { "CaveRoomNW", new() },
        { "CaveRoomSW", new() },
        { "CaveRoomSE", new() },
        { "CaveRoomWE", new() },
        { "CaveRoomN", new() },
        { "CaveRoomS", new() },
        { "CaveRoomW", new() },
        { "CaveRoomE", new() }
    };

    public static List<string> ForestRooms { get; private set; } = new();
    public static List<string> ForestSpawnRooms { get; private set; } = new();

    static readonly int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CalculateRoomLists()
    {
        for (int i = 0; i < sceneCount; i++)
        {
            var scene = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));

            if (scene.StringStartsWith("ForestRoom"))
            {
                ForestRooms.Add(scene);
            }
            else if (scene.StringStartsWith("ForestSpawn"))
            {
                ForestSpawnRooms.Add(scene);
            }
            else
            {
                foreach (var prefix in CaveRoomLists.Keys)
                {
                    if (scene.StringStartsWith(prefix))
                    {
                        CaveRoomLists[prefix].Add(scene);
                        break;
                    }
                }
            }
        }
    }
}
