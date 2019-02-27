using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovement : MovementPattern
{

    private Rigidbody2D rigidbody2d;

    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public override void Move(float speed,int verticalDirection)
    {
        rigidbody2d.velocity = new Vector2(transform.right.x * speed, 0);
    }

}
