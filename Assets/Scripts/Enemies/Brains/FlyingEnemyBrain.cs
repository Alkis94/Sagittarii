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

    //Direction Changing needs more work for flying enemies.
    //public  void ChangeHorizontalDirection2()
    //{
    //    horizontalDirection *= -1;

    //    if (horizontalDirection == -1)
    //    {
    //        transform.localRotation = Quaternion.Euler(0, 180, -transform.localEulerAngles.z);
    //    }
    //    else if (horizontalDirection == 1)
    //    {
    //        transform.localRotation = Quaternion.Euler(0, 0, transform.localEulerAngles.z);
    //    }
    //}

    //public override void ChangeHorizontalDirection()
    //{
    //    transform.localRotation = transform.localRotation.y == 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
    //}
}
