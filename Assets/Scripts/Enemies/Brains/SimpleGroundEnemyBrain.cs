using System.Collections;
using UnityEngine;

public class SimpleGroundEnemyBrain : EnemyBrain
{
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip attackSound;


    private Animator animator;
    private EnemyGroundMovement enemyGroundMovement;
    private CollisionTracker collisionTracker;
    private Rigidbody2D rigidbody2d;

    private int horizontalDirection = 1;
    private int animatorVelocityY_ID;
    private int animatorIsGrounded_ID;

    //This timer will help enemies that get stuck somewhere not to change directions too rapidly
    private float cannotChangeDirectionTime = 0f;


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

        audioSource.clip = attackSound;
        animatorIsGrounded_ID = Animator.StringToHash("IsGrounded");

        InvokeRepeating("StartAttackAnimation", enemyData.delayBeforeFirstAttack, enemyData.attackFrequency);  
        
        if(enemyData.jumpingBehaviour)
        {
            animatorVelocityY_ID = Animator.StringToHash("VelocityY");
            StartCoroutine(UpdateVelocityYForAnimator());
        }
    }

    private void Update()
    {
        if (enemyData.health > 0)
        {
            collisionTracker.collisions.Reset();
            collisionTracker.TrackHorizontalCollisions(horizontalDirection);
            collisionTracker.TrackVerticalCollisions(rigidbody2d.velocity.y);
            HandleWalkingAnimation();


            if ((collisionTracker.collisions.left || collisionTracker.collisions.right || collisionTracker.CloseToGroundEdge(horizontalDirection)) && Time.time > cannotChangeDirectionTime)
            {
                cannotChangeDirectionTime = Time.time + 0.1f;
                ChangeHorizontalDirection();
            }
        }
    }

    private void FixedUpdate()
    {
        if (enemyData.health > 0)
        {
            enemyGroundMovement.Move(enemyData.speed);
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
            enemyGroundMovement.Jump(horizontalDirection);
        }
    }

    protected override void ChangeHorizontalDirection()
    {
        horizontalDirection *= -1;
        enemyGroundMovement.ChangeHorizontalDirection();
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
