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
        nextAttackTime = Time.time + 1;
        stateOwner.StartCoroutine(stateOwner.SpawnSmallBats(stateOwner.spawnSmallBatFrequency));
    }

    public override void FixedUpdateState()
    {
        if (stateOwner.enemyData.Health > 0)
        {
            stateOwner.MovementPatterns[0].Move(stateOwner.enemyData.speed, stateOwner.verticalDirection);
        }

        if(nextAttackTime < Time.time)
        {
            stateOwner.AttackPatterns[0].Attack(0);
            nextAttackTime = Time.time + stateOwner.enemyData.attackFrequencies[0];
        }

        if(stateOwner.enemyData.Health <= 200)
        {
            stateOwner.stateMachine.ChangeState(stateOwner.enragedState);
        }
    }

    public override void ExitState()
    {
        stateOwner.StopAllCoroutines();
    }

}
