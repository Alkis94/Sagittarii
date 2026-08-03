using UnityEngine;

public class BoarChargeState : State<BoarBrain>
{
    public BoarChargeState(BoarBrain stateOwner)
    {
        this.stateOwner = stateOwner;
    }

    public override void EnterState()
    {
        stateOwner.Animator.SetTrigger("Charge");
        stateOwner.AudioSource.Play();
    }

    public override void ExitState()
    {
        stateOwner.Move(0);
        stateOwner.Rigidbody2d.velocity =  Vector3.zero;
        stateOwner.AudioSource.Stop();
    }

    public override void FixedUpdateState()
    {
        if (stateOwner.EnemyStats.Health > 0)
        {
            //RaycastHit2D hit = Physics2D.Raycast(stateOwner.transform.position, stateOwner.transform.right, 3, 1 << LayerMask.NameToLayer("Player"));
            bool hit = Physics2D.OverlapCircle(stateOwner.transform.position, 4, 1 << LayerMask.NameToLayer("Player"));
            if (hit)
            {
                stateOwner.Animator.SetTrigger("Attack");
                stateOwner.stateMachine.ChangeState(stateOwner.stunnedState);
            }

            stateOwner.Move(stateOwner.EnemyStats.Speed);

            if ((stateOwner.CollisionTracker.collisions.left || stateOwner.CollisionTracker.collisions.right || stateOwner.CollisionTracker.CloseToGroundEdge()))
            {
                stateOwner.Animator.SetTrigger("Attack");
                stateOwner.stateMachine.ChangeState(stateOwner.stunnedState);
            }
        }
    }
}

