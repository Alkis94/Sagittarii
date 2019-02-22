using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int Health;
    public float Speed;
    public float ChangeDirectionFrequency;
    public float DelayBeforeFirstAttack;
    [HideInInspector]
    public int GoldGiven;

    

    [Header("Behaviour Bools")]
    public bool ChangingDirections;
    public bool AttackIsDirectionDependant;
    public bool JumpingBehaviour;


    [Header("Attack Stats")]
    public List<float> projectileRotations;
    public float ProjectileSpeed;
    public float ProjectileDestroyDelay;
    public float AttackFrequency;
    public Vector3 ProjectileSpawnPositionOffset;

    [Header("Item Drop")]
    public GameObject SpecialItem;

    private void Start()
    {
        GoldGiven = Health % 10 == 0? Health * 2 : (Health - (Health % 10) + 10) * 2;
    }


}
