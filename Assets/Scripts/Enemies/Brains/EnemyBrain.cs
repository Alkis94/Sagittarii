﻿using UnityEngine;
using System.Collections;

public abstract class  EnemyBrain : MonoBehaviour
{
    [HideInInspector]
    public EnemyStats enemyStats { get; protected set; }
    [HideInInspector]
    public CollisionTracker collisionTracker { get; protected set; }
    [HideInInspector]
    public Rigidbody2D rigidbody2d { get; protected set; }
    [HideInInspector]
    public AudioSource audioSource { get; protected set; }
    [HideInInspector]
    public EnemyGotShot enemyGotShot { get; protected set; }
    [HideInInspector]
    public SpriteRenderer spriteRenderer { get; protected set; }
    [HideInInspector]
    public Raycaster raycaster { get; protected set; }
    [HideInInspector]
    public EnemyAttackHandler enemyAttackHandler { get; protected set; }
    [HideInInspector]
    public Animator animator { get; protected set; }
    [HideInInspector]
    public MovementPattern[] MovementPatterns { get; protected set; }

    //This timer will help enemies that get stuck somewhere not to change directions too rapidly
    [HideInInspector]
    public float cannotChangeDirectionTime = 0f;

    protected virtual void OnEnable()
    {
        enemyStats.EnemyDied += OnEnemyDiedStopAll;
    }

    protected virtual void OnDisable()
    {
        CancelInvoke();
        StopAllCoroutines();
        enemyStats.EnemyDied -= OnEnemyDiedStopAll;
    }

    protected virtual void Awake()
    {
        enemyGotShot = GetComponent<EnemyGotShot>();
        enemyStats = GetComponent<EnemyStats>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyAttackHandler = GetComponent<EnemyAttackHandler>();
        collisionTracker = GetComponentInChildren<CollisionTracker>();
    }

    protected virtual void Start()
    {
        raycaster = GetComponentInChildren<Raycaster>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        StartFacingRandomDirection();
    }

    public virtual void ChangeHorizontalDirection()
    {
        transform.localRotation = transform.localRotation.y == 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
    }

    public virtual void LoadEnemyBrain(Vector3 originalPosition,bool dead)
    {
        //is needed by some enemy brains to make changes when reloading an enemy
    }

    public void UpdateCollisionTracker()
    {
        collisionTracker.collisions.Reset();
        collisionTracker.TrackHorizontalCollisions();
        collisionTracker.TrackVerticalCollisions(rigidbody2d.velocity.y);
    }

    public IEnumerator ChangingDirectionsOverTime(float changeDirectionFrequency)
    {
        while(true)
        {
            ChangeHorizontalDirection();
            yield return new WaitForSeconds(changeDirectionFrequency);
        }
    }


    protected void StartFacingRandomDirection()
    {
        float random = Random.Range(0f, 1f);
        if (random < 0.5f)
        {
            ChangeHorizontalDirection();
        }
    }

    protected void OnEnemyDiedStopAll(DamageType damageType)
    {
        CancelInvoke();
        StopAllCoroutines();
    }

    //Gets called from animation sometimes!
    protected void CallMainAttack()
    {
        enemyAttackHandler.Attack(enemyStats.AttackData[0]);
    }

    protected void StartAttackAnimation()
    {
        animator.SetTrigger("Attack");
    }

    public void LookTowardsPlayer(Transform enemy,Vector3 playerPosition)
    {
        if (playerPosition.x > enemy.position.x)
        {
            enemy.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            enemy.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
