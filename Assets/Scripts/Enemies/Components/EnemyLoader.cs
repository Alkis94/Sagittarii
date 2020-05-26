using UnityEngine;
using System;

public class EnemyLoader : MonoBehaviour
{
    [HideInInspector]
    public int enemyKey;
    [HideInInspector]
    public string roomKey;
    [HideInInspector]
    public MapType mapType;

    [SerializeField]
    private Sprite deadEnemySprite;
    [SerializeField]
    private Sprite criticalDeathEnemySprite;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody2d;
    private EnemyGotShot enemyGotShot;
    private bool dead = false;
    private bool criticalDeath = false;


    public void LoadEnemy()
    {

        Vector3 originalPosition = transform.position;
        dead = ES3.Load<bool>("Dead" + enemyKey.ToString(), "Levels/" + mapType + "/Room" + roomKey);
        criticalDeath = ES3.Load<bool>("CriticalDeath" + enemyKey.ToString(), "Levels/" + mapType + "/Room" + roomKey);
        transform.position = ES3.Load<Vector3>("Position" + enemyKey.ToString(), "Levels/" + mapType + "/Room" + roomKey);
        transform.rotation = ES3.Load<Quaternion>("Rotation" + enemyKey.ToString(), "Levels/" + mapType + "/Room" + roomKey);
        GetComponent<EnemyBrain>().LoadEnemyBrain(originalPosition, dead);

        if (dead)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            rigidbody2d = GetComponent<Rigidbody2D>();
            enemyGotShot = GetComponent<EnemyGotShot>();
            animator.enabled = false;
            if (criticalDeath)
            {
                spriteRenderer.sprite = criticalDeathEnemySprite;
            }
            else
            {
                spriteRenderer.sprite = deadEnemySprite;
            }

            GetComponent<EnemyBrain>().enabled = false;
            rigidbody2d.gravityScale = 1;
            spriteRenderer.sortingLayerName = "DeadEnemies";
            gameObject.layer = 14;
            enemyGotShot.enabled = false;
            foreach (Transform trans in GetComponentsInChildren<Transform>(true))
            {
                trans.gameObject.layer = 14;
            }
        }
    }

    public void ChangeEnemyStatusForSave(bool criticalDeath)
    {
        dead = true;
        this.criticalDeath = criticalDeath;
    }

    private void OnDestroy()
    {
        ES3.Save<bool>("Dead" + enemyKey.ToString(), dead, "Levels/" + mapType + "/Room" + roomKey);
        ES3.Save<bool>("CriticalDeath" + enemyKey.ToString(), criticalDeath, "Levels/" + mapType + "/Room" + roomKey);
        ES3.Save<Vector3>("Position" + enemyKey.ToString(), transform.position, "Levels/" + mapType + "/Room" + roomKey);
        ES3.Save<Quaternion>("Rotation" + enemyKey.ToString(), transform.rotation, "Levels/" + mapType + "/Room" + roomKey);
    }

    
}
