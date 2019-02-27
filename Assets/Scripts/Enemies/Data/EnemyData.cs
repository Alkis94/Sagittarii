using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract  class EnemyData : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int health;
    public float speed;
    public float changeDirectionFrequency = 0;
    public float delayBeforeFirstAttack;
    [HideInInspector]
    public int goldGiven;

    

    [Header("Behaviour Bools")]
    public bool changingDirections;
    public bool attackIsDirectionDependant;
    public bool jumpingBehaviour;


    [Header("Attack Stats")]
    public List<float> projectileRotations;
    public float projectileSpeed;
    public float projectileDestroyDelay;
    public float attackFrequency;
    public Vector3 projectileSpawnPositionOffset;

    [Header("Relic Drop")]
    public GameObject Relic;

    protected virtual void Start()
    {
        goldGiven = health % 10 == 0? health * 2/10 : (health - (health % 10) + 10) * 2/10;
    }


}
