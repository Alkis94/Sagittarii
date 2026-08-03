using UnityEngine;
using System.Collections;

public class DeerBrain : GroundEnemyBrain
{

    protected override void Awake()
    {
        base.Awake();
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
    }

    protected void FixedUpdate()
    {
        if (EnemyStats.Health > 0)
        {
            Move(EnemyStats.Speed);
            Raycaster.UpdateRaycastOrigins();
            UpdateCollisionTracker();

            if (CheckHorizontalGround() && Time.time > cannotChangeDirectionTime)
            {
                cannotChangeDirectionTime = Time.time + 1.5f;
                ChangeHorizontalDirection();
            }
            Animator.SetFloat("VelocityY", Rigidbody2d.velocity.y);
        }

        if(Animator.GetCurrentAnimatorStateInfo(0).IsName("Jumping") && Rigidbody2d.velocity.y >= 6)
        {
            Animator.SetTrigger("Falling");
        }
    }

    

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(!enabled)
        {
            return;
        }

        if (EnemyStats.Health > 0)
        {
            if (Rigidbody2d.velocity.y <= 0 && CollisionTracker.collisions.below)
            {
                Animator.SetTrigger("Jumping");
                Jump(1, 8);
                float randomDelay = Random.Range(0.25f, 0.75f); 
                Invoke("CallMainAttack",randomDelay);
            }
            else if(CollisionTracker.collisions.above)
            {
                Rigidbody2d.velocity = new Vector2(Rigidbody2d.velocity.x, -1);
            }
        }
    }
}
