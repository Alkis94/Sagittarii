using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private List<EnemySpawnInfo> enemySpawnInfos;
    private List<Transform> groundSpawnPoints;
    private List<FlyingSpawnPoint> flyingSpawnPoints;
    [SerializeField]
    private int EnemySpawnCount = 50;


    private void Awake()
    {
        groundSpawnPoints = new List<Transform>();
        flyingSpawnPoints = new List<FlyingSpawnPoint>();

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
        

        for(int i = 0; i < enemySpawnInfos.Count; i++)
        {
            if (enemySpawnInfos[i].enemyType == EnemyType.Ground)
            {
                StartCoroutine(SpawnGroundEnemy(enemySpawnInfos[i].enemy, enemySpawnInfos[i].enemySpawnFrequency));
            }
            else if (enemySpawnInfos[i].enemyType == EnemyType.Flying)
            {
                StartCoroutine(SpawnFlyingEnemy(enemySpawnInfos[i].enemy, enemySpawnInfos[i].enemySpawnFrequency));
            }
        }
    }


    IEnumerator SpawnGroundEnemy(GameObject enemy,float enemySpawnFrequency)
    {
        while (true)
        {
            int randomNumber = Random.Range(0, groundSpawnPoints.Count);
            Instantiate(enemy, groundSpawnPoints[randomNumber].position,Quaternion.identity);
            Debug.Log("Enemy Spawned At :" +enemy.transform.position);
            Debug.Log("Should Spawned At :" +groundSpawnPoints[randomNumber].position);
            yield return new WaitForSeconds(enemySpawnFrequency);
        }
    }

    IEnumerator SpawnFlyingEnemy(GameObject enemy, float enemySpawnFrequency)
    {
        while (true)
        {
            int randomNumber = Random.Range(0, flyingSpawnPoints.Count);
            Vector3 spawnPosition = new Vector3(Random.Range(flyingSpawnPoints[randomNumber].bottomLeftCorner.position.x, flyingSpawnPoints[randomNumber].upRightCorner.position.x),
                Random.Range(flyingSpawnPoints[randomNumber].bottomLeftCorner.position.y, flyingSpawnPoints[randomNumber].upRightCorner.position.y),0);
            Instantiate(enemy, spawnPosition,Quaternion.identity);
            yield return new WaitForSeconds(enemySpawnFrequency);
        }
    }
}

