using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

public class BatBossBrain : AdvancedEnemyBrain
{
    
    private Rigidbody2D rigidbody2d;
    private CollisionTracker collisionTracker;
    private Raycaster raycaster;

    public GameObject smallBat;
    [HideInInspector]
    public StateMachine<BatBossBrain> stateMachine;
    [HideInInspector]
    public int horizontalDirection = 1;
    [HideInInspector]
    public int verticalDirection = 1;
    public float spawnSmallBatFrequency;


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
        collisionTracker = GetComponentInChildren<CollisionTracker>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        raycaster = GetComponentInChildren<Raycaster>();
        stateMachine = new StateMachine<BatBossBrain>(this);
        stateMachine.ChangeState(BatBossCalmState.Instance);
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
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
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
}
