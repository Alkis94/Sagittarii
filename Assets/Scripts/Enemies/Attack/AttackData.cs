using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[CreateAssetMenu(fileName = "AttackData", menuName = "AttackData", order = 1)]
public class AttackData : SerializedScriptableObject
{
    [SerializeField]
    private string enemyName;
    [SerializeField]
    private int attackID;

    //Attack Type
    [OdinSerialize] public ProjectileMovementTypeEnum ProjectileMovementType { get; private set; } = ProjectileMovementTypeEnum.straight;
    [ShowIf("@ ProjectileMovementType == ProjectileMovementTypeEnum.function")]
    [OdinSerialize] public FunctionMovementTypeEnum FunctionMovementType { get; private set; } = FunctionMovementTypeEnum.sin;
    [OdinSerialize] public AttackTypeEnum AttackType { get; private set; } = AttackTypeEnum.perimetrical;
    [ShowIf("@ AttackType == AttackTypeEnum.perimetrical")]
    [OdinSerialize] public bool AttackIsDirectionDependant { get; private set; } = false;
    
    //Attack Stats
    [OdinSerialize] public int ProjectileAmount { get; private set; } = 1;
    [OdinSerialize] public int ConsecutiveAttacks { get; private set; } = 1;
    [ShowIf("@ ConsecutiveAttacks > 1")]
    [OdinSerialize] public float ConsecutiveAttackDelay { get; private set; } = 0;
    [OdinSerialize] public float AttackFrequency { get; private set; } = 7;
    [OdinSerialize] public int Damage { get; private set; } = 10;
    [OdinSerialize] public float ProjectileSpeed { get; private set; } = 5;
    [OdinSerialize] public float ProjectileDestroyDelay { get; private set; } = 10f;

    //Placement
    [OdinSerialize] public List<float> ProjectileRotations { get; private set; }
    [OdinSerialize] public List<Vector3> ProjectileSpawnPositionOffset { get; private set; }
    [OdinSerialize] public Vector3 UniversalSpawnPositionOffset { get; private set; } 

    //randomness
    [OdinSerialize] public float RandomHorizontalFactorMin { get; private set; } = 0;
    [OdinSerialize] public float RandomHorizontalFactorMax { get; private set; } = 0;
    [OdinSerialize] public float RandomVerticalFactorMin { get; private set; } = 0;
    [OdinSerialize] public float RandomVerticalFactorMax { get; private set; } = 0;
    [OdinSerialize] public float RandomRotationFactorMin { get; private set; } = 0;
    [OdinSerialize] public float RandomRotationFactorMax { get; private set; } = 0;

    //Parts
    [OdinSerialize] public GameObject Projectile { get; private set; }
    [OdinSerialize] public AudioClip AttackSound { get; private set; } = null;

    private void OnEnable()
    {
        LoadFromJson();

        if (ProjectileSpawnPositionOffset == null)
        {
            ProjectileSpawnPositionOffset = new List<Vector3>();

            for (int i = 0; i < ProjectileAmount; i++)
            {
                ProjectileSpawnPositionOffset.Add(Vector3.zero);
            }
        }
        else if (ProjectileSpawnPositionOffset.Count < ProjectileAmount)
        {
            int limit = ProjectileAmount - ProjectileSpawnPositionOffset.Count;
            for (int i = 0; i < limit; i++)
            {
               ProjectileSpawnPositionOffset.Add(Vector3.zero);
            }
        }

        if (ProjectileRotations == null)
        {
            ProjectileRotations = new List<float>();

            for (int i = 0; i < ProjectileAmount; i++)
            {
                ProjectileRotations.Add(0);
            }
        }
        else if (ProjectileRotations.Count == 0)
        {
            for (int i = 0; i < ProjectileAmount; i++)
            {
                ProjectileRotations.Add(0);
            }
        }
        else if (ProjectileRotations.Count < ProjectileAmount)
        {
            int limit = ProjectileAmount - ProjectileRotations.Count;
            for (int i = 0; i < limit; i++)
            {
                ProjectileRotations.Add(ProjectileRotations[0]);
            }
        }

       
    }

    private void LoadFromJson()
    {
        AttackDataInfo attackDataInfo = new AttackDataInfo();
        var fileContent = File.ReadAllText(Application.streamingAssetsPath + "/" + enemyName + "/" + enemyName + "AttackData" + attackID + ".json");
        attackDataInfo = JsonConvert.DeserializeObject<AttackDataInfo>(fileContent);

        ProjectileMovementType = attackDataInfo.ProjectileMovementType;
        FunctionMovementType = attackDataInfo.FunctionMovementType;
        AttackType = attackDataInfo.AttackType;
        AttackIsDirectionDependant = attackDataInfo.AttackIsDirectionDependant;
        ProjectileAmount = attackDataInfo.ProjectileAmount;
        ConsecutiveAttacks = attackDataInfo.ConsecutiveAttacks;
        ConsecutiveAttackDelay = attackDataInfo.ConsecutiveAttackDelay;
        AttackFrequency = attackDataInfo.attackFrequency;
        Damage = attackDataInfo.Damage;
        ProjectileSpeed = attackDataInfo.ProjectileSpeed;
        ProjectileDestroyDelay = attackDataInfo.ProjectileDestroyDelay;

        ProjectileSpawnPositionOffset.Clear();
        ProjectileRotations.Clear();

        ProjectileRotations = attackDataInfo.ProjectileRotations;
        ProjectileSpawnPositionOffset = attackDataInfo.ProjectileSpawnPositionOffset;

        //for(int i = 0; i < attackDataInfo.ProjectileRotations.Count; i++)
        //{
        //    ProjectileRotations.Add(attackDataInfo.ProjectileRotations[i]);
        //}

        //for (int i = 0; i < attackDataInfo.ProjectileSpawnPositionOffset.Count; i++)
        //{
        //    ProjectileSpawnPositionOffset.Add(attackDataInfo.ProjectileSpawnPositionOffset[i]);
        //}

        RandomHorizontalFactorMin = attackDataInfo.RandomHorizontalFactorMin;
        RandomHorizontalFactorMax = attackDataInfo.RandomHorizontalFactorMax;
        RandomVerticalFactorMin = attackDataInfo.RandomVerticalFactorMin;
        RandomVerticalFactorMax = attackDataInfo.RandomVerticalFactorMax;
        RandomRotationFactorMin = attackDataInfo.RandomRotationFactorMin;
        RandomRotationFactorMax = attackDataInfo.RandomRotationFactorMax;
    }
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
    public bool Randomness;
    public float RandomHorizontalFactorMin;
    public float RandomHorizontalFactorMax;
    public float RandomVerticalFactorMin;
    public float RandomVerticalFactorMax;
    public float RandomRotationFactorMin;
    public float RandomRotationFactorMax;
}