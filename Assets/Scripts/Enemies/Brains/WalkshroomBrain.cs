using UnityEngine;
using System.Collections;

public class WalkshroomBrain : GroundEnemyBrain
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
    }

    protected void FixedUpdate()
    {
        raycaster.UpdateRaycastOrigins();
        UpdateCollisionTracker();
        HandleWalkingAnimation();

        if (CheckHorizontalGround() && Time.time > cannotChangeDirectionTime)
        {
            cannotChangeDirectionTime = Time.time + 0.1f;
            ChangeHorizontalDirection();
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walking"))
        {
            bool result = Physics2D.OverlapCircle(transform.position, 2.5f, 1 << LayerMask.NameToLayer("Player"));
            if (result)
            {
                animator.SetTrigger("Attack");
                Move(0);
                enemyAttackHandler.Attack(enemyStats.AttackData[0]);
            }
            else
            {
                Move(enemyStats.Speed);
            }
        }
    }
}
