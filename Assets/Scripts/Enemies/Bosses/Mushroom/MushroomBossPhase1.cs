
public class MushroomBossPhase1 : State<MushroomBossBrain>
{
    private int enemiesDiedCounter = 0;

    public MushroomBossPhase1(MushroomBossBrain stateOwner)
    {
        this.stateOwner = stateOwner;
    }

    public override void EnterState()
    {
        EnemyStats.OnEnemyWasKilled += CountEemiesTillChange;
        stateOwner.AudioSource.Play();
        stateOwner.impulseSource.GenerateImpulse();
        stateOwner.InvokeRepeating("StartAttackAnimation", 1f, stateOwner.EnemyStats.AttackData[0].AttackFrequency);
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
            stateOwner.Animator.runtimeAnimatorController = stateOwner.phase2Controller;
            stateOwner.stateMachine.ChangeState(stateOwner.mushroomKingPhase2);
        }
    }

}

