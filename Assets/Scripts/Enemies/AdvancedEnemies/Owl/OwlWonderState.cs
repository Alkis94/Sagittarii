using UnityEngine;

public class OwlWonderState: State<OwlBrain>
{
    private float nextAttackTime;
    private Transform playerPosition;

    public OwlWonderState(OwlBrain stateOwner)
    {
        this.stateOwner = stateOwner;
    }

    public override void EnterState()
    {
        nextAttackTime = Time.time + stateOwner.enemyStats.AttackData[0].AttackFrequency;
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void FixedUpdateState()
    {
        if (playerPosition.position.x - 1 < stateOwner.transform.position.x && stateOwner.transform.position.x < playerPosition.position.x + 1 && playerPosition.position.y + 3 < stateOwner.transform.position.y)
        {
            stateOwner.stateMachine.ChangeState(stateOwner.huntAttackState);
        }

        if (stateOwner.enemyStats.Health > 0)
        {
            stateOwner.MovementPatterns[0].Move(stateOwner.enemyStats.Speed, stateOwner.verticalDirection);
        }

        if(nextAttackTime < Time.time)
        {
            stateOwner.animator.SetTrigger("Attack");
            nextAttackTime = Time.time + stateOwner.enemyStats.AttackData[0].AttackFrequency;
        }
    }

    public override void ExitState()
    {
        stateOwner.StopAllCoroutines();
    }

}
