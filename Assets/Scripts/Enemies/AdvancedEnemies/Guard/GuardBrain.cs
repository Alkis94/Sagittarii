using UnityEngine;
using System.Collections;

public class GuardBrain : GroundEnemyBrain
{
    private Transform player;
    private Rigidbody2D rigidBody2d;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rigidBody2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        animator.SetFloat("VelocityY", rigidBody2d.velocity.y);
        animator.SetFloat("VelocityX", Mathf.Abs(rigidBody2d.velocity.x));
    }

    protected void FixedUpdate()
    {
        if (Mathf.Abs(transform.position.x - player.position.x) > 2)
        {
            Move(enemyStats.Speed);
            CheckCollisions();
        }
        else
        {
            rigidbody2d.velocity = new Vector2(0, rigidbody2d.velocity.y);
        }

        LookTowardsPlayer(transform, player.position);
    }

    protected  void CheckCollisions()
    {
        raycaster.UpdateRaycastOrigins();
        UpdateCollisionTracker();
        HandleWalkingAnimation();

        if ((collisionTracker.collisions.left || collisionTracker.collisions.right || collisionTracker.CloseToGroundEdge()) && Time.time > cannotChangeDirectionTime)
        {
            if(collisionTracker.collisions.below)
            {
                StartCoroutine(GuardJump());
            }
            cannotChangeDirectionTime = Time.time + 0.1f;
        }
    }

    private IEnumerator GuardJump()
    {
        Jump(0, 30);
        yield return new WaitForSeconds(0.2f);
        Jump(5 , 0);
    }
}