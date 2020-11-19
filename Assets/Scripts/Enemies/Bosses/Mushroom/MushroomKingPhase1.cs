
public class MushroomKingPhase1 : State<MushroomKingBrain>
{
    private int enemiesDiedCounter = 0;

    public MushroomKingPhase1(MushroomKingBrain stateOwner)
    {
        this.stateOwner = stateOwner;
    }

    public override void EnterState()
    {
        EnemyStats.OnEnemyWasKilled += CountEemiesTillChange;
        stateOwner.impulseSource.GenerateImpulse();
        stateOwner.InvokeRepeating("StartAttackAnimation", 1f, stateOwner.enemyStats.AttackData[0].AttackFrequency);
    }

    public override void ExitState()
    {
        stateOwner.CancelInvoke();
        EnemyStats.OnEnemyWasKilled -= CountEemiesTillChange;
    }

    public override void UpdateState()
    {

    }

    public override void FixedUpdateState()
    {

    }

    private void CountEemiesTillChange(DamageSource damageSource)
    {
        enemiesDiedCounter++;
        if(enemiesDiedCounter > 15)
        {
            stateOwner.animator.runtimeAnimatorController = stateOwner.phase2Controller;
            stateOwner.stateMachine.ChangeState(stateOwner.mushroomKingPhase2);
        }
    }

}

