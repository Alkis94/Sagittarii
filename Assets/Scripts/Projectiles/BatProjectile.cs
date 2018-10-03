using UnityEngine;
using System.Collections;

public class BatProjectile : Projectile
{

    void Start()
    {
        Destroy(gameObject, 20.0f);
        ProjectileSpeed = C.BAT_PROJECTILE_SPEED;
        ProjectileRB2D = GetComponent<Rigidbody2D>();
        transform.right = GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;
        ProjectileRB2D.AddForce((GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized * ProjectileSpeed);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        ProjectileImpact(0f);
    }
}
