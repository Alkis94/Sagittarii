using UnityEngine;
using System.Collections;

public class FlyupFreefallMovement : MovementPattern
{
    private float fallTime = 0;
    private Rigidbody2D rigidbody2d;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public override void Move(float speed, int brainVerticalDirection, int horizontalDirection)
    {
        if(Time.time > fallTime)
        {
            rigidbody2d.gravityScale = 1;
        }
        else
        {
            rigidbody2d.velocity = new Vector2(transform.right.x * speed * horizontalDirection, transform.right.y * speed * brainVerticalDirection);
            rigidbody2d.gravityScale = 0;
        }
    }
}
