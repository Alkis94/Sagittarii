using System;
using UnityEngine;

public class EnemyGotShot : MonoBehaviour
{

    //public event Action<Transform> OnCriticalDeath = delegate { };
    public event Action<Transform,bool> OnDeath = delegate { };
    private EnemyData enemyData;
    private CircleCollider2D circleCollider2D;

    private void Start()
    {
        enemyData = GetComponentInParent<EnemyData>();
        circleCollider2D = GetComponentInChildren<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Arrow")
        {
            if (enemyData.damageable)
            {
                bool criticalHit = false;

                if (other.GetComponent<PlayerProjectileImpact>().criticalHit)
                {
                    enemyData.health -= other.GetComponent<Projectile>().Damage * 3;
                    criticalHit = true;
                }
                else
                {
                    enemyData.health -= other.GetComponent<Projectile>().Damage;
                }

                if (enemyData.health <= 0)
                {
                    OnDeath?.Invoke(other.transform,criticalHit);
                }

            }

        }
    }
}
