using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStraightMovement : MonoBehaviour
{
    private Projectile projectile;
    private Rigidbody2D rigidbody2d;

    void Start()
    {
        projectile = GetComponent<Projectile>();
        Vector2 Direction = transform.right;
        rigidbody2d = GetComponent<Rigidbody2D>();
        rigidbody2d.velocity = new Vector2(projectile.Speed * Direction.x, projectile.Speed * Direction.y);
        Destroy(gameObject, projectile.DestroyDelay);
    }

}
