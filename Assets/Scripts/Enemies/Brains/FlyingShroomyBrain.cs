using UnityEngine;
using System.Collections;

public class FlyingShroomyBrain : FlyingEnemyBrain
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
        StartCoroutine(FlyupFreefallMovement());
        InvokeRepeating("StartAttackAnimation", EnemyStats.DelayBeforeFirstAttack, EnemyStats.AttackData[0].AttackFrequency);
    }

    private void Update()
    {
        UpdateCollisionTracker();
        Raycaster.UpdateRaycastOrigins();

        //if (collisionTracker.collisions.above && Time.time > cannotChangeDirectionTime)
        //{
            
        //    cannotChangeDirectionTime = Time.time + 0.05f;
        //}
    }

    private IEnumerator FlyupFreefallMovement ()
    {
        float fallTime = Time.time + 3;

        while (true)
        {
            if(fallTime < Time.time)
            {
                Rigidbody2d.velocity = Vector2.zero;
                Animator.SetBool("Flying", false);
                Rigidbody2d.gravityScale = 0.2f;
                yield return new WaitForSeconds(2f);
                fallTime = Time.time + 2;
                Rigidbody2d.gravityScale = 0;
                Animator.SetBool("Flying", true);
            }
            else
            {
                Rigidbody2d.velocity = new Vector2(0, 1.9f);
                yield return new WaitForFixedUpdate();
            }
        }
    }

}
