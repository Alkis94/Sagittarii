using System.Collections;
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

    private int animatorVelocityY_ID;
    private int animatorIsGrounded_ID;
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

        animatorIsGrounded_ID = Animator.StringToHash("IsGrounded");

        InvokeRepeating("StartAttackAnimation", enemyData.DelayBeforeFirstAttack, enemyData.AttackFrequency);  
        
        if(enemyData.JumpingBehaviour)
        {
            animatorVelocityY_ID = Animator.StringToHash("VelocityY");
            StartCoroutine(UpdateVelocityYForAnimator());
        }
    }

    private void Update()
    {
        if (enemyData.Health > 0)
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

            animator.SetBool(animatorIsGrounded_ID, true);
        }
        else
        {

            animator.SetBool(animatorIsGrounded_ID, false);
        }
    }

    IEnumerator UpdateVelocityYForAnimator()
    {
        while(true)
        {
            animator.SetFloat(animatorVelocityY_ID, rigidbody2d.velocity.y);
            yield return null;
        }
    }
}
