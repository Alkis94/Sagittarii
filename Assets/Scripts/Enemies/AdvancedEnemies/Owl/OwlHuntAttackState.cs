using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

public class OwlHuntAttackState : State<OwlBrain>
{
    private Transform player;

    public OwlHuntAttackState(OwlBrain stateOwner)
    {
        this.stateOwner = stateOwner;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void FixedUpdateState()
    {
        if (stateOwner.animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
        {
            return;
        }

        if (player.position.y + 1 > stateOwner.transform.position.y || player.position.x - 10 > stateOwner.transform.position.x || player.position.x + 10 < stateOwner.transform.position.x)
        {
            stateOwner.stateMachine.ChangeState(stateOwner.wonderState);
        }

        if (player.position.x > stateOwner.transform.position.x)
        {
            stateOwner.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            stateOwner.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }

        if (player.position.x - 0.5f < stateOwner.transform.position.x && stateOwner.transform.position.x < player.position.x + 0.5f)
        {
            stateOwner.animator.SetTrigger("Attack");
            stateOwner.MovementPatterns[1].Move(0, stateOwner.verticalDirection, 1);
        }
        else 
        {
            stateOwner.MovementPatterns[1].Move(stateOwner.enemyStats.Speed + 2, stateOwner.verticalDirection, 1);
        }
    }

    public override void ExitState()
    {
        stateOwner.StopAllCoroutines();
    }

}
