using UnityEngine;
using System.Collections;
using StateMachineNamespace;

public class JumpingState : State<PlayerMovement2>
{
    
    public float jumpForce = 13f;            //Initial force of jump
    public float jumpHoldForce = 2.1f;		//Incremental force when jump is held
    private float jumpExtraPushLimit = 0;   //Helps limit the times the incremental force can be applied
    private float maxJumpSpeed = 10;
    private bool jumpHeldContiniously;
 

    public JumpingState(PlayerMovement2 owner)
    {
        stateOwner = owner;
    }

    public override void EnterState()
    {
        //Debug.Log("Entered JumpState");
        stateOwner.animator.SetTrigger("PlayerJumped");
        stateOwner.rigidBody2d.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        stateOwner.playerAudio.PlayJumpSound();
        jumpExtraPushLimit = 0;
        jumpHeldContiniously = true;
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
        if (stateOwner.input.jumpHeld && jumpExtraPushLimit < 20 && jumpHeldContiniously)
        {
            jumpExtraPushLimit++;
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
    }
}

