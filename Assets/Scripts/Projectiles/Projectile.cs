using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    protected Rigidbody2D rigidbody2d;

    protected int ProjectileSpeed;
    protected float HorizontalFactor;
    protected float VerticalFactor;

    protected void ProjectileMove(float horizontalFactor, float verticalFactor)
    {
        rigidbody2d.AddForce(transform.right * ProjectileSpeed * horizontalFactor);
        rigidbody2d.AddForce(transform.up * ProjectileSpeed * verticalFactor);
    }

    protected virtual void ProjectileImpact (float destroyDelay)
    {
        rigidbody2d.velocity = Vector2.zero;
        rigidbody2d.angularVelocity = 0;
        rigidbody2d.isKinematic = true;
        enabled = false;
        Destroy(gameObject,destroyDelay);
    }
}
