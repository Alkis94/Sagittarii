using UnityEngine;
using System.Collections;

public class BoarStunnedState : State<BoarBrain>
{
    public BoarStunnedState(BoarBrain stateOwner)
    {
        this.stateOwner = stateOwner;
    }

    public override void EnterState()
    {
        stateOwner.StartCoroutine(Stunned());
    }

    public override void ExitState()
    {
        stateOwner.StopAllCoroutines();
    }

    private IEnumerator Stunned()
    {
        yield return new WaitForFixedUpdate();
        stateOwner.rigidbody2d.velocity = Vector3.zero;
        yield return new WaitForSeconds(3f);
        stateOwner.stateMachine.ChangeState(stateOwner.idleState);
    }
}

