﻿using UnityEngine;

public class FallingState : State<PlayerMovement>
{
    private float maxFallSpeed = -25f;       //Max speed player can fall
    private float coyoteDuration = .15f;     //How long the player can jump after falling
    private float canStillJumpTime;

    public FallingState(PlayerMovement owner)
    {
        stateOwner = owner;
    }

    public override void EnterState()
    {
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
