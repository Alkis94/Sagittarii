using UnityEngine;
using System.Collections;

public class MushroomKingPhase2 : State<MushroomKingBrain>
{
    public MushroomKingPhase2(MushroomKingBrain stateOwner)
    {
        this.stateOwner = stateOwner;
    }

    public override void EnterState()
    {
        stateOwner.enemyStats.Damageable = true;
        stateOwner.rigidbody2d.gravityScale = 1;
        stateOwner.StartCoroutine(DoRandomAttack());
        stateOwner.InvokeRepeating("SpawnWalkshroom", 5, 7);
    }

    public override void FixedUpdateState()
    {
        stateOwner.raycaster.UpdateRaycastOrigins();
        stateOwner.UpdateCollisionTracker();

        if (stateOwner.animator.GetCurrentAnimatorStateInfo(0).IsName("Walking"))
        {
            stateOwner.Move(stateOwner.enemyStats.Speed);
        }

        if (stateOwner.CheckHorizontalGround() && Time.time > stateOwner.cannotChangeDirectionTime)
        {
            stateOwner.cannotChangeDirectionTime = Time.time + 0.1f;
            stateOwner.ChangeHorizontalDirection();
        }
    }

    private IEnumerator DoRandomAttack()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            stateOwner.rigidbody2d.velocity = Vector3.zero;
            yield return new WaitForFixedUpdate();
            stateOwner.rigidbody2d.velocity = Vector3.zero;
            float randomNumber = Random.Range(0, 1f);
            if (randomNumber < 0.5f)
            {
                stateOwner.animator.SetTrigger("Attack1");
            }
            else
            {
                stateOwner.animator.SetTrigger("Attack2");
            }
            yield return new WaitForSeconds(stateOwner.enemyStats.AttackData[0].AttackFrequency);
        }
    }

}

