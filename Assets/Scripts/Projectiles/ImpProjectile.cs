using UnityEngine;
using System.Collections;

public class ImpProjectile : Projectile, IInitializableProjectile
{

    // Use this for initialization
    void Start ()
    {
        Destroy(gameObject, 20.0f);
        ProjectileSpeed = C.IMP_PROJECTILE_SPEED;
        rigidbody2d = GetComponent<Rigidbody2D>();
        ProjectileMove(HorizontalFactor,0);
    }
	

    public void Initialize(Transform impPosition, float horizontalFactor, float verticalFactor)
    {
        transform.position = impPosition.transform.position;
        HorizontalFactor = horizontalFactor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ProjectileImpact(0);
    }

}
