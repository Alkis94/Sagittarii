using UnityEngine;
using System.Collections;

public class BearBrain : GroundEnemyBrain
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
        InvokeRepeating(nameof(StartAttackAnimation), EnemyStats.DelayBeforeFirstAttack, EnemyStats.AttackData[0].AttackFrequency);  
    }

    protected virtual void FixedUpdate()
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
        }
    }
}
