using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public class EnemiesManager : MonoBehaviour
{
    public static event Action OnRoomHasAliveEnemies = delegate { };
    private MapType mapType;
    private string roomKey;
    private RoomType roomType;
    private int chosenGroupID;
    private GameObject chosenGroup;
    private Spawner spawner;
    private Dictionary<string,GameObject> spawnerEnemies;

    private void Awake()
    {
        GameObject spawnerObject = GameObject.FindGameObjectWithTag("Spawner");
        if (spawnerObject != null)
        {
            spawner = spawnerObject.GetComponent<Spawner>();
            spawnerEnemies = new Dictionary<string, GameObject>();

            for (int i = 0; i < spawner.EnemySpawnInfos.Count; i++)
            {
                spawnerEnemies.Add(spawner.EnemySpawnInfos[i].enemy.name, spawner.EnemySpawnInfos[i].enemy);
            }
        }
    }

    private void Start()
    {
        mapType = MapManager.Instance.CurrentMap;
        roomKey = MapManager.Instance.CurrentMapCoords.x.ToString() + MapManager.Instance.CurrentMapCoords.y.ToString();
        roomType = MapManager.Instance.GetMapRoomType();
        ChooseAndLoad();
    }

    private void ChooseAndLoad()
    {
        if(roomType == RoomType.bossRoom)
        {
            chosenGroup = transform.GetChild(0).gameObject;

            if(ES3.KeyExists("Dead0", "Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + SceneManager.GetActiveScene().name))
            {
                bool dead = ES3.Load<bool>("Dead0", "Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + SceneManager.GetActiveScene().name);
                if(dead)
                {
                    ReloadEnemies();
                }
                else
                {
                    LoadEnemies();
                }
            }
            else
            {
                LoadEnemies();
            }
        }
        if (roomType == RoomType.spawnRoom)
        {
            if (ES3.FileExists("Levels/" + mapType + "/Room" + roomKey + "/Enemies"))
            {
                ReloadSpawnedEnemies();
            }
        }
        else
        {
            if (ES3.FileExists("Levels/" + mapType + "/Room" + roomKey + "/Enemies"))
            {
                chosenGroupID = ES3.Load<int>("ChosenGroupID", "Levels/" + mapType + "/Room" + roomKey + "/Enemies");
                chosenGroup = transform.GetChild(chosenGroupID).gameObject;
                ReloadEnemies();
            }
            else
            {
                chosenGroupID = UnityEngine.Random.Range(0, transform.childCount);
                chosenGroup = transform.GetChild(chosenGroupID).gameObject;
                LoadEnemies();
            }
        }
    }

    private void LoadEnemies()
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
    }

    private void ReloadEnemies()
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
    }

    private void ReloadSpawnedEnemies()
    {
        int counter = 0;
        while(true)
        {
            if(ES3.KeyExists("Dead" + counter, "Levels/" + mapType + "/Room" + roomKey + "/Enemies"))
            {
                string enemyName = ES3.Load<string>("EnemyName" + counter, "Levels/" + mapType + "/Room" + roomKey + "/Enemies");
                GameObject enemy = Instantiate(spawnerEnemies[enemyName]);
                EnemyLoader enemyLoader = enemy.GetComponent<EnemyLoader>();
                enemyLoader.EnemyKey = counter;
                enemyLoader.MapType = mapType;
                enemyLoader.RoomKey = roomKey;
                enemyLoader.Load();
                enemyLoader.IsDead();
            }
            else
            {
                break;
            }

            counter++;
        }
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
