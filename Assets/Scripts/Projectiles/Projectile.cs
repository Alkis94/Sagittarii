using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    protected Rigidbody2D ProjectileRB2D;

    protected int ProjectileSpeed;
    protected float HorizontalFactor;
    protected float VerticalFactor;

    protected void ProjectileMove(float horizontalFactor, float verticalFactor)
    {
        ProjectileRB2D.AddForce(transform.right * ProjectileSpeed * horizontalFactor);
        ProjectileRB2D.AddForce(transform.up * ProjectileSpeed * verticalFactor);
    }

    protected virtual void ProjectileImpact (float destroyDelay)
    {
        ProjectileRB2D.velocity = Vector2.zero;
        ProjectileRB2D.angularVelocity = 0;
        ProjectileRB2D.isKinematic = true;
        enabled = false;
        DestroyObject(gameObject,destroyDelay);
    }
}
