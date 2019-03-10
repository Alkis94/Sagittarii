using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo
{

    public GameObject enemy;
    public float enemySpawnFrequency;
    public EnemyType enemyType;

    public EnemySpawnInfo(GameObject enemy, float enemySpawnFrequency, EnemyType enemyType)
    {
        this.enemy = enemy;
        this.enemySpawnFrequency = enemySpawnFrequency;
        this.enemyType = enemyType;
    }
}

