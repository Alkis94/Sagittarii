using UnityEngine;

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
        stateOwner.StartCoroutine(stateOwner.StartAttacking(stateOwner.enemyStats.AttackData[0].AttackFrequency - 1, 0));
        stateOwner.StartCoroutine(stateOwner.StartAttacking(stateOwner.enemyStats.AttackData[1].AttackFrequency, 1));
    }

    public override void ExitState()
    {
        stateOwner.StopAllCoroutines();
    }

}
