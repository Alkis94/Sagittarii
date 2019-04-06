using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

public class BatBossEnragedState : State<BatBossBrain>
{
    private float nextAttackTime;

    public BatBossEnragedState(BatBossBrain stateOwner)
    {
        this.stateOwner = stateOwner;
    }

    public override void EnterState()
    {
        nextAttackTime = Time.time;
        stateOwner.enemyData.speed += 2;
        stateOwner.StopCoroutine(stateOwner.ChangingDirectionsOverTime(0));
        stateOwner.StartCoroutine(stateOwner.SpawnSmallBats(stateOwner.spawnSmallBatFrequency - 2));
    }

    public override void FixedUpdateState()
    {
        if (stateOwner.enemyData.health > 0)
        {
            stateOwner.MovementPatterns[0].Move(stateOwner.enemyData.speed, stateOwner.verticalDirection);
        }

        if (nextAttackTime < Time.time)
        {
            stateOwner.StartCoroutine(stateOwner.MultiTargetedAttack(3));
            nextAttackTime = Time.time + stateOwner.enemyData.attackFrequencies[0];
        }

        if (stateOwner.enemyData.health <= 100)
        {
            stateOwner.stateMachine.ChangeState(stateOwner.frenzyState);
        }
    }

    public override void ExitState()
    {
        stateOwner.StopAllCoroutines();
    }

}
