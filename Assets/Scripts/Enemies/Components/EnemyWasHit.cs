using UnityEngine;
using System;

public class EnemyWasHit : MonoBehaviour
{
    public event Action<int> OnHit = delegate { };

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerProjectile")
        {
            OnHit?.Invoke(collision.GetComponent<ProjectileDataInitializer>().Damage);
        }
    }
}
