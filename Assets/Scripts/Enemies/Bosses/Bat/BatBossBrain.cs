using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

public class BatBossBrain : EnemyBrain
{

    [HideInInspector]
    public StateMachine<BatBossBrain> stateMachine;
    [HideInInspector]
    public BatBossCalmState calmState;
    [HideInInspector]
    public BatBossEnragedState enragedState;

    public GameObject smallBat;
    [HideInInspector]
    public int horizontalDirection = 1;
    [HideInInspector]
    public int verticalDirection = 1;
    public float spawnSmallBatFrequency = 5.5f;


    protected override void Awake()
    {
        base.Awake();
        calmState = new BatBossCalmState(this);
        enragedState = new BatBossEnragedState(this);
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
        collisionTracker = GetComponentInChildren<CollisionTracker>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        raycaster = GetComponentInChildren<Raycaster>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        stateMachine = new StateMachine<BatBossBrain>(this);
        stateMachine.ChangeState(calmState);
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
        if (enemyData.Health > 0)
        {
            MovementPatterns[0].Move(enemyData.speed, verticalDirection);
        }

        stateMachine.FixedUpdate();
    }

    private void UpdateCollisionTracker()
    {
        collisionTracker.collisions.Reset();
        collisionTracker.TrackHorizontalCollisions();
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

    public IEnumerator SpawnSmallBats(float spawnFrequency)
    {
        while (true)
        {
            Instantiate(smallBat,transform.position, Quaternion.identity,transform.parent);
            yield return new WaitForSeconds(spawnFrequency);
        }
    }

    public IEnumerator StartAttacking(float attackFrequency,int index)
    {
        while(true)
        {
            AttackPatterns.Attack(index);
            yield return new WaitForSeconds(attackFrequency);
        }
    }


    //Gets called from animation
    public void CallCirclePerimetricalAttack()
    {
        audioSource.Play();
        AttackPatterns.Attack(3);
    }

}
