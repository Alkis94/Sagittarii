using UnityEngine;

public class EnemyGroundMovement : MonoBehaviour
{

    private Rigidbody2D rigidbody2d;
    private Raycaster raycaster;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        raycaster = GetComponentInChildren<Raycaster>();
    }

    public void Move(float speed)
    {
        if(rigidbody2d.velocity.y == 0)
        {
            rigidbody2d.velocity = new Vector2(transform.right.x * speed, rigidbody2d.velocity.y);
        }
        raycaster.UpdateRaycastOrigins();
    }

    public void ChangeHorizontalDirection()
    {
        transform.localRotation = transform.localRotation.y == 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
    }


    //Can be called from attack animations like an MoveOnAttackPattern, only exception because it can be used for both attacks and movement.
    public void Jump()
    {
        int horizontalDirection = transform.localRotation.y == 0 ? 1 : -1;
        rigidbody2d.AddForce(new Vector2(4 * horizontalDirection, 5), ForceMode2D.Impulse);
    }

}