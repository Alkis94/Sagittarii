using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

[ShowOdinSerializedPropertiesInInspector]
public class EnemyData : MonoBehaviour
{
    public event Action EnemyHealthChanged = delegate { };
    public event Action EnemyDied = delegate { };

    [Header("Enemy Stats")]
    [SerializeField]
    private int health = 10;
    public float speed = 2;
    public float changeDirectionFrequency = 0;
    [Range(1,float.MaxValue)]
    public float delayBeforeFirstAttack = 2;
    public List<float> attackFrequencies;

    [OdinSerialize] public int minGoldGiven { get; private set; } = 5;
    [OdinSerialize] public int maxGoldGiven { get; private set; } = 15;
    [OdinSerialize] public float goldDropChance { get; private set; } = 0.1f;


    [Header("Behaviour Bools")]
    public bool changingDirections = false;



    [Header("Bools")]
    public bool damageable = true;
    public bool amputation = false;

    [Header("Relic Drop")]
    public GameObject Relic;

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
        float randomizer = UnityEngine.Random.Range(-1.5f, 0.5f);
        delayBeforeFirstAttack += randomizer;
    }
}
