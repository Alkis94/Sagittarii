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
        if (rigidbody2d.velocity.y == 0)
        {
            rigidbody2d.velocity = new Vector2(transform.right.x * speed, rigidbody2d.velocity.y);
        }
    }

    public void Jump(float horizontalForce, float verticalForce)
    {
        int horizontalDirection = transform.localRotation.y == 0 ? 1 : -1;
        rigidbody2d.AddForce(new Vector2(horizontalForce * horizontalDirection, verticalForce), ForceMode2D.Impulse);
    }

    public bool CheckHorizontalGround ()
    {
        if (collisionTracker.collisions.left || collisionTracker.collisions.right || collisionTracker.CloseToGroundEdge())
        {
            return true;
        }

        return false;
    }



    protected virtual void HandleWalkingAnimation()
    {
        if (collisionTracker.collisions.below)
        {

            animator.SetBool("IsGrounded", true);
        }
        else
        {

            animator.SetBool("IsGrounded", false);
        }
    }
}
