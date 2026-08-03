using UnityEngine;

public abstract class GroundEnemyBrain : EnemyBrain
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void Start()
    {
        base.Start();
    }

    public void Move(float speed)
    {
        if (CollisionTracker.collisions.below)
        {
            Rigidbody2d.velocity = new Vector2(transform.right.x * speed, Rigidbody2d.velocity.y);
        }
    }

    public void Stop()
    {
        Rigidbody2d.velocity = new Vector2(0f, Rigidbody2d.velocity.y);
    }

    public void Jump(float horizontalForce, float verticalForce)
    {
        var horizontalDirection = transform.localRotation.y == 0 ? 1 : -1;
        Rigidbody2d.AddForce(new Vector2(horizontalForce * horizontalDirection, verticalForce), ForceMode2D.Impulse);
    }

    public bool CheckHorizontalGround()
    {
        if (CollisionTracker.collisions.left || CollisionTracker.collisions.right || CollisionTracker.CloseToGroundEdge())
        {
            return true;
        }

        return false;
    }

    protected void HandleWalkingAnimation()
    {
        if (CollisionTracker.collisions.below)
        {
            Animator.SetBool("IsGrounded", true);
        }
        else
        {

            Animator.SetBool("IsGrounded", false);
        }
    }
}
