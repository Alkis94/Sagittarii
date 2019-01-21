using UnityEngine;
using System.Collections;

public class BatProjectile : Projectile
{

    // this projectile does not have a constant speed as of right now

    //Vector2 temp_force;
    void Start()
    {
        Destroy(gameObject, 20.0f);
        ProjectileSpeed = C.BAT_PROJECTILE_SPEED;
        rigidbody2d = GetComponent<Rigidbody2D>();
        transform.right = ((Vector2)GameObject.FindGameObjectWithTag("Player").transform.position - (Vector2)transform.position).normalized;
        rigidbody2d.AddForce((GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized * ProjectileSpeed);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        ProjectileImpact(0f);
    }
}
