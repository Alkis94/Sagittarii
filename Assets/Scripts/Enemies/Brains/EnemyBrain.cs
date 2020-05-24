using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  EnemyBrain : MonoBehaviour
{
    [SerializeField]
    protected bool hasAttack = true;

    [HideInInspector]
    public EnemyStats enemyStats;
    protected EnemyGotShot enemyGotShot;
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rigidbody2d;
    protected CollisionTracker collisionTracker;
    protected Raycaster raycaster;
    protected AudioSource audioSource;

    [HideInInspector]
    public Animator animator { get; protected set; }
    [HideInInspector]
    public AttackPattern AttackPatterns { get; protected set; }
    [HideInInspector]
    public MovementPattern[] MovementPatterns { get; protected set; }


    //This timer will help enemies that get stuck somewhere not to change directions too rapidly
    protected float cannotChangeDirectionTime = 0f;

    protected float speed;

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
        AttackPatterns = GetComponent<AttackPattern>();
    }

    protected virtual void Start()
    {
        collisionTracker = GetComponentInChildren<CollisionTracker>();
        raycaster = GetComponentInChildren<Raycaster>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (enemyStats.ChangingDirections)
        {
            StartCoroutine(ChangingDirectionsOverTime(enemyStats.ChangeDirectionFrequency));
        }

        StartFacingRandomDirection();
    }

    protected abstract void ChangeHorizontalDirection();

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

    protected void OnEnemyDiedStopAll()
    {
        CancelInvoke();
        StopAllCoroutines();
    }

    //Gets called from animation sometimes!
    protected void CallMainAttack()
    {
        AttackPatterns.Attack(enemyStats.AttackData[0]);
    }

    protected void StartAttackAnimation()
    {
        animator.SetTrigger("Attack");
    }
}
