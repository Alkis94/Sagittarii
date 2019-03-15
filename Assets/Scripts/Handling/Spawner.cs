using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    
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
        amountOfEnemySpawns = Random.Range(minAmountOfEnemySpawns, maxAmountOfEnemySpawns + 1);

        for (int i = 0; i < amountOfEnemySpawns; i++)
        {
            int randomNumber = Random.Range(0, enemySpawnInfos.Count);
            enemiesChosenToSpawn.Add(enemySpawnInfos[randomNumber]);
            Debug.Log("Enemy chosen : " + enemySpawnInfos[randomNumber].enemy.name);
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

        timeLimit = Time.time + Random.Range(minTimeLimit, maxTimeLimit);
        StartCoroutine(StopAfterTimeLimit());
    }

    IEnumerator StopAfterTimeLimit()
    {
        while (true)
        {
            if (Time.time > timeLimit)
            {
                Debug.Log("Cororoutines Stopped");
                StopAllCoroutines();
            }
            yield return  null;
        }
    }


    IEnumerator SpawnGroundEnemy(GameObject enemy,float enemySpawnFrequency)
    {
        float randomDelay = Random.Range(0f, 10f);
        yield return new WaitForSeconds(randomDelay);

        while (true)
        {
            int randomNumber = Random.Range(0, groundSpawnPoints.Count);
            Instantiate(enemy, groundSpawnPoints[randomNumber].position,Quaternion.identity);
            yield return new WaitForSeconds(enemySpawnFrequency);
        }
    }

    IEnumerator SpawnFlyingEnemy(GameObject enemy, float enemySpawnFrequency)
    {
        float randomDelay = Random.Range(0f, 10f);
        yield return new WaitForSeconds(randomDelay);

        while (true)
        {
            int randomNumber = Random.Range(0, flyingSpawnPoints.Count);
            Vector3 spawnPosition = new Vector3(Random.Range(flyingSpawnPoints[randomNumber].MinBoundX, flyingSpawnPoints[randomNumber].MaxBoundX),
                                                Random.Range(flyingSpawnPoints[randomNumber].MinBoundY, flyingSpawnPoints[randomNumber].MaxBoundY),0);
            Instantiate(enemy, spawnPosition,Quaternion.identity);
            yield return new WaitForSeconds(enemySpawnFrequency);
        }
    }
}

