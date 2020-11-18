using UnityEngine;

public class JumpingState : State<PlayerMovement>
{
    public float jumpForce = 15f;            //Initial force of jump
    public float jumpHoldForce = 2.5f;		//Incremental force when jump is held
    private float jumpExtraPush = 0;
    private const float jumpExtraPushLimit = 20; //Helps limit the times the incremental force can be applied
    private float maxJumpSpeed = 12;
    private bool jumpHeldContiniously;
    private float bouncyMushroomCooldown = 0;
 
    public JumpingState(PlayerMovement owner)
    {
        stateOwner = owner;
    }

    public override void EnterState()
    {
        Jump();
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {

    }

    public override void FixedUpdateState()
    {
        //...and the jump button is held, apply an incremental force to the rigidbody...
        if (stateOwner.input.jumpHeld && jumpExtraPush < jumpExtraPushLimit && jumpHeldContiniously)
        {
            jumpExtraPush++;
            stateOwner.rigidBody2d.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);
        }
        else
        {
            jumpHeldContiniously = false;
        }

        if (stateOwner.rigidBody2d.velocity.y < 0)
        {
            stateOwner.stateMachine.ChangeState(stateOwner.fallingState);
        }

        if(stateOwner.LegOnGround())
        {
            stateOwner.stateMachine.ChangeState(stateOwner.onGroundState);
        }

        if (stateOwner.rigidBody2d.velocity.y > maxJumpSpeed)
        {
            stateOwner.rigidBody2d.velocity = new Vector2(stateOwner.rigidBody2d.velocity.x, maxJumpSpeed);
        }

        if (stateOwner.input.jumpPressed && stateOwner.LegOnBouncyBall() && bouncyMushroomCooldown < Time.time)
        {
            Jump();
            bouncyMushroomCooldown = Time.time + 0.25f;
        }
    }

    private void Jump()
    {
        stateOwner.rigidBody2d.velocity = Vector2.zero;
        stateOwner.animator.SetTrigger("Jump");
        stateOwner.rigidBody2d.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        stateOwner.playerAudio.PlayJumpSound();
        jumpExtraPush = 0;
        jumpHeldContiniously = true;
    }
}

