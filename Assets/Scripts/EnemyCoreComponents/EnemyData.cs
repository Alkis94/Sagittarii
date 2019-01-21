using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{

    public int Health;

    public List<Vector2> projectileDirections;

    public bool ChangingDirections;
    public bool AttackIsDirectionDependant;

    public float Speed;
    public float ProjectileSpeed;
    public float ProjectileDestroyDelay;
    public float AttackFrequency;
    public float ChangeDirectionFrequency;
    public float DelayBeforeFirstAttack;

    public GameObject projectile;
    public GameObject SpecialItem;

    public Vector3 ProjectileSpawnPositionOffset;


}
