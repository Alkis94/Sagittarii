using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class AttackData : SerializedScriptableObject
{
    //Attack Type
    [OdinSerialize] public ProjectileMovementTypeEnum ProjectileMovementType { get; protected set; } = ProjectileMovementTypeEnum.straight;
    [ShowIf("@ ProjectileMovementType == ProjectileMovementTypeEnum.function")]
    [OdinSerialize] public FunctionMovementTypeEnum FunctionMovementType { get; protected set; } = FunctionMovementTypeEnum.sin;
    [OdinSerialize] public AttackTypeEnum AttackType { get; protected set; } = AttackTypeEnum.perimetrical;
    [ShowIf("@ AttackType == AttackTypeEnum.perimetrical")]
    [OdinSerialize] public bool AttackIsDirectionDependant { get; protected set; } = false;
    
    //Attack Stats
    [OdinSerialize] public int ConsecutiveAttacks { get; protected set; } = 1;
    [ShowIf("@ ConsecutiveAttacks > 1")]
    [OdinSerialize] public float ConsecutiveAttackDelay { get; protected set; } = 0;

    //Placement
    [OdinSerialize] public List<float> ProjectileRotations { get; protected set; }
    [OdinSerialize] public List<Vector3> ProjectileSpawnPositionOffset { get; protected set; }
    [OdinSerialize] public Vector3 UniversalSpawnPositionOffset { get; protected set; } 

    //randomness
    [OdinSerialize] public float RandomHorizontalFactorMin { get; protected set; } = 0;
    [OdinSerialize] public float RandomHorizontalFactorMax { get; protected set; } = 0;
    [OdinSerialize] public float RandomVerticalFactorMin { get; protected set; } = 0;
    [OdinSerialize] public float RandomVerticalFactorMax { get; protected set; } = 0;
    [OdinSerialize] public float RandomRotationFactorMin { get; protected set; } = 0;
    [OdinSerialize] public float RandomRotationFactorMax { get; protected set; } = 0;

    //Parts
    [OdinSerialize] public GameObject Projectile { get; protected set; }
    [OdinSerialize] public AudioClip AttackSound { get; protected set; } = null;

   

    
}

public struct AttackDataInfo
{
    [JsonConverter(typeof(StringEnumConverter))]
    public ProjectileMovementTypeEnum ProjectileMovementType;
    [JsonConverter(typeof(StringEnumConverter))]
    public FunctionMovementTypeEnum FunctionMovementType;
    [JsonConverter(typeof(StringEnumConverter))]
    public AttackTypeEnum AttackType;
    public bool AttackIsDirectionDependant;


    //Attack Stats
    public int ProjectileAmount;
    public int ConsecutiveAttacks;
    public float ConsecutiveAttackDelay;
    public float attackFrequency;
    public int Damage;
    public float ProjectileSpeed;
    public float ProjectileDestroyDelay;

    //Placement
    public List<float> ProjectileRotations;
    public List<Vector3> ProjectileSpawnPositionOffset;
    public Vector3 UniversalSpawnPositionOffset;


    //randomness
    public float RandomHorizontalFactorMin;
    public float RandomHorizontalFactorMax;
    public float RandomVerticalFactorMin;
    public float RandomVerticalFactorMax;
    public float RandomRotationFactorMin;
    public float RandomRotationFactorMax;
}