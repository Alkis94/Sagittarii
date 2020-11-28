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
        if (enemyStats.Health > 0)
        {
            Move(enemyStats.Speed);
            raycaster.UpdateRaycastOrigins();
            UpdateCollisionTracker();

            if (CheckHorizontalGround() && Time.time > cannotChangeDirectionTime)
            {
                cannotChangeDirectionTime = Time.time + 1.5f;
                ChangeHorizontalDirection();
            }
            animator.SetFloat("VelocityY", rigidbody2d.velocity.y);
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Jumping") && rigidbody2d.velocity.y >= 6)
        {
            animator.SetTrigger("Falling");
        }
    }

    

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(!enabled)
        {
            return;
        }

        if (enemyStats.Health > 0)
        {
            if (rigidbody2d.velocity.y <= 0 && collisionTracker.collisions.below)
            {
                animator.SetTrigger("Jumping");
                Jump(1, 8);
                float randomDelay = Random.Range(0.25f, 0.75f); 
                Invoke("CallMainAttack",randomDelay);
            }
            else if(collisionTracker.collisions.above)
            {
                rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, -1);
            }
        }
    }
}
