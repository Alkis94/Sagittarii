using UnityEngine;
using System.Collections;

public class MedusaProjectile : Projectile, IInitializableProjectile
{

    // Use this for initialization
    void Start ()
    {
        Destroy(gameObject, 20.0f);
        ProjectileRB2D = GetComponent<Rigidbody2D>();
        ProjectileSpeed = C.MEDUSA_PROJECTILE_SPEED;
        ProjectileMove(HorizontalFactor, -1);
    }

    public void Initialize(Transform medusaPosition, float horizontalFactor, float verticalFactor)
    {
        transform.position = medusaPosition.transform.position;
        HorizontalFactor = horizontalFactor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ProjectileImpact(0f);
    }
}
