using UnityEngine;

public class MovementController : MonoBehaviour
{
    protected Animator animator;
    protected Raycaster raycaster;
    protected MovementCollisionHandler movementCollisionHandler;
    protected Gravity gravity;

    protected Vector2 velocity;
    protected float velocityXSmoothing;

    protected float accelerationTimeAirborne = .2f;
    protected float accelerationTimeGrounded = .1f;

    protected float minJumpVelocity;
    protected float maxJumpVelocity;

    private int HorizontalDirection = 1;


    protected virtual void Start()
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

        transform.Translate(moveAmount, Space.World);
    }

    protected virtual void HandleWalkingAnimation()
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

    public virtual void Jump()
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
    }

}