using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GroundMovement : MonoBehaviour
{
    protected Animator animator;
    
    protected Raycaster raycaster;
    protected MovementCollisionHandler movementCollisionHandler;
    protected Gravity gravity;

    protected float velocityXSmoothing;
    protected Vector2 velocity;
    protected int HorizontalDirection = 1;

    protected float accelerationTimeAirborne = .2f;
    protected float accelerationTimeGrounded = .1f;

    protected float maxJumpVelocity;

    protected virtual void Start()
    {
        gravity = GetComponent<Gravity>();
        animator = GetComponent<Animator>();
        movementCollisionHandler = GetComponent<MovementCollisionHandler>();
        raycaster = GetComponent<Raycaster>();
        maxJumpVelocity = gravity.maxJumpVelocity;
    }



    public void CalculateMovement(float speed, bool GoThroughPlatform)
    {
        Vector2 moveAmount = CalculateVelocity(speed);
        HandleWalkingAnimation();
        raycaster.UpdateRaycastOrigins();
        movementCollisionHandler.collisions.Reset();

        if (moveAmount.x != 0)
        {
            movementCollisionHandler.collisions.facingDirection = (int)Mathf.Sign(moveAmount.x);
        }

        movementCollisionHandler.HandleHorizontalCollisions(ref moveAmount.x);

        if (moveAmount.y != 0)
        {
            movementCollisionHandler.HandleVerticalCollisions(ref moveAmount.y, GoThroughPlatform);
        }

        moveAmount.x *= transform.right.x;
        transform.Translate(moveAmount);
    }

    private Vector2 CalculateVelocity(float speed)
    {
        float targetVelocityX = speed * HorizontalDirection;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (movementCollisionHandler.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        if (gravity.enabled)
        {
            gravity.ApplyGravityForce(ref velocity.y);
        }
        return new Vector2(velocity.x, velocity.y) * Time.deltaTime; ;
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


    public void Jump()
    {
        if (movementCollisionHandler.collisions.below)
        {
            animator.SetTrigger("Jumped");
            velocity.y = maxJumpVelocity;
        }
    }
}
