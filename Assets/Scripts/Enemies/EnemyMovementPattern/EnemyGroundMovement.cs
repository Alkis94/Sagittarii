using UnityEngine;

public class EnemyGroundMovement : MonoBehaviour
{

    private Rigidbody2D rigidbody2d;
    private BoxCollider2D boxCollider2d;
    private Raycaster raycaster;
    private Vector2 movement = new Vector2(1, 0);
    private CollisionTracker collisionTracker;

    private void Awake()
    {
        boxCollider2d = GetComponent<BoxCollider2D>();
        collisionTracker = GetComponent<CollisionTracker>();
    }

    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        raycaster = GetComponent<Raycaster>();
    }

    public void Move(float speed)
    {
        transform.Translate(speed * movement * Time.deltaTime);
        raycaster.UpdateRaycastOrigins();
    }

    public void ChangeDirection()
    {
        transform.localRotation = transform.right.x > 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
    }

    public void Jump(int HorizontalDirection)
    {
        rigidbody2d.AddForce(new Vector2(4 * HorizontalDirection, 5), ForceMode2D.Impulse);
    }

}