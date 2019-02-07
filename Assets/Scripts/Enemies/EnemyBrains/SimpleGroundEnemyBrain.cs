using UnityEngine;

public class SimpleGroundEnemyBrain : EnemyBrain
{
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip AttackSound;

    private BoxCollider2D boxCollider2d;
    private Animator animator;
    private GroundEnemyMovement groundEnemyMovement;
    private MovementCollisionHandler movementCollisionHandler;

    protected override void Awake()
    {
        base.Awake();
        groundEnemyMovement = GetComponent<GroundEnemyMovement>();
        movementCollisionHandler = GetComponent<MovementCollisionHandler>();
    }

    private void OnEnable()
    {
        enemyCollision.OnGroundCollision += ChangeDirection;
        enemyCollision.OnDeath += CancelInvoke;
    }

    private void OnDisable()
    {
        enemyCollision.OnGroundCollision -= ChangeDirection;
        enemyCollision.OnDeath -= CancelInvoke;
    }

    protected override void Start()
    {
        base.Start();
        
        
        animator = GetComponent<Animator>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = AttackSound;
        InvokeRepeating("StartAttackAnimation", enemyData.DelayBeforeFirstAttack, enemyData.AttackFrequency);   
    }

    private void Update()
    {
        if (enemyData.Health > 1)
        {
            groundEnemyMovement.Move(enemyData.Speed,false);
        }

        if (movementCollisionHandler.collisions.left || movementCollisionHandler.collisions.right)
        {
            ChangeDirection();
        }
    }

    private void StartAttackAnimation()
    {
        animator.SetTrigger("Attack");
        audioSource.Play();
    }

    private void CallAttack()
    {
        attackPatern.Attack(enemyData);
    }

    protected override void ChangeDirection()
    {
        groundEnemyMovement.ChangeDirection();
    }

   
}
