
public class BatBossWakingState : State<BatBossBrain>
{
    public BatBossWakingState(BatBossBrain stateOwner)
    {
        this.stateOwner = stateOwner;
    }

    public override void FixedUpdateState()
    {
       if(!stateOwner.animator.GetCurrentAnimatorStateInfo(0).IsName("BatBossWaking"))
        {
            stateOwner.stateMachine.ChangeState(stateOwner.calmState);
        }
    }

    public override void ExitState()
    {
        stateOwner.enemyStats.Damageable = true;
    }
}
