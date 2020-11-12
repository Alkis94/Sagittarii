using UnityEngine;
using System.Collections;

public class GuardBrain : EnemyBrain
{
    private Transform player;
    private EnemyGroundMovement enemyGroundMovement;
    protected int animatorIsGrounded_ID;
    private int animatorVelocityY_ID;
    private int animatorVelocityX_ID;
    private Rigidbody2D rigidBody2d;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animatorIsGrounded_ID = Animator.StringToHash("IsGrounded");
        enemyGroundMovement = GetComponent<EnemyGroundMovement>();
        animatorVelocityX_ID = Animator.StringToHash("VelocityX");
        animatorVelocityY_ID = Animator.StringToHash("VelocityY");
        rigidBody2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        animator.SetFloat(animatorVelocityY_ID, rigidBody2d.velocity.y);
        animator.SetFloat(animatorVelocityX_ID, Mathf.Abs(rigidBody2d.velocity.x));
    }

    protected void FixedUpdate()
    {
        if (Mathf.Abs(transform.position.x - player.position.x) > 2)
        {
            enemyGroundMovement.Move(enemyStats.Speed);
            CheckCollisions();
        }
        else
        {
            rigidbody2d.velocity = new Vector2(0, rigidbody2d.velocity.y);
        }

        if(transform.position.x > player.position.x)
        {
            transform.localRotation =  Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.localRotation =  Quaternion.Euler(0, 0, 0);
        }
    }

    protected  void CheckCollisions()
    {
        raycaster.UpdateRaycastOrigins();
        collisionTracker.collisions.Reset();
        collisionTracker.TrackHorizontalCollisions();
        collisionTracker.TrackVerticalCollisions(rigidbody2d.velocity.y);
        HandleWalkingAnimation();

        if ((collisionTracker.collisions.left || collisionTracker.collisions.right || collisionTracker.CloseToGroundEdge()) && Time.time > cannotChangeDirectionTime)
        {
            if(collisionTracker.collisions.below)
            {
                StartCoroutine(Jump());
            }
            cannotChangeDirectionTime = Time.time + 0.1f;
        }
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

    private IEnumerator Jump()
    {
        enemyGroundMovement.Jump(0, 30);
        yield return new WaitForSeconds(0.2f);
        enemyGroundMovement.Jump(5 , 0);
    }
}