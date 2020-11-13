using UnityEngine;
using System.Collections;

public class FatshroomBrain : EnemyBrain
{

    private Transform player;

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
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("StartAttackAnimation", enemyStats.DelayBeforeFirstAttack, enemyStats.AttackData[0].AttackFrequency);
    }

    private void FixedUpdate()
    {
        LookTowardsPlayer(transform, player.position);
    }
}
