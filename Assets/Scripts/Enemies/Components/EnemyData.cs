using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

[ShowOdinSerializedPropertiesInInspector]
public class EnemyData : SerializedMonoBehaviour
{
    public event Action EnemyHealthChanged = delegate { };
    public event Action EnemyDied = delegate { };

    [Title("Enemy Stats")]
    [OdinSerialize] private int health = 10;
    [OdinSerialize] public float speed { get; private set; } = 2;
    
    [OdinSerialize] public float delayBeforeFirstAttack { get; private set; } = 3;
    [OdinSerialize] public List<AttackData> attackData { get; private set; }

    [Title("Behaviour Bools")]
    [OdinSerialize] public bool changingDirections { get; private set; } = false;
    [ShowIf("@ changingDirections")]
    [OdinSerialize] public float changeDirectionFrequency { get; private set; } = 0;

    [Title("Bools")]
    [OdinSerialize] public bool damageable { get; set; } = true;
    [OdinSerialize] public bool amputation { get; private set; } = false;

    [Title("Drops")]
    [OdinSerialize] public int minGoldGiven { get; private set; } = 5;
    [OdinSerialize] public int maxGoldGiven { get; private set; } = 15;
    [OdinSerialize] public float goldDropChance { get; private set; } = 0.1f;
    [OdinSerialize] public GameObject Relic { get; private set; }

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
        RandomizeDelayBeforeFirstAttack();
    }

    private void RandomizeDelayBeforeFirstAttack()
    {
        float randomizer = UnityEngine.Random.Range(-2.8f, 0.5f);
        delayBeforeFirstAttack += randomizer;
    }
}
