using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Raycaster))]
[RequireComponent(typeof(CollisionHandler))]
public class PlayerController : MonoBehaviour
{

    private Animator animator;
    private Raycaster raycaster;
    private CollisionHandler collisionHandler;

    public AudioSource JumpSound;

    private bool DoubleJump = false;
    private bool TripleJump = false;





    public delegate void VoidDelegate();
    public VoidDelegate OnJump;

    private Vector2 playerInput;
    private Vector2 velocity;

    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    private float accelerationTimeAirborne = .2f;
    private float accelerationTimeGrounded = .1f;
    private float moveSpeed = 6;

    private float gravity;
    private float maxJumpVelocity;
    private float minJumpVelocity;
    private float velocityXSmoothing;


    void OnEnable()
    {
        PlayerCollision.OnPlayerGotBatWings += PlayerGotBatWings;
    }


    void OnDisable()
    {
        PlayerCollision.OnPlayerGotBatWings -= PlayerGotBatWings;
    }




    private void Start()
    {
        animator = GetComponent<Animator>();
        collisionHandler = GetComponent<CollisionHandler>();
        raycaster = GetComponent<Raycaster>();
        raycaster.CalculateRaySpacing();
        collisionHandler.collisions.facingDirection = 1;
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

        if (ItemHandler.PlayerHasBatWings)
        {
            OnJump = JumpWithWings;
        }
        else
        {
            OnJump = JumpWithoutWings;
        }
    }

    private void Update()
    {
        CalculateVelocity();
        Move(velocity * Time.deltaTime);

        if (collisionHandler.collisions.above || collisionHandler.collisions.below)
        {
            {
                velocity.y = 0;
            }
        }
    }

    private void CalculateVelocity()
    {
        float targetVelocityX = playerInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (collisionHandler.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
    }


    public void Move(Vector2 moveAmount)
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
            collisionHandler.HandleVerticalCollisions(ref moveAmount,playerInput.y);
        }

        transform.Translate(moveAmount,Space.World);
    }


    public void SetDirectionalInput(Vector2 input)
    {
        playerInput = input;
    }

    public void JumpWithoutWings()
    {
        if (collisionHandler.collisions.below)
        {
            DoubleJump = true;
            animator.SetTrigger("PlayerJumped");
            velocity.y = maxJumpVelocity;
            JumpSound.Play();
        }
        else if (DoubleJump && !collisionHandler.collisions.below)
        {
            DoubleJump = false;
            animator.SetTrigger("PlayerJumped");
            velocity.y = maxJumpVelocity;
            JumpSound.Play();
        }
    }

    public void JumpWithWings()
    {
        if (collisionHandler.collisions.below)
        {
            DoubleJump = true;
            animator.SetTrigger("PlayerJumped");
            velocity.y = maxJumpVelocity;
            JumpSound.Play();
        }
        else if (DoubleJump && !collisionHandler.collisions.below)
        {
            TripleJump = true;
            DoubleJump = false;
            animator.SetTrigger("PlayerJumped");
            velocity.y = maxJumpVelocity;
            JumpSound.Play();
        }
        else if (TripleJump && !collisionHandler.collisions.below)
        {
            TripleJump = false;
            animator.SetTrigger("PlayerJumped");
            velocity.y = maxJumpVelocity;
            JumpSound.Play();
        }
    }

    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }

    private void PlayerGotBatWings()
    {
        OnJump = JumpWithWings;
    }





    public void StandingOnPlatform()
    {
        collisionHandler.collisions.below = true;
    }

    private void HandleWalkingAnimation()
    {

        if (collisionHandler.collisions.below && playerInput.x != 0)
        {

            animator.SetBool("PlayerIsMoving",true);
        }
        else
        {

            animator.SetBool("PlayerIsMoving", false);
        }
    }
   

}
