using UnityEngine;

public class EnemyWasHitDetector : MonoBehaviour
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
            enemyGotShot.Hit(collision.GetComponent<ProjectileDataInitializer>().Damage);
        }
    }
}
