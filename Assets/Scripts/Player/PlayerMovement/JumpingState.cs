using UnityEngine;
using System.Collections;
using StateMachineNamespace;

public class JumpingState : State<PlayerMovement2>
{
    
    public float jumpForce = 15f;            //Initial force of jump
    public float jumpHoldForce = 1f;		//Incremental force when jump is held
    private float jumpExtraPushLimit = 0;   //Helps limit the times the incremental force can be applied
 

    public JumpingState(PlayerMovement2 owner)
    {
        stateOwner = owner;
    }

    public override void EnterState()
    {
        Debug.Log("Entered JumpState");
        stateOwner.animator.SetTrigger("PlayerJumped");
        stateOwner.rigidBody2d.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        stateOwner.playerAudio.PlayJumpSound();
        jumpExtraPushLimit = 0;
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
        if (stateOwner.input.jumpHeld && jumpExtraPushLimit < 10)
        {
            jumpExtraPushLimit++;
            stateOwner.rigidBody2d.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);
        }

        if (stateOwner.rigidBody2d.velocity.y < 0)
        {
            stateOwner.stateMachine.ChangeState(stateOwner.fallingState);
        }

        if(stateOwner.LegOnGround())
        {
            stateOwner.stateMachine.ChangeState(stateOwner.onGroundState);
        }
    }
}

