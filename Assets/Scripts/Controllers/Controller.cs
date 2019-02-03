using UnityEngine;

public class Controller : MonoBehaviour
{
    protected float moveSpeed = 6;

    protected Animator animator;
    protected Raycaster raycaster;
    protected CollisionHandler collisionHandler;
    protected Gravity gravity;

    protected Vector2 velocity;
    protected float velocityXSmoothing;
    


    protected virtual void Start()
    {
        gravity = GetComponent<Gravity>();
        animator = GetComponent<Animator>();
        collisionHandler = GetComponent<CollisionHandler>();
        raycaster = GetComponent<Raycaster>();
        raycaster.CalculateRaySpacing();
        collisionHandler.collisions.facingDirection = 1;
    }

    protected void CalculateVelocity()
    {
        float targetVelocityX = CalculateTargetVelocity();
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (collisionHandler.collisions.below) ? gravity.accelerationTimeGrounded : gravity.accelerationTimeAirborne);
        velocity.y += gravity.value * Time.deltaTime;
    }

    protected virtual float CalculateTargetVelocity()
    {
        return moveSpeed;
    }


    public void Move(Vector2 moveAmount,bool GoThroughPlatform)
    {
        HandleWalkingAnimation();
        raycaster.UpdateRaycastOrigins();
        collisionHandler.collisions.Reset();
        collisionHandler.collisions.moveAmountOld = moveAmount;

        if (moveAmount.x != 0)
        {
            collisionHandler.collisions.facingDirection = (int)Mathf.Sign(moveAmount.x);
        }


        collisionHandler.HandleHorizontalCollisions(ref moveAmount);

        if (moveAmount.y != 0)
        {
            collisionHandler.HandleVerticalCollisions(ref moveAmount, GoThroughPlatform);
        }

        transform.Translate(moveAmount, Space.World);
    }

    protected virtual void  HandleWalkingAnimation()
    {

        if (collisionHandler.collisions.below)
        {

            animator.SetBool("IsMoving", true);
        }
        else
        {

            animator.SetBool("IsMoving", false);
        }
    }

}
