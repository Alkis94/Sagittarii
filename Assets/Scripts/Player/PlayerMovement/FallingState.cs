using UnityEngine;
using System.Collections;
using StateMachineNamespace;

public class FallingState : State<PlayerMovement2>
{
    private float maxFallSpeed = -25f;       //Max speed player can fall
    private float coyoteDuration = .1f;     //How long the player can jump after falling
    private float canStillJumpTime;

    public FallingState(PlayerMovement2 owner)
    {
        stateOwner = owner;
    }

    public override void EnterState()
    {
        //Debug.Log("Entered FallingState");
        canStillJumpTime = Time.time + coyoteDuration;
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {

    }

    public override void FixedUpdateState()
    {
        if (stateOwner.rigidBody2d.velocity.y < maxFallSpeed)
        {
            stateOwner.rigidBody2d.velocity = new Vector2(stateOwner.rigidBody2d.velocity.x, maxFallSpeed);
        }

        if (stateOwner.LegOnGround())
        {
            stateOwner.stateMachine.ChangeState(stateOwner.onGroundState);
        }

        if(stateOwner.input.jumpPressed && stateOwner.stateMachine.previousState is OnGroundState && Time.time < canStillJumpTime)
        {
            stateOwner.stateMachine.ChangeState(stateOwner.jumpingState);
        }

        if (stateOwner.input.jumpPressed && stateOwner.LegOnBouncyBall())
        {
            stateOwner.stateMachine.ChangeState(stateOwner.jumpingState);
        }
    }
}
