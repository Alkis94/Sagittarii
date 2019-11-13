using UnityEngine;
using System.Collections;
using StateMachineNamespace;

public class OwlBrain : AdvancedEnemyBrain
{
    private AudioSource audioSource;
    private Rigidbody2D rigidbody2d;
    private CollisionTracker collisionTracker;
    private Raycaster raycaster;

    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public StateMachine<OwlBrain> stateMachine;
    [HideInInspector]
    public OwlWonderState wonderState;
    [HideInInspector]
    public OwlHuntAttackState huntAttackState;

    [HideInInspector]
    public int horizontalDirection = 1;
    [HideInInspector]
    public int verticalDirection = 1;



    protected override void Awake()
    {
        base.Awake();
        wonderState = new OwlWonderState(this);
        huntAttackState = new OwlHuntAttackState(this);
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
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        stateMachine = new StateMachine<OwlBrain>(this);
        stateMachine.ChangeState(wonderState);
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


    //Called from Animation
    public void Attack()
    {
        AttackPatterns[0].Attack();
    }

    //Called from Animation
    public void MoveOnAttack()
    {
        MoveOnAttackPatterns[0].MoveOnAttack(-8);
    }

    public void MoveBackOnAttack()
    {
        MoveOnAttackPatterns[0].MoveOnAttack(16);
    }
}
