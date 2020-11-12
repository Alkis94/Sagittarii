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
        InvokeRepeating("StartAttackAnimation", enemyStats.DelayBeforeFirstAttack, enemyStats.AttackData[0].AttackFrequency);  
    }

    protected virtual void FixedUpdate()
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
        }
    }
}
