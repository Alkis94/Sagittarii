using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int health = 10;
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
    public bool jumpingBehaviour = false;

    [Header("Bools")]
    public bool damageable = true;

    [Header("Relic Drop")]
    public GameObject Relic;

    private void Awake()
    {
        RandomizeDelayBeforeFirstAttack();
    }

    private void Start()
    {
        if(giveSpecificAmountOfGold == 0)
        {
            goldGiven = health % 10 == 0 ? health * 2 / 10 : (health - (health % 10) + 10) * 2 / 10;
        }
        else
        {
            goldGiven = giveSpecificAmountOfGold;
        }
    }

    private void RandomizeDelayBeforeFirstAttack()
    {
        float randomizer = Random.Range(-2.5f, 1);
        delayBeforeFirstAttack += randomizer;
    }
}
