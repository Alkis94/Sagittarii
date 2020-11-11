using System.Collections;
using UnityEngine;

public class WolfBrain : GroundEnemyBrain
{
    
    private int animatorVelocityY_ID;


    protected override void Awake()
    {
        base.Awake();
        enemyGroundMovement = GetComponent<EnemyGroundMovement>();
        collisionTracker = GetComponentInChildren<CollisionTracker>();
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
        animatorVelocityY_ID = Animator.StringToHash("VelocityY");
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        animator.SetFloat(animatorVelocityY_ID, rigidbody2d.velocity.y);
    }

    //Gets called from animation
    private void CallJump()
    {
        if (collisionTracker.collisions.below)
        {
            enemyGroundMovement.Jump(4,5);
            animator.SetFloat(animatorVelocityY_ID, rigidbody2d.velocity.y);
        }
    }

}
