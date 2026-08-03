using UnityEngine;

public abstract class FlyingEnemyBrain : EnemyBrain
{
    [HideInInspector]
    public int verticalDirection = 1;

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
    }

    protected bool HorizontalCollisions()
    {
        if (CollisionTracker.collisions.left || CollisionTracker.collisions.right)
        {
            return true;
        }
        return false;
    }

    protected bool VerticalCollisions()
    {
        if (CollisionTracker.collisions.above || CollisionTracker.collisions.below)
        {
            return true;
        }
        return false;
    }
}
