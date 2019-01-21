using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinMovement : MovementPattern
{
    private float TimePassed = 0;
    private float VerticalDirection = 0;

    public override void Move(float horizontalDirection,float speed)
    {
        TimePassed += Time.deltaTime;
        VerticalDirection = Mathf.Sin(TimePassed);
        Vector3 Movement = new Vector3(horizontalDirection, VerticalDirection, 0);
        transform.Translate(Movement * Time.deltaTime * speed);
    }


}
