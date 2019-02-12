using System;
using UnityEngine;

public class EnemyGotShot : MonoBehaviour
{

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
            if (enemyData.Health < 1)
            {
                OnDeath?.Invoke();
                Destroy(gameObject);
            }
        }
    }

}
