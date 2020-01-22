using UnityEngine;
using System.Collections;

public class EnemiesSerializer : MonoBehaviour
{
    [HideInInspector]
    public MapType mapType;
    [HideInInspector]
    public string roomKey;

    private void Start()
    {
        int i = 0;
        foreach(Transform child in transform)
        {
            EnemyLoader enemyLoader = child.GetComponent<EnemyLoader>();
            enemyLoader.enemyKey = i;
            enemyLoader.mapType = mapType;
            enemyLoader.roomKey = roomKey;
            enemyLoader.LoadEnemy();
            i += 1;
        }
    }
}
