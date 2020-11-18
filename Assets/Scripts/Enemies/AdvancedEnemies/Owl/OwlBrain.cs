using UnityEngine;

public class OwlBrain : FlyingEnemyBrain
{
    [HideInInspector]
    public StateMachine<OwlBrain> stateMachine;
    [HideInInspector]
    public OwlWonderState wonderState;
    [HideInInspector]
    public OwlHuntAttackState huntAttackState;

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

        if (HorizontalCollisions() && Time.time > cannotChangeDirectionTime)
        {
            ChangeHorizontalDirection();
            cannotChangeDirectionTime = Time.time + 0.05f;
        }

        if (VerticalCollisions() && Time.time > cannotChangeDirectionTime)
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

    //Called from Animation
    public void CallAttack()
    {
        enemyAttackHandler.Attack(enemyStats.AttackData[0]);
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
