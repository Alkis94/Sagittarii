using UnityEngine;

public class SimpleGroundEnemyBrain : EnemyBrain
{
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip AttackSound;


    private Animator animator;
    private EnemyGroundMovement enemyGroundMovement;
    private CollisionTracker collisionTracker;
    private Rigidbody2D rigidbody2d;

    private int HorizontalDirection = 1;

    //private bool jumped = false;

    protected override void Awake()
    {
        base.Awake();
        enemyGroundMovement = GetComponent<EnemyGroundMovement>();
        collisionTracker = GetComponent<CollisionTracker>();
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
        rigidbody2d = GetComponent<Rigidbody2D>();
        audioSource.clip = AttackSound;
        
        InvokeRepeating("StartAttackAnimation", enemyData.DelayBeforeFirstAttack, enemyData.AttackFrequency);   
    }

    private void Update()
    {
        if (enemyData.Health > 1)
        {
            collisionTracker.collisions.Reset();
            collisionTracker.TrackHorizontalCollisions(HorizontalDirection);
            collisionTracker.TrackVerticalCollisions(rigidbody2d.velocity.y);
            enemyGroundMovement.Move(enemyData.Speed);
            HandleWalkingAnimation();


            if (collisionTracker.collisions.left || collisionTracker.collisions.right || collisionTracker.CloseToGroundEdge())
            {
                ChangeDirection();
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

    private void CallJump()
    {
        //Gets called from animation
        if (collisionTracker.collisions.below)
        {
            enemyGroundMovement.Jump(HorizontalDirection);
        }
    }

    protected override void ChangeDirection()
    {
        HorizontalDirection *= -1;
        enemyGroundMovement.ChangeDirection();
    }

    private void HandleWalkingAnimation()
    {
        if (collisionTracker.collisions.below)
        {

            animator.SetBool("IsMoving", true);
        }
        else
        {

            animator.SetBool("IsMoving", false);
        }
    }
}
