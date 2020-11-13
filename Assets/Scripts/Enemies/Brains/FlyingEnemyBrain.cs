using UnityEngine;

//Should be abstract but unity throws a lot of wrong warnings if you have it abstract.
public class FlyingEnemyBrain : EnemyBrain
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
        if (collisionTracker.collisions.left || collisionTracker.collisions.right)
        {
            return true;
        }
        return false;
    }

    protected bool VerticalCollisions()
    {
        if (collisionTracker.collisions.above || collisionTracker.collisions.below)
        {
            return true;
        }
        return false;
    }
}
