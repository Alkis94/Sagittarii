using UnityEngine;
using System;

public class EnemiesManager : MonoBehaviour
{
    public static event Action OnRoomHasAliveEnemies = delegate { };
    private MapType mapType;
    private string roomKey;
    private RoomType roomType;
    private int chosenGroupID;
    private GameObject chosenGroup;

    private void Start()
    {
        mapType = MapManager.Instance.CurrentMap;
        roomKey = MapManager.Instance.CurrentMapCoords.x.ToString() + MapManager.Instance.CurrentMapCoords.y.ToString();
        roomType = MapManager.Instance.GetMapRoomType();

        ChooseGroup();

        if (ES3.FileExists("Levels/" + mapType + "/Room" + roomKey + "/Enemies"))
        {
            ReloadEnemies();
        }
        else
        {
            LoadEnemies();
        }
    }

    private void ChooseGroup()
    {
        //if(roomType == RoomType.bossRoom)
        //{
        //    if(ES3.FileExists("Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + mapType))
        //    {
        //        GameObject child = transform.GetChild(0).gameObject;
        //        EnemiesSerializer childSerializer = child.GetComponent<EnemiesSerializer>();
        //        childSerializer.MapType = mapType;
        //        childSerializer.RoomKey = roomKey;
        //        child.SetActive(true);
        //        childSerializer.ReloadEnemies();
        //    }
        //    else
        //    {
        //        GameObject child = transform.GetChild(0).gameObject;
        //        EnemiesSerializer childSerializer = child.GetComponent<EnemiesSerializer>();
        //        childSerializer.MapType = mapType;
        //        childSerializer.RoomKey = roomKey;
        //        child.SetActive(true);
        //        childSerializer.LoadEnemies();
        //    }
        //}

        if(roomType == RoomType.bossRoom)
        {
            if (ES3.FileExists("Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + mapType))
            {

            }
        }

        if (ES3.FileExists("Levels/" + mapType + "/Room" + roomKey + "/Enemies"))
        {
            chosenGroupID = ES3.Load<int>("ChosenGroupID", "Levels/" + mapType + "/Room" + roomKey + "/Enemies");
            chosenGroup = transform.GetChild(chosenGroupID).gameObject;
        }
        else
        {
            chosenGroupID = UnityEngine.Random.Range(0, transform.childCount);
            chosenGroup = transform.GetChild(chosenGroupID).gameObject;
        }
    }

    public void LoadEnemies()
    {
        int i = 0;
        foreach (Transform child in chosenGroup.transform)
        {
            EnemyLoader enemyLoader = child.GetComponent<EnemyLoader>();
            enemyLoader.EnemyKey = i;
            enemyLoader.MapType = mapType;
            enemyLoader.RoomKey = roomKey;
            i++;
        }
        chosenGroup.SetActive(true);
        CheckForAliveEnemies();
        Debug.Log("Enemies Loaded!");
    }

    public void ReloadEnemies()
    {
        int i = 0;
        int childCount = chosenGroup.transform.childCount;
        int jkey = 0;
        while (i < childCount)
        {
            EnemyLoader enemyLoader = chosenGroup.transform.GetChild(i).GetComponent<EnemyLoader>();
            enemyLoader.EnemyKey = jkey;
            enemyLoader.MapType = mapType;
            enemyLoader.RoomKey = roomKey;
            enemyLoader.Load();
            if (enemyLoader.IsDead())
            {
                childCount = chosenGroup.transform.childCount;
            }
            else
            {
                i++;
            }
            jkey++;
        }
        chosenGroup.SetActive(true);
        CheckForAliveEnemies();
        Debug.Log("Enemies Reloaded!");
    }

    private void CheckForAliveEnemies()
    {
        if (chosenGroup.transform.childCount > 0)
        {
            OnRoomHasAliveEnemies?.Invoke();
        }
    }

    private void OnDestroy()
    {
        ES3.Save<int>("ChosenGroupID", chosenGroupID, "Levels/" + mapType + "/Room" + roomKey + "/Enemies");
    }

}
