using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using System;

public class Spawner : SerializedMonoBehaviour
{
    public event Action OnSpawnerFinished = delegate { };

    [SerializeField]
    private Transform enemiesParent;
    private float timeLimit;
    private int amountOfEnemySpawns;
    private MapType mapType;
    private string roomKey;
    private int keyCounter;

    [Header("Spawner Configuration")]
    [SerializeField]
    private float minTimeLimit = 60f;
    [SerializeField]
    private float maxTimeLimit = 120f;
    [SerializeField]
    private int minAmountOfEnemySpawns = 3;
    [SerializeField]
    private int maxAmountOfEnemySpawns = 5;

    [OdinSerialize] public List<EnemySpawnInfo> EnemySpawnInfos { get; private set; }
    private List<EnemySpawnInfo> enemiesChosenToSpawn;
    private List<Transform> groundSpawnPoints;
    private List<FlyingSpawnPoint> flyingSpawnPoints;
    
    private void Awake()
    {
        groundSpawnPoints = new List<Transform>();
        flyingSpawnPoints = new List<FlyingSpawnPoint>();
        enemiesChosenToSpawn = new List<EnemySpawnInfo>();
        mapType = MapManager.Instance.CurrentMap;
        roomKey = MapManager.Instance.CurrentMapCoords.x.ToString() + MapManager.Instance.CurrentMapCoords.y.ToString();

        foreach(Transform spawnPoint in transform)
        {
            if(spawnPoint.tag == "GroundSpawnPoint")
            {
                groundSpawnPoints.Add(spawnPoint.transform);
            }
            else if (spawnPoint.tag == "FlyingSpawnPoint")
            {
                FlyingSpawnPoint flyingSpawnPoint = spawnPoint.GetComponent<FlyingSpawnPoint>();
                flyingSpawnPoints.Add(flyingSpawnPoint);
            }
        }
    }


    void Start()
    {
        if (!ES3.FileExists("Levels/" + mapType + "/Room" + roomKey + "/Enemies"))
        {
            enemiesParent.gameObject.SetActive(true);
            amountOfEnemySpawns = UnityEngine.Random.Range(minAmountOfEnemySpawns, maxAmountOfEnemySpawns + 1);

            for (int i = 0; i < amountOfEnemySpawns; i++)
            {
                int randomNumber = UnityEngine.Random.Range(0, EnemySpawnInfos.Count);
                enemiesChosenToSpawn.Add(EnemySpawnInfos[randomNumber]);
            }

            for (int i = 0; i < enemiesChosenToSpawn.Count; i++)
            {
                if (enemiesChosenToSpawn[i].enemyType == EnemyType.ground)
                {
                    StartCoroutine(SpawnGroundEnemy(enemiesChosenToSpawn[i].enemy, enemiesChosenToSpawn[i].enemySpawnFrequency));
                }
                else if (enemiesChosenToSpawn[i].enemyType == EnemyType.flying)
                {
                    StartCoroutine(SpawnFlyingEnemy(enemiesChosenToSpawn[i].enemy, enemiesChosenToSpawn[i].enemySpawnFrequency));
                }
            }

            timeLimit = Time.time + UnityEngine.Random.Range(minTimeLimit, maxTimeLimit);
            StartCoroutine(StopAfterTimeLimit());
        }
    }

    IEnumerator StopAfterTimeLimit()
    {
        while (true)
        {
            if (Time.time > timeLimit)
            {
                StopAllCoroutines();
                OnSpawnerFinished?.Invoke();
            }
            yield return  null;
        }
    }


    IEnumerator SpawnGroundEnemy(GameObject enemy,float enemySpawnFrequency)
    {
        float randomDelay = UnityEngine.Random.Range(0f, 10f);
        yield return new WaitForSeconds(randomDelay);

        while (true)
        {
            int randomNumber = UnityEngine.Random.Range(0, groundSpawnPoints.Count);
            var someEnemy = Instantiate(enemy, groundSpawnPoints[randomNumber].position,Quaternion.identity, enemiesParent);
            EnemyLoader enemyLoader = someEnemy.GetComponent<EnemyLoader>();
            enemyLoader.EnemyKey = keyCounter;
            enemyLoader.RoomKey = roomKey;
            enemyLoader.MapType = mapType;
            keyCounter++;
            yield return new WaitForSeconds(enemySpawnFrequency);
        }
    }

    IEnumerator SpawnFlyingEnemy(GameObject enemy, float enemySpawnFrequency)
    {
        float randomDelay = UnityEngine.Random.Range(0f, 10f);
        yield return new WaitForSeconds(randomDelay);

        while (true)
        {
            int randomNumber = UnityEngine.Random.Range(0, flyingSpawnPoints.Count);
            Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(flyingSpawnPoints[randomNumber].MinBoundX, flyingSpawnPoints[randomNumber].MaxBoundX),
                                                UnityEngine.Random.Range(flyingSpawnPoints[randomNumber].MinBoundY, flyingSpawnPoints[randomNumber].MaxBoundY), 0);
            var someEnemy = Instantiate(enemy, spawnPosition,Quaternion.identity, enemiesParent);
            EnemyLoader enemyLoader = someEnemy.GetComponent<EnemyLoader>();
            enemyLoader.EnemyKey = keyCounter;
            enemyLoader.RoomKey = roomKey;
            enemyLoader.MapType = mapType;
            keyCounter++;
            yield return new WaitForSeconds(enemySpawnFrequency);
        }
    }
}

