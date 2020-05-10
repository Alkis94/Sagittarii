using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(fileName = "AttackData", menuName = "AttackData", order = 1)]
public class AttackData : ScriptableObject
{

    //Attack Type
    [ShowInInspector] public AttackTypeEnum AttackType { get; private set; } = AttackTypeEnum.perimetrical;
    [ShowIf("@ AttackType == AttackTypeEnum.perimetrical")]
    [ShowInInspector] public bool AttackIsDirectionDependant { get; private set; } = false;

    //Attack Stats
    [ShowInInspector] public int ProjectileAmount { get; private set; } = 1;
    [ShowInInspector] public int ConsecutiveAttacks { get; private set; } = 1;
    [ShowIf("@ ConsecutiveAttacks > 1")]
    [ShowInInspector] public float ConsecutiveAttackDelay { get; private set; } = 0;
    [ShowInInspector] public int Damage { get; private set; } = 10;
    [ShowInInspector] public float ProjectileSpeed { get; private set; } = 5;
    [ShowInInspector] public float ProjectileDestroyDelay { get; private set; } = 10f;
    [ShowInInspector] public List<float> ProjectileRotations { get; private set; }
    [ShowInInspector] public List<Vector3> ProjectileSpawnPositionOffset { get; private set; }
    [ShowInInspector] public Vector3 UniversalSpawnPositionOffset { get; private set; } 

    //Parts
    [ShowInInspector] public GameObject Projectile { get; private set; }
    [ShowInInspector] public AudioClip AttackSound { get; private set; } = null;
    

    //randomness
    [ShowInInspector] public bool Randomness { get; private set; } = false;
    [ShowIf("@ Randomness")]
    [ShowInInspector] public float RandomHorizontalFactorMin { get; private set; } = 0;
    [ShowIf("@ Randomness")]
    [ShowInInspector] public float RandomHorizontalFactorMax { get; private set; } = 0;
    [ShowIf("@ Randomness")]
    [ShowInInspector] public float RandomVerticalFactorMin { get; private set; } = 0;
    [ShowIf("@ Randomness")]
    [ShowInInspector] public float RandomVerticalFactorMax { get; private set; } = 0;
    [ShowIf("@ Randomness")]
    [ShowInInspector] public float RandomRotationFactorMin { get; private set; } = 0;
    [ShowIf("@ Randomness")]
    [ShowInInspector] public float RandomRotationFactorMax { get; private set; } = 0;
}
