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
        StartCoroutine(UpdateVelocityYForAnimator());
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }


    private void CallJump()
    {
        //Gets called from animation
        if (collisionTracker.collisions.below)
        {
            enemyGroundMovement.Jump(4,5);
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
