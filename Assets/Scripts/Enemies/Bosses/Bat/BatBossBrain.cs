using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBossBrain : FlyingEnemyBrain
{
    [HideInInspector]
    public StateMachine<BatBossBrain> stateMachine;
    [HideInInspector]
    public BatBossCalmState calmState;
    [HideInInspector]
    public BatBossEnragedState enragedState;
    public GameObject smallBat;
    public float spawnSmallBatFrequency = 5.5f;
    [HideInInspector]
    public float speedBoost = 0;


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
        StartCoroutine(ChangingDirectionsOverTime(10));
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
        if (enemyStats.Health > 0)
        {
            MovementPatterns[0].Move(enemyStats.Speed + speedBoost, verticalDirection);
        }

        stateMachine.FixedUpdate();
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
            enemyAttackHandler.Attack(enemyStats.AttackData[index]);
            yield return new WaitForSeconds(attackFrequency);
        }
    }

}
