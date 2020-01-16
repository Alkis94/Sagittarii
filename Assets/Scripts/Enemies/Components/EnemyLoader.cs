using UnityEngine;
using System.Collections;

public class EnemyLoader : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private Sprite deadEnemySprite;
    private EnemyGotShot enemyGotShot;

    private bool dead = false;
    public int enemyKey;
    public string sceneKey;
    public MapType mapType;

    private void OnEnable()
    {
        enemyGotShot = GetComponentInChildren<EnemyGotShot>();
        enemyGotShot.OnDeath += SaveOnDeath;
    }

    private void OnDisable()
    {
        enemyGotShot.OnDeath -= SaveOnDeath;
    }

    // Use this for initialization
    void Start()
    {
        dead = ES3.Load<bool>("Dead" + enemyKey.ToString(), "Levels/" + mapType + sceneKey);
        transform.position = ES3.Load<Vector3>("Position" + enemyKey.ToString(), mapType + "/Scene" +sceneKey);

        if(dead)
        {
            spriteRenderer.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = deadEnemySprite;
            GetComponent<EnemyBrain>().enabled = false;
        }
    }

    private void SaveOnDeath()
    {
        ES3.Save<bool>("Dead" + enemyKey.ToString(), true, "Levels/" + mapType + sceneKey);
        ES3.Save<Vector3>("Position" + enemyKey.ToString(), transform.position, mapType + "/Scene" + sceneKey);
    }

    
}
