using UnityEngine;
using System.Collections;
using StateMachineNamespace;

public class BoarIdleState : State<BoarBrain>
{
    private Transform player;

    public BoarIdleState(BoarBrain stateOwner)
    {
        this.stateOwner = stateOwner;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void EnterState()
    {
        stateOwner.StartCoroutine(Idle());
    }

    public override void ExitState()
    {
        stateOwner.StopAllCoroutines();
    }

    public override void FixedUpdateState()
    {
        if (stateOwner.enemyStats.Health > 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(stateOwner.transform.position, stateOwner.transform.right, 25, 1 << LayerMask.NameToLayer("Player"));
            if (hit)
            {
                stateOwner.stateMachine.ChangeState(stateOwner.chargeState);
            }

            stateOwner.LookTowardsPlayer(stateOwner.transform, player.position);
        }
    }

    private IEnumerator Idle()
    {
        yield return new WaitForSeconds(2f);
        while(true)
        {
            stateOwner.animator.SetTrigger("Idle");
            yield return new WaitForSeconds(5f);
        }
    }
}

