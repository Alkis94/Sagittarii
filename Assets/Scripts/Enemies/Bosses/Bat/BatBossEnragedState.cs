using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

public class BatBossEnragedState : State<BatBossBrain>
{
    private float nextAttackTime;
    private static BatBossEnragedState instance;

    private BatBossEnragedState()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }

    public static BatBossEnragedState Instance
    {
        get
        {
            if (instance == null)
            {
                new BatBossEnragedState();
            }
            return instance;
        }
    }

    public override void EnterState(BatBossBrain stateOwner)
    {
        nextAttackTime = Time.time;
        stateOwner.enemyData.speed += 2;
        stateOwner.enemyData.changingDirections = false;
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

        if (stateOwner.enemyData.health <= 50)
        {
            stateOwner.stateMachine.ChangeState(BatBossFrenzyState.Instance);
        }
    }


    IEnumerator TripleTargetedAttack(BatBossBrain stateOwner)
    {
        stateOwner.AttackPatterns[0].Attack();
        yield return new WaitForSeconds(0.25f);
        stateOwner.AttackPatterns[0].Attack();
        yield return new WaitForSeconds(0.25f);
        stateOwner.AttackPatterns[0].Attack();
    }

}
