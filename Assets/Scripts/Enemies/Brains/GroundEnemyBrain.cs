using System.Collections;
using UnityEngine;

public class GroundEnemyBrain : EnemyBrain
{
    
    protected EnemyGroundMovement enemyGroundMovement;
    protected int animatorIsGrounded_ID;

    protected override void Awake()
    {
        base.Awake();
        enemyGroundMovement = GetComponent<EnemyGroundMovement>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    public override void LoadEnemyBrain(Vector3 originalPosition, bool dead)
    {

    }

    protected override void Start()
    {
        base.Start();
        animatorIsGrounded_ID = Animator.StringToHash("IsGrounded");
        InvokeRepeating("StartAttackAnimation", enemyStats.DelayBeforeFirstAttack, enemyStats.AttackData[0].AttackFrequency);  
    }

    protected virtual void Update()
    {
        if (enemyStats.Health > 0)
        {
            CheckCollisions();
        }
    }

    protected virtual void CheckCollisions()
    {
        
        collisionTracker.collisions.Reset();
        collisionTracker.TrackHorizontalCollisions();
        collisionTracker.TrackVerticalCollisions(rigidbody2d.velocity.y);
        HandleWalkingAnimation();

        if ((collisionTracker.collisions.left || collisionTracker.collisions.right || collisionTracker.CloseToGroundEdge()) && Time.time > cannotChangeDirectionTime)
        {
            cannotChangeDirectionTime = Time.time + 0.1f;
            ChangeHorizontalDirection();
        }
    }

    protected virtual void FixedUpdate()
    {
        if (enemyStats.Health > 0)
        {
            enemyGroundMovement.Move(enemyStats.Speed);
        }
    }

    protected override void ChangeHorizontalDirection()
    {
        enemyGroundMovement.ChangeHorizontalDirection();
    }

    protected virtual void HandleWalkingAnimation()
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

}
