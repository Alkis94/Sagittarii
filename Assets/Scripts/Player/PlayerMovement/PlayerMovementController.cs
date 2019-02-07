using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public AudioClip JumpSound;
    private AudioSource audioSource;

    private bool GoThroughPlatform = false;
    private bool DoubleJump = false;
    private bool TripleJump = false;

    [SerializeField]
    private float Speed = 6;

    public delegate void VoidDelegate();
    public VoidDelegate OnJump;

    protected BoxCollider2D boxCollider2d;
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

    [HideInInspector]
    public Vector2 playerInput;

    void OnEnable()
    {
        PlayerCollision.OnPlayerGotBatWings += PlayerGotBatWings;
    }

    void OnDisable()
    {
        PlayerCollision.OnPlayerGotBatWings -= PlayerGotBatWings;
    }

    private  void Start()
    {
        boxCollider2d = GetComponent<BoxCollider2D>();
        gravity = GetComponent<Gravity>();
        animator = GetComponent<Animator>();
        movementCollisionHandler = GetComponent<MovementCollisionHandler>();
        raycaster = GetComponent<Raycaster>();
        raycaster.CalculateRaySpacing();
        movementCollisionHandler.collisions.facingDirection = 1;
        minJumpVelocity = gravity.minJumpVelocity;
        maxJumpVelocity = gravity.maxJumpVelocity;
        audioSource = GetComponent<AudioSource>();


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

        TryingToGoThroughPlatform();
        CalculateMovement(Speed * playerInput.x, GoThroughPlatform);

        if (movementCollisionHandler.collisions.above || movementCollisionHandler.collisions.below)
        {
            {
                velocity.y = 0;
            }
        }
    }

    private void CalculateMovement(float speed, bool GoThroughPlatform)
    {
        Vector2 moveAmount = CalculateVelocity(speed);
        HandleWalkingAnimation();
        raycaster.UpdateRaycastOrigins();
        movementCollisionHandler.collisions.Reset();
 

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

    private void TryingToGoThroughPlatform()
    {
        GoThroughPlatform = playerInput.y == -1 ? true : false;
    }


    private void HandleWalkingAnimation()
    {

        if (movementCollisionHandler.collisions.below && playerInput.x != 0)
        {

            animator.SetBool("IsMoving", true);
        }
        else
        {

            animator.SetBool("IsMoving", false);
        }
    }

    public void JumpWithoutWings()
    {
        if (movementCollisionHandler.collisions.below)
        {
            DoubleJump = true;
            animator.SetTrigger("PlayerJumped");
            velocity.y = maxJumpVelocity;
            PlayJumpSound();
        }
        else if (DoubleJump && !movementCollisionHandler.collisions.below)
        {
            DoubleJump = false;
            animator.SetTrigger("PlayerJumped");
            velocity.y = maxJumpVelocity;
            PlayJumpSound();
        }
    }

    public void JumpWithWings()
    {
        if (movementCollisionHandler.collisions.below)
        {
            DoubleJump = true;
            animator.SetTrigger("PlayerJumped");
            velocity.y = maxJumpVelocity;
            PlayJumpSound();
        }
        else if (DoubleJump && !movementCollisionHandler.collisions.below)
        {
            TripleJump = true;
            DoubleJump = false;
            animator.SetTrigger("PlayerJumped");
            velocity.y = maxJumpVelocity;
            PlayJumpSound();
        }
        else if (TripleJump && !movementCollisionHandler.collisions.below)
        {
            TripleJump = false;
            animator.SetTrigger("PlayerJumped");
            velocity.y = maxJumpVelocity;
            PlayJumpSound();
        }
    }

    public void PlayJumpSound()
    {
        audioSource.clip = JumpSound;
        audioSource.Play();
    }
    public void OnJumpInputUp()
    {
        if (velocity.y > gravity.minJumpVelocity)
        {
            velocity.y = gravity.minJumpVelocity;
        }
    }

    private void PlayerGotBatWings()
    {
        OnJump = JumpWithWings;
    }

    public void StandingOnPlatform()
    {
        movementCollisionHandler.collisions.below = true;
    }
}
