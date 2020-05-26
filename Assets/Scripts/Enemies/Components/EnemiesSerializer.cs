using UnityEngine;

public class EnemiesSerializer : MonoBehaviour
{

    public MapType MapType { get; set; }
    public string RoomKey { get; set; }

    private void OnEnable()
    {
        if (ES3.FileExists("Levels/" + MapType + "/Room" + RoomKey))
        {
            int i = 0;
            foreach (Transform child in transform)
            {
                EnemyLoader enemyLoader = child.GetComponent<EnemyLoader>();
                enemyLoader.enemyKey = i;
                enemyLoader.mapType = MapType;
                enemyLoader.roomKey = RoomKey;
                enemyLoader.LoadEnemy();
                i++;
            }
        }
        else
        {
            int i = 0;
            foreach (Transform child in transform)
            {
                EnemyLoader enemyLoader = child.GetComponent<EnemyLoader>();
                enemyLoader.enemyKey = i;
                enemyLoader.mapType = MapType;
                enemyLoader.roomKey = RoomKey;
                i++;
            }
        }
    }
}
