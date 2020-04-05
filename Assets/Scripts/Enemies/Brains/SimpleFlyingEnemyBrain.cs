using UnityEngine;

public class SimpleFlyingEnemyBrain : EnemyBrain
{
    
    private int horizontalDirection = 1;
    private int verticalDirection = 1;


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
        if(AttackPatterns[0] != null)
        {
            InvokeRepeating("CallMainAttack", enemyData.delayBeforeFirstAttack, enemyData.attackFrequencies[0]);
        }
        
    }


    private void Update()
    {
        UpdateCollisionTracker();
        raycaster.UpdateRaycastOrigins();

        if (collisionTracker.collisions.left || collisionTracker.collisions.right && Time.time > cannotChangeDirectionTime)
        {
            ChangeHorizontalDirection();
            cannotChangeDirectionTime = Time.time + 0.05f;
        }

        if (collisionTracker.collisions.above || collisionTracker.collisions.below && Time.time > cannotChangeDirectionTime)
        {
            verticalDirection *= -1;
            ChangeHorizontalDirection();
            cannotChangeDirectionTime = Time.time + 0.05f;
        }
    }

    private void FixedUpdate()
    {
        if (enemyData.health > 0)
        {
            MovementPatterns[0].Move(enemyData.speed,horizontalDirection, verticalDirection);
        }
    }

    private void UpdateCollisionTracker()
    {
        collisionTracker.collisions.Reset();
        collisionTracker.TrackHorizontalCollisions();
        collisionTracker.TrackVerticalCollisions(rigidbody2d.velocity.y);
    }


    //Direction Changing needs more work for flying enemies.
    protected override void ChangeHorizontalDirection()
    {
        horizontalDirection *= -1;

        if (horizontalDirection == -1)
        {
            transform.localRotation = Quaternion.Euler(0, 180, -transform.localEulerAngles.z);
        }
        else if (horizontalDirection == 1)
        {

            transform.localRotation = Quaternion.Euler(0, 0, transform.localEulerAngles.z);
        }
    }
}
