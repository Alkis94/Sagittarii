using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

public class OwlWonderState: State<OwlBrain>
{
    private float nextAttackTime;
    private Vector3 playerPosition;

    public OwlWonderState(OwlBrain stateOwner)
    {
        this.stateOwner = stateOwner;
    }

    public override void EnterState()
    {
        nextAttackTime = Time.time + stateOwner.enemyData.AttackData[0].AttackFrequency;
    }

    public override void FixedUpdateState()
    {

        playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

        if (playerPosition.x - 1 < stateOwner.transform.position.x && stateOwner.transform.position.x < playerPosition.x + 1 && playerPosition.y + 3 < stateOwner.transform.position.y)
        {
            stateOwner.stateMachine.ChangeState(stateOwner.huntAttackState);
        }

        if (stateOwner.enemyData.Health > 0)
        {
            stateOwner.MovementPatterns[0].Move(stateOwner.enemyData.Speed, stateOwner.verticalDirection);
        }

        if(nextAttackTime < Time.time)
        {
            stateOwner.animator.SetTrigger("Attack");
            nextAttackTime = Time.time + stateOwner.enemyData.AttackData[0].AttackFrequency;
        }
    }

    public override void ExitState()
    {
        stateOwner.StopAllCoroutines();
    }

}
