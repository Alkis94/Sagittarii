using UnityEngine;

public class GroundEnemyMovement : MonoBehaviour
{
    private BoxCollider2D boxCollider2d;
    private Animator animator;
    private Raycaster raycaster;
    private MovementCollisionHandler movementCollisionHandler;
    private Gravity gravity;

    private Vector2 velocity;
    private float velocityXSmoothing;

    private float accelerationTimeAirborne = .2f;
    private float accelerationTimeGrounded = .1f;

    private float minJumpVelocity;
    private float maxJumpVelocity;

    private int HorizontalDirection = 1;

    private void Awake()
    {
        boxCollider2d = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        
        gravity = GetComponent<Gravity>();
        animator = GetComponent<Animator>();
        movementCollisionHandler = GetComponent<MovementCollisionHandler>();
        raycaster = GetComponent<Raycaster>();
        raycaster.CalculateRaySpacing();
        movementCollisionHandler.collisions.facingDirection = 1;
        minJumpVelocity = gravity.minJumpVelocity;
        maxJumpVelocity = gravity.maxJumpVelocity;
    }

    private Vector2 CalculateVelocity(float speed)
    {
        float targetVelocityX = speed * HorizontalDirection;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (movementCollisionHandler.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity.enabled ? gravity.value * Time.deltaTime : 0;
        return new Vector2(velocity.x, velocity.y) * Time.deltaTime; ;
    }

    public void Move(float speed, bool GoThroughPlatform)
    {
        Vector2 moveAmount = CalculateVelocity(speed);
        HandleWalkingAnimation();
        raycaster.UpdateRaycastOrigins();
        movementCollisionHandler.collisions.Reset();
        movementCollisionHandler.collisions.moveAmountOld = moveAmount;

        if (moveAmount.x != 0)
        {
            movementCollisionHandler.collisions.facingDirection = (int)Mathf.Sign(moveAmount.x);
        }

        movementCollisionHandler.HandleHorizontalCollisions(ref moveAmount);

        if (moveAmount.y != 0)
        {
            movementCollisionHandler.HandleVerticalCollisions(ref moveAmount, GoThroughPlatform);
        }

        moveAmount.x *= transform.right.x;
        transform.Translate(moveAmount);
    }

    private void HandleWalkingAnimation()
    {
        if (movementCollisionHandler.collisions.below)
        {

            animator.SetBool("IsMoving", true);
        }
        else
        {

            animator.SetBool("IsMoving", false);
        }
    }

    public  void Jump()
    {
        if (movementCollisionHandler.collisions.below)
        {
            animator.SetTrigger("Jumped");
            velocity.y = maxJumpVelocity;
        }
    }


    public void ChangeDirection()
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