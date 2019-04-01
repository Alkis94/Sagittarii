using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int health = 10;
    public float speed = 2;
    public float changeDirectionFrequency = 0;
    public float delayBeforeFirstAttack = 3;
    public List<float> attackFrequencies;
    [HideInInspector]
    public int goldGiven;

    [Header("Behaviour Bools")]
    public bool changingDirections = false;
    public bool jumpingBehaviour = false;

    [Header("Relic Drop")]
    public GameObject Relic;

    protected virtual void Start()
    {
        goldGiven = health % 10 == 0? health * 2/10 : (health - (health % 10) + 10) * 2/10;
    }


}
