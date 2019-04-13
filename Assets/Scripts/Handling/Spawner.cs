using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spawner : MonoBehaviour
{

    public event Action OnSpawnerFinished = delegate { };

    private Transform enemiesParent;

    private float timeLimit;
    private int amountOfEnemySpawns;

    [Header("Spawner Configuration")]
    [SerializeField]
    private float minTimeLimit = 60f;
    [SerializeField]
    private float maxTimeLimit = 120f;
    [SerializeField]
    private int maxAmountOfEnemySpawns = 5;
    [SerializeField]
    private int minAmountOfEnemySpawns = 3;

    [Header("Possible Enemy Spawns")]
    [SerializeField]
    private List<EnemySpawnInfo> enemySpawnInfos;
    private List<EnemySpawnInfo> enemiesChosenToSpawn;
    private List<Transform> groundSpawnPoints;
    private List<FlyingSpawnPoint> flyingSpawnPoints;
    


    private void Awake()
    {
        groundSpawnPoints = new List<Transform>();
        flyingSpawnPoints = new List<FlyingSpawnPoint>();
        enemiesChosenToSpawn = new List<EnemySpawnInfo>();
        enemiesParent = GameObject.FindGameObjectWithTag("Enemies").GetComponent<Transform>();

        foreach (GameObject groundSpawnPoint in GameObject.FindGameObjectsWithTag("GroundSpawnPoint"))
        {
            groundSpawnPoints.Add(groundSpawnPoint.transform);
        }

        foreach (GameObject flyingSpawnPoint in GameObject.FindGameObjectsWithTag("FlyingSpawnPoint"))
        {
            FlyingSpawnPoint someFlyingSpawnPoint = flyingSpawnPoint.GetComponent<FlyingSpawnPoint>();
            flyingSpawnPoints.Add(someFlyingSpawnPoint);
        }
    }


    void Start()
    {
        amountOfEnemySpawns = UnityEngine.Random.Range(minAmountOfEnemySpawns, maxAmountOfEnemySpawns + 1);

        for (int i = 0; i < amountOfEnemySpawns; i++)
        {
            int randomNumber = UnityEngine.Random.Range(0, enemySpawnInfos.Count);
            enemiesChosenToSpawn.Add(enemySpawnInfos[randomNumber]);
            //Debug.Log("Enemy Chosen: " + enemySpawnInfos[randomNumber].enemy);
        }

        for (int i = 0; i < enemiesChosenToSpawn.Count; i++)
        {
            if (enemiesChosenToSpawn[i].enemyType == EnemyType.Ground)
            {
                StartCoroutine(SpawnGroundEnemy(enemiesChosenToSpawn[i].enemy, enemiesChosenToSpawn[i].enemySpawnFrequency));
            }
            else if (enemiesChosenToSpawn[i].enemyType == EnemyType.Flying)
            {
                StartCoroutine(SpawnFlyingEnemy(enemiesChosenToSpawn[i].enemy, enemiesChosenToSpawn[i].enemySpawnFrequency));
            }
        }

        timeLimit = Time.time + UnityEngine.Random.Range(minTimeLimit, maxTimeLimit);
        StartCoroutine(StopAfterTimeLimit());
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
            Instantiate(enemy, groundSpawnPoints[randomNumber].position,Quaternion.identity, enemiesParent);
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
                                                UnityEngine.Random.Range(flyingSpawnPoints[randomNumber].MinBoundY, flyingSpawnPoints[randomNumber].MaxBoundY),0);
            Instantiate(enemy, spawnPosition,Quaternion.identity, enemiesParent);
            yield return new WaitForSeconds(enemySpawnFrequency);
        }
    }
}

