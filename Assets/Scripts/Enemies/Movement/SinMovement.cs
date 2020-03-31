using UnityEngine;

public class SinMovement : MovementPattern
{
    [SerializeField]
    private float curveFactor = 1; // (0.01 - 0.99 slower curves, 1+ faster curves)
    private float timePassed = 0;
    private float verticalDirection = 0;
    private Rigidbody2D rigidbody2d;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public override void Move(float speed, int brainVerticalDirection, int horizontalDirection)
    {
        timePassed += Time.deltaTime;
        verticalDirection = Mathf.Sin(timePassed * curveFactor);
        float velocityX = transform.right.x * speed;
        float velocityY = transform.up.y * speed * verticalDirection * brainVerticalDirection; // Why 2 vertical directions?
        rigidbody2d.velocity = new Vector2(velocityX, velocityY);
    }
}
