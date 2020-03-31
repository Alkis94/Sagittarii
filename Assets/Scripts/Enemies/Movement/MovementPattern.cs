using UnityEngine;

public abstract class MovementPattern : MonoBehaviour
{
    abstract public void Move(float speed, int verticalDirection, int horizontalDirection = 1);
}
