using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovement : MovementPattern
{

    public override void Move(float horizontalDirection,float speed)
    {
        Vector3 Movement = new Vector3(horizontalDirection, 0, 0);
        transform.Translate(Movement * Time.deltaTime * speed);
    }

}
