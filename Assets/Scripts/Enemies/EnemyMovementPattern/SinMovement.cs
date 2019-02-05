using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinMovement : MovementPattern
{
    private float TimePassed = 0;
    private float VerticalDirection = 0;

    public override void Move(float speed)
    {
        TimePassed += Time.deltaTime;
        VerticalDirection = Mathf.Sin(TimePassed);
        transform.Translate(transform.right * Time.deltaTime * speed,Space.World);
        transform.Translate(transform.up * Time.deltaTime * speed * VerticalDirection, Space.World);
    }


}
