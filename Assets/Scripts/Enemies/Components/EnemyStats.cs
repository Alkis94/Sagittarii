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
    public event Action EnemyHealthChanged = delegate { };
    public event Action EnemyDied = delegate { };

    [SerializeField]
    private EnemyData enemyData;
    
    [Title("Enemy Stats")]
    [OdinSerialize] private int health = 10;
    [OdinSerialize] public float Speed { get; private set; } = 2;
    [OdinSerialize] public float DelayBeforeFirstAttack { get; private set; } = 3;
    

    [Title("Bools")]
    [OdinSerialize] public bool Damageable { get; set; } = true;
    [OdinSerialize] public bool Amputation { get; private set; } = false;
    [OdinSerialize] public bool ChangingDirections { get; private set; } = false;
    [ShowIf("@ ChangingDirections")]
    [OdinSerialize] public float ChangeDirectionFrequency { get; private set; } = 0;

    [Title("Drops")]
    [OdinSerialize] public int MinGoldGiven { get; private set; } = 5;
    [OdinSerialize] public int MaxGoldGiven { get; private set; } = 15;
    [OdinSerialize] public float GoldDropChance { get; private set; } = 0.1f;
    [OdinSerialize] public string Relic { get; private set; }
    [OdinSerialize] public float RelicDropChance { get; private set; } = 0.01f;


    [Title("Attacks")]
    [OdinSerialize] public List<AttackData> AttackData { get; private set; }

    public int Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
            EnemyHealthChanged?.Invoke();

            if (health <= 0)
            {
                EnemyDied?.Invoke();
            }
        }
    }

    private void Awake()
    {
        LoadFromJSON();
        RandomizeDelayBeforeFirstAttack();
    }

    private void LoadFromJSON()
    {
        health = enemyData.Health;
        Speed = enemyData.Speed;
        DelayBeforeFirstAttack = enemyData.DelayBeforeFirstAttack;
        Damageable = enemyData.Damageable;
        Amputation = enemyData.Amputation;
        ChangingDirections = enemyData.ChangingDirections;
        ChangeDirectionFrequency = enemyData.ChangeDirectionFrequency;
        MinGoldGiven = enemyData.MinGoldGiven;
        MaxGoldGiven = enemyData.MaxGoldGiven;
        GoldDropChance = enemyData.GoldDropChance;
        Relic = enemyData.Relic;
        RelicDropChance = enemyData.RelicDropChance;
     
    }

    private void RandomizeDelayBeforeFirstAttack()
    {
        float randomizer = UnityEngine.Random.Range(-2.8f, 0.5f);
        DelayBeforeFirstAttack += randomizer;
    }
}
