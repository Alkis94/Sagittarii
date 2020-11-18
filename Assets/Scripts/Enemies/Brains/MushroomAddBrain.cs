public class MushroomAddBrain : EnemyBrain
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
        float randomizer = UnityEngine.Random.Range(0, enemyStats.AttackData[0].AttackFrequency - 2f);
        InvokeRepeating("StartAttackAnimation", enemyStats.DelayBeforeFirstAttack + randomizer, enemyStats.AttackData[0].AttackFrequency);
    }
}
