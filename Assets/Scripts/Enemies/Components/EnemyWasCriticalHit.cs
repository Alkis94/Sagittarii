using UnityEngine;
using System;

public class EnemyWasCriticalHit : MonoBehaviour
{
    public event Action<int,Vector2> OnCriticalHit = delegate { };

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Arrow")
        {
            OnCriticalHit?.Invoke(collision.GetComponent<Projectile>().Damage, collision.GetComponent<PlayerProjectileImpact>().velocityOnHit);
        }
    }
}
