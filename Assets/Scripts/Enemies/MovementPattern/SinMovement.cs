using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinMovement : MovementPattern
{
    private float timePassed = 0;
    private float verticalDirection = 0;
    private Rigidbody2D rigidbody2d;

    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public override void Move(float speed, int brainVerticalDirection)
    {
        //Needs some more testing
        timePassed += Time.deltaTime;
        verticalDirection = Mathf.Sin(timePassed);
        float velocityX = transform.right.x * speed;
        float velocityY = transform.up.y * speed * verticalDirection * brainVerticalDirection;
        rigidbody2d.velocity = new Vector2(velocityX, velocityY);
        //Debug.Log("vertical : " + verticalDirection + "brainVertical : " + brainVerticalDirection);
    }


}
