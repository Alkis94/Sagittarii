using UnityEngine;

public class EnemyWasCriticalHitDetector : MonoBehaviour
{
    private EnemyGotShot enemyGotShot;

    private void Start()
    {
        enemyGotShot = GetComponentInParent<EnemyGotShot>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerProjectile"))
        {
            enemyGotShot.CriticalHit(collision.GetComponent<ProjectileDataInitializer>().Damage, collision.GetComponent<ProjectileHandler>().velocityOnHit);
        }
    }
}
