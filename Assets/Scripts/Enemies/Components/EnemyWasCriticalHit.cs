using UnityEngine;
using System;

public class EnemyWasCriticalHit : MonoBehaviour
{
    public event Action<int,Vector2> OnCriticalHit = delegate { };

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerProjectile")
        {
            OnCriticalHit?.Invoke(collision.GetComponent<ProjectileDataInitializer>().Damage, collision.GetComponent<ProjectileImpact>().velocityOnHit);
        }
    }
}
