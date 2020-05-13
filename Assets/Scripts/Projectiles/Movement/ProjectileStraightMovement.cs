using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStraightMovement : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;

    void Start()
    {
        float speed = GetComponent<ProjectileDataInitializer>().Speed;
        Vector2 Direction = transform.right;
        rigidbody2d = GetComponent<Rigidbody2D>();
        rigidbody2d.velocity = new Vector2(speed * Direction.x, speed * Direction.y);
    }

}
