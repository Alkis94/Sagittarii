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
        InvokeRepeating("CallMainAttack", EnemyStats.DelayBeforeFirstAttack, EnemyStats.AttackData[0].AttackFrequency);
    }

    private void Update()
    {
        UpdateCollisionTracker();
        Raycaster.UpdateRaycastOrigins();

        if (HorizontalCollisions() && Time.time > cannotChangeDirectionTime)
        {
            ChangeHorizontalDirection();
            cannotChangeDirectionTime = Time.time + 0.05f;
        }
    }

    private void FixedUpdate()
    {
        if (EnemyStats.Health > 0)
        {
            MovementPatterns[0].Move(EnemyStats.Speed, 1, 1);
        }
    }
}
