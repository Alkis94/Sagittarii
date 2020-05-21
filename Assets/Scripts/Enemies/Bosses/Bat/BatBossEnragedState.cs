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
        stateOwner.speedBoost = 4;
        stateOwner.StopCoroutine(stateOwner.ChangingDirectionsOverTime(0));
        stateOwner.StartCoroutine(stateOwner.SpawnSmallBats(stateOwner.spawnSmallBatFrequency - 4));
        stateOwner.StartCoroutine(stateOwner.StartAttacking(stateOwner.enemyData.attackData[0].attackFrequency - 1, 0));
        stateOwner.StartCoroutine(stateOwner.StartAttacking(stateOwner.enemyData.attackData[1].attackFrequency, 1));
    }

    public override void ExitState()
    {
        stateOwner.StopAllCoroutines();
    }

}
