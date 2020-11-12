using UnityEngine;
using System.Collections;

public class CrowBrain : FlyingEnemyBrain
{

    protected override void Awake()
    {
        base.Awake();
        MovementPatterns = GetComponents<MovementPattern>();
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
        InvokeRepeating("CallMainAttack", enemyStats.DelayBeforeFirstAttack, enemyStats.AttackData[0].AttackFrequency);
    }

    private void Update()
    {
        UpdateCollisionTracker();
        raycaster.UpdateRaycastOrigins();

        if (HorizontalCollisions() && Time.time > cannotChangeDirectionTime)
        {
            ChangeHorizontalDirection();
            cannotChangeDirectionTime = Time.time + 0.05f;
        }
    }

    private void FixedUpdate()
    {
        if (enemyStats.Health > 0)
        {
            MovementPatterns[0].Move(enemyStats.Speed, 1, 1);
        }
    }
}
