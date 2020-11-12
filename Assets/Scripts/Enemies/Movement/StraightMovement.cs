
using UnityEngine;

public class StraightMovement : MovementPattern
{
    private Rigidbody2D rigidbody2d;

    [SerializeField]
    private float rotation = 0;
    

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        transform.localRotation = Quaternion.Euler(0f, 0f, rotation);
    }

    public override void Move(float speed,int verticalDirection,int horizontalDirection)
    {
        rigidbody2d.velocity = new Vector2(transform.right.x * speed * horizontalDirection, transform.right.y * speed * verticalDirection);
    }

}
