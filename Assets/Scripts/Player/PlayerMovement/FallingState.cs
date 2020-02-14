using UnityEngine;
using System.Collections;
using StateMachineNamespace;

public class FallingState : State<PlayerMovement2>
{
    public float maxFallSpeed = -25f;       //Max speed player can fall
    public float coyoteDuration = .05f;     //How long the player can jump after falling

    public FallingState(PlayerMovement2 owner)
    {
        stateOwner = owner;
    }

    public override void EnterState()
    {

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
    }
}
