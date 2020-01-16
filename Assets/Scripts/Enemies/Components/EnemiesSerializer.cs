using UnityEngine;
using System.Collections;

public class EnemiesSerializer : MonoBehaviour
{

    public MapType mapType;
    public string sceneKey;

    private void Awake()
    {
        int i = 0;
        foreach(GameObject gameObject in transform)
        {
            EnemyLoader enemyLoader = gameObject.GetComponent<EnemyLoader>();
            enemyLoader.enemyKey = i;
            enemyLoader.mapType = mapType;
            enemyLoader.sceneKey = sceneKey;
            i += 1;
        }
    }
}
