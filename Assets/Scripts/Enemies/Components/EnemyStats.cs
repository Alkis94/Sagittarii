using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

[ShowOdinSerializedPropertiesInInspector]
public class EnemyStats : SerializedMonoBehaviour
{
    public static event Action<DamageSource> OnEnemyWasKilled = delegate { };
    public event Action EnemyHealthChanged = delegate { };
    public event Action<DamageType> EnemyDied = delegate { };

    [SerializeField]
    private EnemyData enemyData;
    
    [Title("Enemy Stats")]
    [OdinSerialize] private int health = 10;
    [OdinSerialize] public float Speed { get; private set; } = 2;
    [OdinSerialize] public float DelayBeforeFirstAttack { get; private set; } = 3;
    

    [Title("Bools")]
    [OdinSerialize] public bool Damageable { get; set; } = true;
    [OdinSerialize] public bool Amputation { get; private set; } = false;
    [OdinSerialize] public bool HasBlood { get; private set; } = true;
    [OdinSerialize] public bool HasCriticalDeath { get; private set; } = false;
    [OdinSerialize] public bool ShakeBeforeDeath { get; private set; } = false;
    [OdinSerialize] public bool ChangingDirections { get; private set; } = false;

    [ShowIf("@ ChangingDirections")]
    [OdinSerialize] public float ChangeDirectionFrequency { get; private set; } = 0;

    [Title("Drops")]
    [OdinSerialize] public int MinGoldGiven { get; private set; } = 5;
    [OdinSerialize] public int MaxGoldGiven { get; private set; } = 15;
    [OdinSerialize] public float GoldDropChance { get; private set; } = 0.1f;
    [OdinSerialize] public List<string> Relics { get; private set; }
    [OdinSerialize] public List<float> RelicDropChance { get; private set; }


    [Title("Attacks")]
    [OdinSerialize] public List<AttackData> AttackData { get; private set; }

    private DamageSource lastDamageSource;
    private DamageType lastDamageType;

    public int Health
    {
        get
        {
            return health;
        }

        private set
        {
            health = value;
            EnemyHealthChanged?.Invoke();

            if (health <= 0)
            {
                EnemyDied?.Invoke(lastDamageType);
                OnEnemyWasKilled?.Invoke(lastDamageSource);
            }
        }
    }

    public void ApplyDamage(int damage,DamageType damageType, DamageSource damageSource)
    {
        lastDamageType = damageType;
        lastDamageSource = damageSource;

        if(damageType == DamageType.normal)
        {
            Health -= damage;
        }
        else
        {
            Health -= damage * 2;
        }

    }

    private void Awake()
    {
        LoadFromData();
        RandomizeDelayBeforeFirstAttack();
    }

    private void LoadFromData()
    {
        health = enemyData.Health;
        Speed = enemyData.Speed;
        DelayBeforeFirstAttack = enemyData.DelayBeforeFirstAttack;
        Damageable = enemyData.Damageable;
        Amputation = enemyData.Amputation;
        HasBlood = enemyData.HasBlood;
        HasCriticalDeath = enemyData.HasCriticalDeath;
        ShakeBeforeDeath = enemyData.ShakeBeforeDeath;
        ChangingDirections = enemyData.ChangingDirections;
        ChangeDirectionFrequency = enemyData.ChangeDirectionFrequency;
        MinGoldGiven = enemyData.MinGoldGiven;
        MaxGoldGiven = enemyData.MaxGoldGiven;
        GoldDropChance = enemyData.GoldDropChance;
        Relics = enemyData.Relics;
        RelicDropChance = enemyData.RelicDropChance;
     
    }

    private void RandomizeDelayBeforeFirstAttack()
    {
        float randomizer = UnityEngine.Random.Range(-2.8f, 1f);
        DelayBeforeFirstAttack += randomizer;
    }
}
