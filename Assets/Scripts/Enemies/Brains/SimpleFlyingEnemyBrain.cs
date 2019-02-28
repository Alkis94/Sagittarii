using UnityEngine;

public class SimpleFlyingEnemyBrain : EnemyBrain
{
    private Rigidbody2D rigidbody2d;
    private CollisionTracker collisionTracker;
    private MovementPattern movementPattern;
    private Raycaster raycaster;
    private int horizontalDirection = 1;
    private int verticalDirection = 1;


    protected override void Awake()
    {
        base.Awake();
        movementPattern = GetComponent<MovementPattern>();

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

        collisionTracker = GetComponent<CollisionTracker>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        raycaster = GetComponent<Raycaster>();

        InvokeRepeating("Attack", enemyData.delayBeforeFirstAttack, enemyData.attackFrequency);
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
            cannotChangeDirectionTime = Time.time + 0.05f;
        }
    }

    private void FixedUpdate()
    {
        if (enemyData.health > 0)
        {
            movementPattern.Move(enemyData.speed, verticalDirection);
        }
    }

    private void UpdateCollisionTracker()
    {
        collisionTracker.collisions.Reset();
        collisionTracker.TrackHorizontalCollisions(horizontalDirection);
        collisionTracker.TrackVerticalCollisions(rigidbody2d.velocity.y);
    }

    protected override void ChangeHorizontalDirection()
    {
        horizontalDirection *= -1;
        if (horizontalDirection == -1)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (horizontalDirection == 1)
        {

            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void Attack()
    {
        attackPatern.Attack(enemyData);
    }
}
