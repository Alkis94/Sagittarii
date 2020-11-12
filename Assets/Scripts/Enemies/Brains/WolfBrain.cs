using System.Collections;
using UnityEngine;

public class WolfBrain : GroundEnemyBrain
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void Start()
    {
        base.Start();
        //InvokeRepeating("StartAttackAnimation", enemyStats.DelayBeforeFirstAttack, enemyStats.AttackData[0].AttackFrequency);
        StartCoroutine(WolfAttack());
    }

    protected void FixedUpdate()
    {
        if (enemyStats.Health > 0)
        {
            enemyGroundMovement.Move(enemyStats.Speed);
            raycaster.UpdateRaycastOrigins();
            UpdateCollisionTracker();
            HandleWalkingAnimation();

            if (CheckHorizontalGround() && Time.time > cannotChangeDirectionTime)
            {
                cannotChangeDirectionTime = Time.time + 0.1f;
                ChangeHorizontalDirection();
            }
            animator.SetFloat("VelocityY", rigidbody2d.velocity.y);
        }
    }

    //Gets called from animation
    private void CallJump()
    {
        enemyGroundMovement.Jump(5, 5);
        animator.SetFloat("VelocityY", rigidbody2d.velocity.y);
    }

    IEnumerator WolfAttack()
    {
        yield return new WaitForSeconds(enemyStats.DelayBeforeFirstAttack);
        while(true)
        {
            if (collisionTracker.collisions.below && animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
            {
                CallJump();
                yield return new WaitForFixedUpdate();
                animator.SetTrigger("Attack");
                animator.SetFloat("VelocityY", rigidbody2d.velocity.y);
                yield return new WaitForSeconds(0.5f);
                CallMainAttack();
            }

            yield return new WaitForSeconds(enemyStats.AttackData[0].AttackFrequency);
        }
    }

}
