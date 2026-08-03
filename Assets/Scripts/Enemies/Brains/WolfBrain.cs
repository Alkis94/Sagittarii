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
        StartCoroutine(WolfAttack());
    }

    protected void FixedUpdate()
    {
        if (EnemyStats.Health > 0)
        {
            Move(EnemyStats.Speed);
            Raycaster.UpdateRaycastOrigins();
            UpdateCollisionTracker();
            HandleWalkingAnimation();

            if (CheckHorizontalGround() && Time.time > cannotChangeDirectionTime)
            {
                cannotChangeDirectionTime = Time.time + 0.1f;
                ChangeHorizontalDirection();
            }
            Animator.SetFloat("VelocityY", Rigidbody2d.velocity.y);
        }
    }

    //Gets called from animation
    private void CallJump()
    {
        Jump(5, 5);
        Animator.SetFloat("VelocityY", Rigidbody2d.velocity.y);
    }

    IEnumerator WolfAttack()
    {
        yield return new WaitForSeconds(EnemyStats.DelayBeforeFirstAttack);
        while(true)
        {
            if (CollisionTracker.collisions.below && Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
            {
                CallJump();
                yield return new WaitForFixedUpdate();
                Animator.SetTrigger("Attack");
                Animator.SetFloat("VelocityY", Rigidbody2d.velocity.y);
                yield return new WaitForSeconds(0.5f);
                CallMainAttack();
            }

            yield return new WaitForSeconds(EnemyStats.AttackData[0].AttackFrequency);
        }
    }
}
