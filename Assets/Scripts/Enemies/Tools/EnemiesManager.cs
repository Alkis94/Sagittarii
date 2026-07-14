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
        mapType = MapManager.Instance.CurrentMapInfo.Type;
        roomKey = MapManager.Instance.CurrentMapInfo.Coords.x.ToString() + MapManager.Instance.CurrentMapInfo.Coords.y.ToString();
        roomType = MapManager.Instance.CurrentMapInfo.Map[MapManager.Instance.CurrentMapInfo.Coords.x, MapManager.Instance.CurrentMapInfo.Coords.y].Room.Type;
        ChooseAndLoad();
    }

    private void ChooseAndLoad()
    {
        if(roomType == RoomType.BossRoom)
        {
            chosenGroup = transform.GetChild(0).gameObject;

            if(ES3.KeyExists("Dead0", SaveManager.Instance.GetProfileRunPath() + SaveFolders.Bosses + "/" + SceneManager.GetActiveScene().name))
            {
                bool dead = ES3.Load<bool>("Dead0", SaveManager.Instance.GetProfileRunPath() + SaveFolders.Bosses + "/" + SceneManager.GetActiveScene().name);
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
        else
        {
            if (ES3.FileExists(SaveManager.Instance.GetProfileRunPath() + SaveFolders.Levels + "/" + mapType + SaveFolders.Room + roomKey + SaveFolders.Enemies))
            {
                chosenGroupID = ES3.Load<int>("ChosenGroupID", SaveManager.Instance.GetProfileRunPath() + SaveFolders.Levels + "/" + mapType + SaveFolders.Room + roomKey + SaveFolders.Enemies);
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
        var i = 0;
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
        var i = 0;
        var childCount = chosenGroup.transform.childCount;
        var jkey = 0;
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

    private void CheckForAliveEnemies()
    {
        if (chosenGroup.transform.childCount > 0)
        {
            OnRoomHasAliveEnemies?.Invoke();
        }
    }

    private void OnDestroy()
    {
        ES3.Save("ChosenGroupID", chosenGroupID, SaveManager.Instance.GetProfileRunPath() + SaveFolders.Levels + "/" + mapType + SaveFolders.Room + roomKey + SaveFolders.Enemies);
    }
}
