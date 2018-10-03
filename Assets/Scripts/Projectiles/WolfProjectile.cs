using UnityEngine;
using System.Collections;

public class WolfProjectile : Projectile , IInitializableProjectile
{

	// Use this for initialization
	void Start ()
    {
        Destroy(gameObject, 0.8f);
        ProjectileSpeed = C.WOLF_PROJECTILE_SPEED;
        ProjectileRB2D = GetComponent<Rigidbody2D>();
        ProjectileMove(HorizontalFactor, VerticalFactor);
    }

    public void Initialize(Transform wolfPosition, float horizontalFactor, float verticalFactor)
    {
        transform.position = wolfPosition.transform.position + new Vector3(2*horizontalFactor, 0, 0) ;
        HorizontalFactor = horizontalFactor;
        VerticalFactor = verticalFactor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ProjectileImpact(0);
    }
}
