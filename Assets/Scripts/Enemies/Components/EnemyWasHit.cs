using UnityEngine;
using System;

public class EnemyWasHit : MonoBehaviour
{
    public event Action<int> OnHit = delegate { };

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Arrow")
        {
            OnHit?.Invoke(collision.GetComponent<Projectile>().Damage);
        }
    }
}
