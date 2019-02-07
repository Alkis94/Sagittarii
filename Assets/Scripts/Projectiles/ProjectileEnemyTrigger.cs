using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemyTrigger : MonoBehaviour
{
    [SerializeField]
    private float ImpactDestroyDelay = 0;
    private Rigidbody2D rigidbody2d;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Background")
        {
            rigidbody2d.velocity = Vector2.zero;
            rigidbody2d.angularVelocity = 0;
            rigidbody2d.isKinematic = true;
            enabled = false;
            Destroy(gameObject, ImpactDestroyDelay);
        }
    }
}
