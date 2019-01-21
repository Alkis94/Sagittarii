using UnityEngine;
using System.Collections;

public class CrowProjectile : Projectile, IInitializableProjectile
{
    private Animator CrowProjectileAnimator;


    void Start()
    {
        Destroy(gameObject, 20.0f);
        rigidbody2d = GetComponent<Rigidbody2D>();
        CrowProjectileAnimator = GetComponent<Animator>();
        ProjectileSpeed = C.CROW_PROJECTILE_SPEED;
        ProjectileMove(HorizontalFactor,-1);   
    }

    public void Initialize(Transform crowPosition, float horizontalFactor, float verticalFactor)
    {
        transform.position = crowPosition.transform.position;
        HorizontalFactor = horizontalFactor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ProjectileImpact(0.25f);
        CrowProjectileAnimator.SetTrigger("Impact");
    }
}
