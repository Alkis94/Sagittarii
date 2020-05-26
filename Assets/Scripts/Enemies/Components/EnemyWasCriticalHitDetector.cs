using UnityEngine;
using System;

public class EnemyWasCriticalHitDetector : MonoBehaviour
{
    private EnemyGotShot enemyGotShot;

    private void Start()
    {
        enemyGotShot = GetComponentInParent<EnemyGotShot>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerProjectile")
        {
            enemyGotShot.CriticalHit(collision.GetComponent<ProjectileDataInitializer>().Damage, collision.GetComponent<ProjectileImpact>().velocityOnHit);
        }
    }
}
