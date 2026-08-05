using UnityEngine;

public class OnGroundState : State<PlayerMovement>
{
    // X Offset of feet raycast
    public float footOffset = -0.065f;
    // Distance player is considered to be on the ground
    public float groundDistance = 0;
    // Layer of the ground
    public LayerMask groundLayer;			
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
