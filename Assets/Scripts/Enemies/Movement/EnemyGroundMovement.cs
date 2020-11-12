using UnityEngine;

public class EnemyGroundMovement : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Move(float speed)
    {
        if(rigidbody2d.velocity.y == 0)
        {
            rigidbody2d.velocity = new Vector2(transform.right.x * speed, rigidbody2d.velocity.y);
        }
    }

    public void Jump(float horizontalForce, float verticalForce)
    {
        int horizontalDirection = transform.localRotation.y == 0 ? 1 : -1;
        rigidbody2d.AddForce(new Vector2(horizontalForce * horizontalDirection, verticalForce), ForceMode2D.Impulse);
    }

}