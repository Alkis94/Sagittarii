using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

public class BatBossFrenzyState : State<BatBossBrain>
{
    private float nextAttackTime;
    private float nextAttackTime2;


    public BatBossFrenzyState(BatBossBrain stateOwner)
    {
        this.stateOwner = stateOwner;
    }

    public override void EnterState()
    {
        nextAttackTime = Time.time;
        nextAttackTime2 = Time.time + 1;
        stateOwner.enemyData.speed += 2;
        stateOwner.StartCoroutine(stateOwner.SpawnSmallBats(stateOwner.spawnSmallBatFrequency - 4));
    }

    public override void FixedUpdateState()
    {
        if (stateOwner.enemyData.Health > 0)
        {
            stateOwner.MovementPatterns[0].Move(stateOwner.enemyData.speed, stateOwner.verticalDirection);
        }

        if (nextAttackTime < Time.time)
        {
            stateOwner.StartCoroutine(stateOwner.MultiTargetedAttack(5));
            nextAttackTime = Time.time + stateOwner.enemyData.attackFrequencies[0];
        }

        if (nextAttackTime2 < Time.time)
        {
            stateOwner.AttackPatterns.Attack(1);
            nextAttackTime2 = Time.time + stateOwner.enemyData.attackFrequencies[1];
        }

        if (stateOwner.enemyData.Health <= 0)
        {
            stateOwner.stateMachine.ChangeState(null);
        }
    }

    public override void ExitState()
    {
        stateOwner.StopAllCoroutines();
    }
}
