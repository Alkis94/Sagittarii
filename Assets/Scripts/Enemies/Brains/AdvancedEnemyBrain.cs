using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public abstract class AdvancedEnemyBrain : EnemyBrain
{
    [HideInInspector]
    public AttackPattern[] AttackPatterns { get; protected set; }
    public MovementPattern[] MovementPatterns { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        AttackPatterns = GetComponents<AttackPattern>();
        MovementPatterns = GetComponents<MovementPattern>();
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


}
