using UnityEngine;

public class PlayerController : Controller
{
    public AudioClip JumpSound;
    private AudioSource audioSource;

    private bool GoThroughPlatform = false;
    private bool DoubleJump = false;
    private bool TripleJump = false;

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
        CalculateVelocity();
        TryingToGoThroughPlatform();
        Move(velocity * Time.deltaTime, GoThroughPlatform);

        if (collisionHandler.collisions.above || collisionHandler.collisions.below)
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

    protected override float CalculateTargetVelocity()
    {
        return moveSpeed * playerInput.x;
    }

    protected override void HandleWalkingAnimation()
    {

        if (collisionHandler.collisions.below && playerInput.x != 0)
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
        if (collisionHandler.collisions.below)
        {
            DoubleJump = true;
            animator.SetTrigger("PlayerJumped");
            velocity.y = gravity.maxJumpVelocity;
            PlayJumpSound();
        }
        else if (DoubleJump && !collisionHandler.collisions.below)
        {
            DoubleJump = false;
            animator.SetTrigger("PlayerJumped");
            velocity.y = gravity.maxJumpVelocity;
            PlayJumpSound();
        }
    }

    public void JumpWithWings()
    {
        if (collisionHandler.collisions.below)
        {
            DoubleJump = true;
            animator.SetTrigger("PlayerJumped");
            velocity.y = gravity.maxJumpVelocity;
            PlayJumpSound();
        }
        else if (DoubleJump && !collisionHandler.collisions.below)
        {
            TripleJump = true;
            DoubleJump = false;
            animator.SetTrigger("PlayerJumped");
            velocity.y = gravity.maxJumpVelocity;
            PlayJumpSound();
        }
        else if (TripleJump && !collisionHandler.collisions.below)
        {
            TripleJump = false;
            animator.SetTrigger("PlayerJumped");
            velocity.y = gravity.maxJumpVelocity;
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
        collisionHandler.collisions.below = true;
    }

    
   

}
