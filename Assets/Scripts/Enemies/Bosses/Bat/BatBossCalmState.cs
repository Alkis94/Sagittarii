using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

public class BatBossCalmState : State<BatBossBrain>
{
    private float nextAttackTime;
    private static BatBossCalmState instance;

    private BatBossCalmState()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }

    public static BatBossCalmState Instance
    {
        get
        {
            if (instance == null)
            {
                new BatBossCalmState();
            }
            return instance;
        }
    }

    public override void EnterState(BatBossBrain stateOwner)
    {
        nextAttackTime = Time.time + 1;
        
    }

    public override void FixedUpdateState(BatBossBrain stateOwner)
    {
        if (stateOwner.enemyData.health > 0)
        {
            stateOwner.MovementPatterns[0].Move(stateOwner.enemyData.speed, stateOwner.verticalDirection);
        }

        if(nextAttackTime < Time.time)
        {
            stateOwner.AttackPatterns[0].Attack();
            nextAttackTime = Time.time + stateOwner.enemyData.attackFrequencies[0];
        }

        if(stateOwner.enemyData.health <= 100)
        {
            stateOwner.stateMachine.ChangeState(BatBossEnragedState.Instance);
        }
    }

    
}
