using UnityEngine;
using System.Collections;
using StateMachineNamespace;

public class OnGroundState : State<PlayerMovement>
{
    public float footOffset = -0.065f;          //X Offset of feet raycast
    public float groundDistance = 0;        //Distance player is considered to be on the ground
    public LayerMask groundLayer;			//Layer of the ground
    private const float skinWidth = .015f;
    private float lastTimeInThisState = 0;

    

    public OnGroundState(PlayerMovement owner)
    {
        stateOwner = owner;
        lastTimeInThisState = Time.time;
    }

    public override void EnterState()
    {
        stateOwner.animator.SetBool("IsGrounded", true);

        if(lastTimeInThisState + 0.5f < Time.time)
        {
            stateOwner.playerAudio.PlayGroundImpactSound();
        }
        
    }

    public override void ExitState()
    {
        stateOwner.animator.SetBool("IsGrounded", false);
        lastTimeInThisState = Time.time;
    }

    public override void UpdateState()
    {
        
    }

    public override void FixedUpdateState()
    {
        if (!stateOwner.LegOnGround())
        {
            stateOwner.stateMachine.ChangeState(stateOwner.fallingState);
        }

        if (stateOwner.input.jumpPressed)
        {
            stateOwner.stateMachine.ChangeState(stateOwner.jumpingState);
        }
    }
}
