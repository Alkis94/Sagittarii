using UnityEngine;
using System;

public class EnemiesSerializer : MonoBehaviour
{

    public MapType MapType { get; set; }
    public string RoomKey { get; set; }

    public static event Action OnRoomHasAliveEnemies = delegate { };

    public void LoadEnemies()
    {
        int i = 0;
        foreach (Transform child in transform)
        {
            EnemyLoader enemyLoader = child.GetComponent<EnemyLoader>();
            enemyLoader.EnemyKey = i;
            enemyLoader.MapType = MapType;
            enemyLoader.RoomKey = RoomKey;
            i++;
        }

        CheckForAliveEnemies();
    }

    public void ReloadEnemies()
    {
        int i = 0;
        int childCount = transform.childCount;
        int jkey = 0;
        while (i < childCount)
        {
            EnemyLoader enemyLoader = transform.GetChild(i).GetComponent<EnemyLoader>();
            enemyLoader.EnemyKey = jkey;
            enemyLoader.MapType = MapType;
            enemyLoader.RoomKey = RoomKey;
            enemyLoader.Load();
            bool isEnemyDeadAlready = enemyLoader.IsDead();
            if (isEnemyDeadAlready)
            {
                childCount = transform.childCount;
            }
            else
            {
                i++;
            }
            jkey++;
        }

        CheckForAliveEnemies();
    }

    private void CheckForAliveEnemies()
    {
        if (transform.childCount > 0)
        {
            OnRoomHasAliveEnemies?.Invoke();
        }
    }

    //private void OnEnable()
    //{
    //    if (ES3.FileExists("Levels/" + MapType + "/Room" + RoomKey))
    //    {
    //        int i = 0;
    //        int childCount = transform.childCount;
    //        int jkey = 0;
    //        while(i < childCount)
    //        {
    //            EnemyLoader enemyLoader = transform.GetChild(i).GetComponent<EnemyLoader>();
    //            enemyLoader.EnemyKey = jkey;
    //            enemyLoader.MapType = MapType;
    //            enemyLoader.RoomKey = RoomKey;
    //            enemyLoader.Load();
    //            bool isEnemyDeadAlready = enemyLoader.IsDead();
    //            if (isEnemyDeadAlready)
    //            {
    //                childCount = transform.childCount;
    //            }
    //            else
    //            {
    //                i++;
    //            }
    //            jkey++;
    //        }
    //    }
    //    else
    //    {
    //        int i = 0;
    //        foreach (Transform child in transform)
    //        {
    //            EnemyLoader enemyLoader = child.GetComponent<EnemyLoader>();
    //            enemyLoader.EnemyKey = i;
    //            enemyLoader.MapType = MapType;
    //            enemyLoader.RoomKey = RoomKey;
    //            i++;
    //        }
    //    }

    //    if (transform.childCount > 0)
    //    {
    //        OnRoomHasAliveEnemies?.Invoke();
    //    }
    //}




}
