using UnityEngine;

public class SimpleGroundEnemyBrain : EnemyBrain
{
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip AttackSound;


    private Animator animator;
    private EnemyGroundMovement enemyGroundMovement;
    private MovementCollisionHandler movementCollisionHandler;

    //private bool jumped = false;

    protected override void Awake()
    {
        base.Awake();
        enemyGroundMovement = GetComponent<EnemyGroundMovement>();
        movementCollisionHandler = GetComponent<MovementCollisionHandler>();
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
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = AttackSound;
        InvokeRepeating("StartAttackAnimation", enemyData.DelayBeforeFirstAttack, enemyData.AttackFrequency);   
    }

    private void Update()
    {
        if (enemyData.Health > 1)
        { 
            enemyGroundMovement.CalculateMovement(enemyData.Speed,false);

            if (movementCollisionHandler.collisions.left || movementCollisionHandler.collisions.right || movementCollisionHandler.CloseToGroundEdge())
            {
                ChangeDirection();
            }

            if (movementCollisionHandler.collisions.above || movementCollisionHandler.collisions.below)
            {
                {
                    enemyGroundMovement.velocity.y = 0;
                }
            }
        }
    }

    private void StartAttackAnimation()
    {
        animator.SetTrigger("Attack");
        audioSource.Play();
    }

    private void CallAttack()
    {
        //Gets called from animation
        attackPatern.Attack(enemyData);
    }



    protected override void ChangeDirection()
    {
        enemyGroundMovement.ChangeDirection();
    }
}
