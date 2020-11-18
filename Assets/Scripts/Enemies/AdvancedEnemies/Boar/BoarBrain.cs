using UnityEngine;

public class BoarBrain : GroundEnemyBrain
{
    [HideInInspector]
    public StateMachine<BoarBrain> stateMachine;
    [HideInInspector]
    public BoarChargeState chargeState;
    [HideInInspector]
    public BoarIdleState idleState;
    [HideInInspector]
    public BoarStunnedState stunnedState;

    protected override void Awake()
    {
        base.Awake();
        idleState = new BoarIdleState(this);
        stunnedState = new BoarStunnedState(this);
        chargeState = new BoarChargeState(this);
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
        
        stateMachine = new StateMachine<BoarBrain>(this);
        stateMachine.ChangeState(idleState);
    }

    private void Update()
    {
        UpdateCollisionTracker();
        raycaster.UpdateRaycastOrigins();
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }
}
