using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[CreateAssetMenu(fileName = "AttackData", menuName = "AttackData", order = 1)]
public class AttackData : ScriptableObject
{
    public AttackType attackType = AttackType.perimetrical;
    public int projectileAmount = 1;
    [ShowIf("@ attackType == AttackType.perimetrical")]
    public bool attackIsDirectionDependant = false;
    public bool randomness = false;
    public float randomHorizontalFactorMin = 0;
    public float randomHorizontalFactorMax = 0;
    public float randomVerticalFactorMin = 0;
    public float randomVerticalFactorMax = 0;
    public int damage = 10;
    public GameObject projectile;
    public float projectileSpeed = 2;
    public float projectileDestroyDelay = 10f;
    public List<float> projectileRotations;
    public List<Vector3> projectileSpawnPositionOffset;
    public Vector3 spawnPositionOffset = Vector3.zero;
    public AudioClip attackSound = null;
}
