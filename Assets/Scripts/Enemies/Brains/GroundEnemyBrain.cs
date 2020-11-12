using UnityEngine;

public abstract class GroundEnemyBrain : EnemyBrain
{
    
    [HideInInspector]
    public EnemyGroundMovement enemyGroundMovement;

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

    protected override void Start()
    {
        base.Start();
    }

    public bool CheckHorizontalGround ()
    {
        if (collisionTracker.collisions.left || collisionTracker.collisions.right || collisionTracker.CloseToGroundEdge())
        {
            return true;
        }

        return false;
    }

    //public override void ChangeHorizontalDirection()
    //{
    //    enemyGroundMovement.ChangeHorizontalDirection();
    //}

    protected void HandleWalkingAnimation()
    {
        if (collisionTracker.collisions.below)
        {

            animator.SetBool("IsGrounded", true);
        }
        else
        {

            animator.SetBool("IsGrounded", false);
        }
    }

}
