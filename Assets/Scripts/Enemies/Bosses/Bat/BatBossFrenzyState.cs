using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

public class BatBossFrenzyState : State<BatBossBrain>
{
    private float nextAttackTime;
    private float nextAttackTime2;
    private static BatBossFrenzyState instance;

    private BatBossFrenzyState()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }

    public static BatBossFrenzyState Instance
    {
        get
        {
            if (instance == null)
            {
                new BatBossFrenzyState();
            }
            return instance;
        }
    }

    public override void EnterState(BatBossBrain stateOwner)
    {
        nextAttackTime = Time.time;
        nextAttackTime2 = Time.time + 1;
        stateOwner.enemyData.speed += 2;
    }

    public override void FixedUpdateState(BatBossBrain stateOwner)
    {
        if (stateOwner.enemyData.health > 0)
        {
            stateOwner.MovementPatterns[0].Move(stateOwner.enemyData.speed, stateOwner.verticalDirection);
        }

        if (nextAttackTime < Time.time)
        {
            stateOwner.StartCoroutine(TripleTargetedAttack(stateOwner));
            nextAttackTime = Time.time + stateOwner.enemyData.attackFrequencies[0];
        }

        if (nextAttackTime2 < Time.time)
        {
            stateOwner.AttackPatterns[1].Attack();
            nextAttackTime2 = Time.time + stateOwner.enemyData.attackFrequencies[1];
        }

        if (stateOwner.enemyData.health <= 0)
        {
            stateOwner.stateMachine.ChangeState(null);
        }
    }

    IEnumerator TripleTargetedAttack(BatBossBrain stateOwner)
    {
        stateOwner.AttackPatterns[0].Attack();
        yield return new WaitForSeconds(0.25f);
        stateOwner.AttackPatterns[0].Attack();
        yield return new WaitForSeconds(0.25f);
        stateOwner.AttackPatterns[0].Attack();
        yield return new WaitForSeconds(0.25f);
        stateOwner.AttackPatterns[0].Attack();
        yield return new WaitForSeconds(0.25f);
        stateOwner.AttackPatterns[0].Attack();
    }

}
