using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovement : MovementPattern
{

    private Rigidbody2D rigidbody2d;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }


    public override void Move(float speed,int verticalDirection,int horizontalDirection)
    {
        rigidbody2d.velocity = new Vector2(transform.right.x * speed * horizontalDirection, 0);
    }

}
