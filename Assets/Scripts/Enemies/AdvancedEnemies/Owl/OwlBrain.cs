using UnityEngine;
using System.Collections;
using StateMachineNamespace;

public class OwlBrain : EnemyBrain
{
    [HideInInspector]
    public StateMachine<OwlBrain> stateMachine;
    [HideInInspector]
    public OwlWonderState wonderState;
    [HideInInspector]
    public OwlHuntAttackState huntAttackState;

    [HideInInspector]
    public int horizontalDirection = 1;
    [HideInInspector]
    public int verticalDirection = 1;



    protected override void Awake()
    {
        base.Awake();
        wonderState = new OwlWonderState(this);
        huntAttackState = new OwlHuntAttackState(this);
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
        MovementPatterns = GetComponents<MovementPattern>();

        stateMachine = new StateMachine<OwlBrain>(this);
        stateMachine.ChangeState(wonderState);
    }


    private void Update()
    {
        UpdateCollisionTracker();
        raycaster.UpdateRaycastOrigins();

        if (collisionTracker.collisions.left || collisionTracker.collisions.right && Time.time > cannotChangeDirectionTime)
        {
            ChangeHorizontalDirection();
            cannotChangeDirectionTime = Time.time + 0.05f;
        }

        if (collisionTracker.collisions.above || collisionTracker.collisions.below && Time.time > cannotChangeDirectionTime)
        {
            verticalDirection *= -1;
            cannotChangeDirectionTime = Time.time + 0.05f;
        }
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    private void UpdateCollisionTracker()
    {
        collisionTracker.collisions.Reset();
        collisionTracker.TrackHorizontalCollisions();
        collisionTracker.TrackVerticalCollisions(rigidbody2d.velocity.y);
    }

    protected override void ChangeHorizontalDirection()
    {
        horizontalDirection *= -1;
        if (horizontalDirection == -1)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (horizontalDirection == 1)
        {

            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }


    //Called from Animation
    public void CallAttack()
    {
        AttackPatterns.Attack(enemyStats.AttackData[0]);
    }

    //Called from Animation
    public void CallDive()
    {

        rigidbody2d.AddForce(new Vector2(0, -8), ForceMode2D.Impulse);
    }

    public void CallMoveBack()
    {
        rigidbody2d.AddForce(new Vector2(0, 16), ForceMode2D.Impulse);
    }

}
