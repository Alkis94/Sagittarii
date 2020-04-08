using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public event Action EnemyHealthChanged = delegate { };
    public event Action EnemyDied = delegate { };

    [Header("Enemy Stats")]
    [SerializeField]
    private int health = 10;
    public float speed = 2;
    public float changeDirectionFrequency = 0;
    [Range(3,float.MaxValue)]
    public float delayBeforeFirstAttack = 3;
    public List<float> attackFrequencies;
    [HideInInspector]
    public int goldGiven;
    [SerializeField]
    private int giveSpecificAmountOfGold;

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

    private void Start()
    {
        if(giveSpecificAmountOfGold == 0)
        {
            goldGiven = health / 10;
        }
        else
        {
            goldGiven = giveSpecificAmountOfGold;
        }
    }

    private void RandomizeDelayBeforeFirstAttack()
    {
        float randomizer = UnityEngine.Random.Range(-2.5f, 1);
        delayBeforeFirstAttack += randomizer;
    }
}
