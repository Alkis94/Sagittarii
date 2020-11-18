using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

[ShowOdinSerializedPropertiesInInspector]
public class EnemyStats : SerializedMonoBehaviour , IDamageable
{
    public static event Action<DamageSource> OnEnemyWasKilled = delegate { };
    public event Action EnemyHealthChanged = delegate { };
    public event Action<DamageType> EnemyDied = delegate { };

    [SerializeField]
    private EnemyData enemyData;
    [OdinSerialize] public string EnemyName{ get; private set; }

    [Title("Enemy Stats")]
    [OdinSerialize] private int health = 10;
    [OdinSerialize] public float Speed { get; private set; } = 2;
    [OdinSerialize] public float DelayBeforeFirstAttack { get; private set; } = 0.5f;
    

    [Title("Bools")]
    [OdinSerialize] public bool Damageable { get; set; } = true;
    [OdinSerialize] public bool Amputation { get; private set; } = false;
    [OdinSerialize] public bool HasBlood { get; private set; } = true;
    [OdinSerialize] public bool HasCriticalDeath { get; private set; } = false;
    [OdinSerialize] public bool ShakeBeforeDeath { get; private set; } = false;

    [Title("Drops")]
    [OdinSerialize] public int MinGoldGiven { get; private set; } = 5;
    [OdinSerialize] public int MaxGoldGiven { get; private set; } = 15;
    [OdinSerialize] public float GoldDropChance { get; private set; } = 0.2f;
    [OdinSerialize] public List<string> Relics { get; private set; }
    [OdinSerialize] public List<float> RelicDropChance { get; private set; }


    [Title("Attacks")]
    [OdinSerialize] public List<EnemyAttackData> AttackData { get; private set; }

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

    public void ApplyDamage(int damage, DamageSource damageSource, DamageType damageType = DamageType.normal)
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
        EnemyName = enemyData.EnemyName;
        health = enemyData.Health;
        Speed = enemyData.Speed;
        Amputation = enemyData.Amputation;
        HasBlood = enemyData.HasBlood;
        HasCriticalDeath = enemyData.HasCriticalDeath;
        ShakeBeforeDeath = enemyData.ShakeBeforeDeath;
        MinGoldGiven = enemyData.MinGoldGiven;
        MaxGoldGiven = enemyData.MaxGoldGiven;
        GoldDropChance = enemyData.GoldDropChance;
        Relics = enemyData.Relics;
        RelicDropChance = enemyData.RelicDropChance;
    }

    private void RandomizeDelayBeforeFirstAttack()
    {
        float randomizer = UnityEngine.Random.Range(0, 1.5f);
        DelayBeforeFirstAttack += randomizer;
    }
}
