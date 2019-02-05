using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovement : MovementPattern
{

    public override void Move(float speed)
    {
        transform.Translate( transform.right * Time.deltaTime * speed, Space.World);   
    }

}
