using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementPattern : MonoBehaviour
{
    abstract public void Move(float speed);
}
