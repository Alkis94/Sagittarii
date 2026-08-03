
public class FlowershroomBrain : EnemyBrain
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
        InvokeRepeating("StartAttackAnimation", EnemyStats.DelayBeforeFirstAttack, EnemyStats.AttackData[0].AttackFrequency);
    }
}
