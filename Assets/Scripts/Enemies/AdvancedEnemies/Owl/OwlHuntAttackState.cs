using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

public class OwlHuntAttackState : State<OwlBrain>
{
    private Vector3 playerPosition;

    public OwlHuntAttackState(OwlBrain stateOwner)
    {
        this.stateOwner = stateOwner;
    }

    public override void EnterState()
    {

    }

    public override void FixedUpdateState()
    {
        if (stateOwner.animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
        {
            return;
        }

        playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

        if (playerPosition.y + 1 > stateOwner.transform.position.y || playerPosition.x - 10 > stateOwner.transform.position.x || playerPosition.x + 10 < stateOwner.transform.position.x)
        {
            stateOwner.stateMachine.ChangeState(stateOwner.wonderState);
        }


        if (stateOwner.transform.position.x < playerPosition.x)
        {
            stateOwner.horizontalDirection = stateOwner.transform.right.x > 0 ? 1 : -1;
        }
        else if (stateOwner.transform.position.x > playerPosition.x)
        {
            stateOwner.horizontalDirection = stateOwner.transform.right.x < 0 ? 1 : -1;
        }

        if (playerPosition.x - 1 < stateOwner.transform.position.x && stateOwner.transform.position.x < playerPosition.x + 1)
        {
            stateOwner.animator.SetTrigger("Attack");
            stateOwner.MovementPatterns[1].Move(0, stateOwner.verticalDirection, stateOwner.horizontalDirection);
        }
        else 
        {
            stateOwner.MovementPatterns[1].Move(stateOwner.enemyStats.Speed + 2, stateOwner.verticalDirection, stateOwner.horizontalDirection);
        }

    }

    public override void ExitState()
    {
        stateOwner.StopAllCoroutines();
    }

}
