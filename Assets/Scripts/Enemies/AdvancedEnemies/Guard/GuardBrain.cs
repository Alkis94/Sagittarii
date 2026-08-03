using UnityEngine;
using System.Collections;

public class GuardBrain : GroundEnemyBrain
{
    private Transform player;
    private Rigidbody2D rigidBody2d;
    public float ExtraDinstance { get; set; } = 0;

    protected override void OnEnable()
    {
        base.OnEnable();
        rigidBody2d = GetComponent<Rigidbody2D>();
        rigidBody2d.bodyType = RigidbodyType2D.Dynamic;
    }

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        Animator.SetFloat("VelocityY", rigidBody2d.velocity.y);
        Animator.SetFloat("VelocityX", Mathf.Abs(rigidBody2d.velocity.x));
    }

    protected void FixedUpdate()
    {
        CheckCollisions();

        if (Mathf.Abs(transform.position.x - player.position.x) > 2 + ExtraDinstance)
        {
            Move(EnemyStats.Speed);
            CheckForJump();
        }
        else
        {
            Rigidbody2d.velocity = new Vector2(0, Rigidbody2d.velocity.y);
        }

        LookTowardsPlayer(transform, player.position);
    }

    protected  void CheckCollisions()
    {
        Raycaster.UpdateRaycastOrigins();
        UpdateCollisionTracker();
        HandleWalkingAnimation();
    }

    private void CheckForJump()
    {
        if ((CollisionTracker.collisions.left || CollisionTracker.collisions.right || CollisionTracker.CloseToGroundEdge()) && Time.time > cannotChangeDirectionTime)
        {
            if (CollisionTracker.collisions.below)
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