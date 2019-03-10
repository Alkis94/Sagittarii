using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AttackData", menuName = "AttackData", order = 1)]
public class AttackData : ScriptableObject
{
    public bool attackIsDirectionDependant = false;
    public int damage = 10;
    public GameObject projectile;
    public float projectileSpeed = 2;
    public float projectileDestroyDelay = 10f;
    public Vector3 projectileSpawnPositionOffset = Vector3.zero;
}
