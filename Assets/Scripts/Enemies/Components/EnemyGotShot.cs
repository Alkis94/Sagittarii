using System;
using UnityEngine;

public class EnemyGotShot : MonoBehaviour
{

    public static event Action<int> OnEnemyDeathGiveGold = delegate { };
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
            enemyData.health -= other.GetComponent<Projectile>().Damage;
            if (enemyData.health <= 0)
            {
                OnEnemyDeathGiveGold?.Invoke(enemyData.goldGiven);
                OnDeath?.Invoke();
                //Destroy(gameObject);
            }
        }
    }
}
