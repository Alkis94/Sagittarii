using UnityEngine;
using System;

public class EnemyWasHitDetector : MonoBehaviour
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
            enemyGotShot.Hit(collision.GetComponent<ProjectileDataInitializer>().Damage);
        }
    }
}
