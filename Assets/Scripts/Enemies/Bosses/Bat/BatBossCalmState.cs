using UnityEngine;
using StateMachineNamespace;

public class BatBossCalmState : State<BatBossBrain>
{
    private float nextAttackTime;

    public BatBossCalmState(BatBossBrain stateOwner)
    {
        this.stateOwner = stateOwner;
    }

    public override void EnterState()
    {
        stateOwner.StartCoroutine(stateOwner.SpawnSmallBats(stateOwner.spawnSmallBatFrequency));
        stateOwner.StartCoroutine(stateOwner.StartAttacking(stateOwner.enemyData.AttackData[0].AttackFrequency, 0));
    }

    public override void UpdateState()
    {
        if(stateOwner.enemyData.Health <= 100)
        {
            stateOwner.stateMachine.ChangeState(stateOwner.enragedState);
        }
    }

    public override void ExitState()
    {
        stateOwner.StopAllCoroutines();
    }

    

}
