using System;
using UnityEngine;

public class EnemyGotShot : MonoBehaviour
{

    public static event Action OnDeathNotifyUI = delegate { };
    public event Action OnDeath = delegate { };
    private EnemyData enemyData;


    private void Start()
    {
        enemyData = GetComponentInParent<EnemyData>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Arrow")
        {
            enemyData.Health -= PlayerStats.Damage;
            if (enemyData.Health <= 0)
            {
                PlayerStats.Gold += enemyData.GoldGiven;
                OnDeathNotifyUI?.Invoke();
                OnDeath?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
