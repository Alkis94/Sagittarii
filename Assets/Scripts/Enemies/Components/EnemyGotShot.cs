using System;
using UnityEngine;

public class EnemyGotShot : MonoBehaviour
{

    //public event Action<Transform> OnCriticalDeath = delegate { };
    public event Action<bool> OnDeath = delegate { };
    private EnemyData enemyData;
    private EnemyCriticalHit enemyCriticalHit;
    private bool criticalHit = false;
    [SerializeField]
    private GameObject amputationPart;
    

    private void Start()
    {
        enemyData = GetComponent<EnemyData>();
        enemyCriticalHit = GetComponentInChildren<EnemyCriticalHit>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Arrow")
        {
            if (enemyData.damageable)
            {
                if (enemyCriticalHit != null)
                {
                    criticalHit = enemyCriticalHit.criticalHit;
                }

                if (criticalHit)
                {
                    enemyData.health -= other.GetComponent<Projectile>().Damage * 3;
                }
                else
                {
                    enemyData.health -= other.GetComponent<Projectile>().Damage;
                }

                if (enemyData.health <= 0)
                {
                    OnDeath?.Invoke(criticalHit);

                    if(criticalHit && enemyData.amputation)
                    {
                        amputationPart.SetActive(true);
                        amputationPart.GetComponent<Rigidbody2D>().AddForce(other.GetComponent<PlayerProjectileImpact>().velocityOnHit / 2, ForceMode2D.Impulse);
                    }
                    
                }

            }

        }
    }
}
