using UnityEngine;

public class EnemyGroundMovement : GroundMovement
{
    private BoxCollider2D boxCollider2d;

    private void Awake()
    {
        boxCollider2d = GetComponent<BoxCollider2D>();
    }

    protected override void Start()
    {
        base.Start();
    }


    public  void ChangeDirection()
    {
        HorizontalDirection = HorizontalDirection * (-1);
        transform.localRotation = transform.right.x > 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
        if (HorizontalDirection == -1)
        {
            transform.position = new Vector3(transform.position.x + (2 * boxCollider2d.offset.x) - 0.25f, transform.position.y);
        }
        else
        {
            transform.position = new Vector3(transform.position.x - (2 * boxCollider2d.offset.x) + 0.25f, transform.position.y);
        }
    }

}