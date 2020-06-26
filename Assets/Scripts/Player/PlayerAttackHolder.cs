using UnityEngine;
using System.Collections.Generic;

public class PlayerAttackHolder
{
    //Attack Type
     public ProjectileMovementTypeEnum ProjectileMovementType { get; set; } = ProjectileMovementTypeEnum.straight;
     public FunctionMovementTypeEnum FunctionMovementType { get; set; } = FunctionMovementTypeEnum.sin;
     public AttackTypeEnum AttackType { get; set; } = AttackTypeEnum.perimetrical;

     public bool AttackIsDirectionDependant { get; set; } = false;
     public int ProjectileAmount { get; set; } = 1;
     public int ConsecutiveAttacks { get; set; } = 1;

     public float ConsecutiveAttackDelay { get; set; } = 0;
     public float AttackFrequency { get; set; } = 0;
     public int Damage { get; set; } = 0;
     public float ProjectileSpeed { get; set; } = 10;
     public float ProjectileDestroyDelay { get; set; } = 10f;
    
     public List<float> ProjectileRotations { get; set; } = new List<float>();
     public List<Vector3> ProjectileSpawnPositionOffset { get; set; } = new List<Vector3>();
     public Vector3 UniversalSpawnPositionOffset { get; set; }
    
     public float RandomHorizontalFactorMin { get; set; } = 0;
     public float RandomHorizontalFactorMax { get; set; } = 0;
     public float RandomVerticalFactorMin { get; set; } = 0;
     public float RandomVerticalFactorMax { get; set; } = 0;
     public float RandomRotationFactorMin { get; set; } = 0;
     public float RandomRotationFactorMax { get; set; } = 0;
    
     public GameObject Projectile { get; set; }
     public AudioClip AttackSound { get; set; } = null;
}
