using UnityEngine;
using System.Collections;
using StateMachineNamespace;

public class BoarChargeState : State<BoarBrain>
{
    public BoarChargeState(BoarBrain stateOwner)
    {
        this.stateOwner = stateOwner;
    }

    public override void EnterState()
    {
        stateOwner.animator.SetTrigger("Charge");
        stateOwner.audioSource.Play();
    }

    public override void ExitState()
    {
        stateOwner.Move(0);
        stateOwner.rigidbody2d.velocity =  Vector3.zero;
        stateOwner.audioSource.Stop();
    }

    public override void FixedUpdateState()
    {
        if (stateOwner.enemyStats.Health > 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(stateOwner.transform.position, stateOwner.transform.right, 3, 1 << LayerMask.NameToLayer("Player"));
            if (hit)
            {
                stateOwner.animator.SetTrigger("Attack");
                stateOwner.stateMachine.ChangeState(stateOwner.stunnedState);
            }

            stateOwner.Move(stateOwner.enemyStats.Speed);

            if ((stateOwner.collisionTracker.collisions.left || stateOwner.collisionTracker.collisions.right || stateOwner.collisionTracker.CloseToGroundEdge()))
            {
                stateOwner.animator.SetTrigger("Attack");
                stateOwner.stateMachine.ChangeState(stateOwner.stunnedState);
            }
        }
    }
}

