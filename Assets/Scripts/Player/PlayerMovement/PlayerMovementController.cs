using UnityEngine;

public class PlayerMovementController : MovementController
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

    protected override void Start()
    {
        base.Start();
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
        Move(Speed * playerInput.x,GoThroughPlatform);

        if (movementCollisionHandler.collisions.above || movementCollisionHandler.collisions.below)
        {
            {
                velocity.y = 0;
            }
        }
    }

    private void TryingToGoThroughPlatform()
    {
        GoThroughPlatform = playerInput.y == -1 ? true : false;
    }


    protected override void HandleWalkingAnimation()
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
