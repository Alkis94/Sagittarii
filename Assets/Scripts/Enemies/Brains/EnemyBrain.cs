using UnityEngine;
using System.Collections;

public abstract class  EnemyBrain : MonoBehaviour
{
    [HideInInspector]
    public EnemyStats EnemyStats { get; protected set; }
    [HideInInspector]
    public CollisionTracker CollisionTracker { get; protected set; }
    [HideInInspector]
    public Rigidbody2D Rigidbody2d { get; protected set; }
    [HideInInspector]
    public AudioSource AudioSource { get; protected set; }
    [HideInInspector]
    public EnemyGotShot EnemyGotShot { get; protected set; }
    [HideInInspector]
    public SpriteRenderer SpriteRenderer { get; protected set; }
    [HideInInspector]
    public Raycaster Raycaster { get; protected set; }
    [HideInInspector]
    public EnemyAttackHandler EnemyAttackHandler { get; protected set; }
    [HideInInspector]
    public Animator Animator { get; protected set; }
    [HideInInspector]
    public MovementPattern[] MovementPatterns { get; protected set; }

    // This timer will help enemies that get stuck somewhere not to change directions too rapidly
    [HideInInspector]
    public float cannotChangeDirectionTime = 0f;

    protected virtual void OnEnable()
    {
        EnemyStats.EnemyDied += OnEnemyDiedStopAll;
    }

    protected virtual void OnDisable()
    {
        CancelInvoke();
        StopAllCoroutines();
        EnemyStats.EnemyDied -= OnEnemyDiedStopAll;
    }

    protected virtual void Awake()
    {
        EnemyGotShot = GetComponent<EnemyGotShot>();
        EnemyStats = GetComponent<EnemyStats>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        EnemyAttackHandler = GetComponent<EnemyAttackHandler>();
        CollisionTracker = GetComponentInChildren<CollisionTracker>();
    }

    protected virtual void Start()
    {
        Raycaster = GetComponentInChildren<Raycaster>();
        Rigidbody2d = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        AudioSource = GetComponent<AudioSource>();

        StartFacingRandomDirection();
    }

    public virtual void ChangeHorizontalDirection()
    {
        transform.localRotation = transform.localRotation.y == 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
    }

    public virtual void LoadEnemyBrain(Vector3 originalPosition, bool dead)
    {
        //is needed by some enemy brains to make changes when reloading an enemy
    }

    public void UpdateCollisionTracker()
    {
        CollisionTracker.collisions.Reset();
        CollisionTracker.TrackHorizontalCollisions();
        CollisionTracker.TrackVerticalCollisions(Rigidbody2d.velocity.y);
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
        var random = Random.Range(0f, 1f);
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

    // Gets called from animation sometimes!
    protected void CallMainAttack()
    {
        EnemyAttackHandler.Attack(EnemyStats.AttackData[0]);
    }

    protected void StartAttackAnimation()
    {
        Animator.SetTrigger("Attack");
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
