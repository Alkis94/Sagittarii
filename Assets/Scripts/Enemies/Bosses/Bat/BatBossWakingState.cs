
public class BatBossWakingState : State<BatBossBrain>
{
    public BatBossWakingState(BatBossBrain stateOwner)
    {
        this.stateOwner = stateOwner;
    }

    public override void FixedUpdateState()
    {
       if(!stateOwner.Animator.GetCurrentAnimatorStateInfo(0).IsName("BatBossWaking"))
        {
            stateOwner.stateMachine.ChangeState(stateOwner.calmState);
        }
    }

    public override void ExitState()
    {
        stateOwner.EnemyStats.Damageable = true;
    }
}
