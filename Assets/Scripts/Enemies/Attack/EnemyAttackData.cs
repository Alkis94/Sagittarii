using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[CreateAssetMenu(fileName = "EnemyAttackData", menuName = "EnemyAttackData", order = 1)]
public class EnemyAttackData : AttackData
{

    [OdinSerialize] public int ProjectileAmount { get; private set; } = 1;
    [OdinSerialize] public float AttackFrequency { get; private set; } = 7;
    [OdinSerialize] public int Damage { get; private set; } = 10;
    [OdinSerialize] public float ProjectileSpeed { get; private set; } = 5;
    [OdinSerialize] public float ProjectileDestroyDelay { get; private set; } = 10f;

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

    private  void LoadFromJson()
    {
        AttackDataInfo attackDataInfo = new AttackDataInfo();

        string fileContent;
        string attackerName = name.Substring(0, name.Length - 11);
        if (File.Exists(Application.streamingAssetsPath + "/Enemy/" + attackerName + "/" + name + ".json"))
        {
            fileContent = File.ReadAllText(Application.streamingAssetsPath + "/Enemy/" + attackerName + "/" + name + ".json");
        }
        else
        {
            attackerName = name.Substring(0, name.Length - 12);
            fileContent = File.ReadAllText(Application.streamingAssetsPath + "/Enemy/" + attackerName + "/" + name + ".json");
        }

        attackDataInfo = JsonConvert.DeserializeObject<AttackDataInfo>(fileContent);

        ProjectileMovementType = attackDataInfo.ProjectileMovementType;
        FunctionMovementType = attackDataInfo.FunctionMovementType;
        AttackType = attackDataInfo.AttackType;
        AttackIsDirectionDependant = attackDataInfo.AttackIsDirectionDependant;

        ProjectileAmount = attackDataInfo.ProjectileAmount;
        AttackFrequency = attackDataInfo.attackFrequency;
        Damage = attackDataInfo.Damage;
        ProjectileSpeed = attackDataInfo.ProjectileSpeed;
        ProjectileDestroyDelay = attackDataInfo.ProjectileDestroyDelay;
        ConsecutiveAttacks = attackDataInfo.ConsecutiveAttacks;
        ConsecutiveAttackDelay = attackDataInfo.ConsecutiveAttackDelay;
        UniversalSpawnPositionOffset = attackDataInfo.UniversalSpawnPositionOffset;

        if(ProjectileSpawnPositionOffset != null)
        {
            ProjectileSpawnPositionOffset.Clear();
        }
        
        if(ProjectileRotations != null)
        {
            ProjectileRotations.Clear();
        }
        
        ProjectileRotations = attackDataInfo.ProjectileRotations;
        ProjectileSpawnPositionOffset = attackDataInfo.ProjectileSpawnPositionOffset;

        RandomHorizontalFactorMin = attackDataInfo.RandomHorizontalFactorMin;
        RandomHorizontalFactorMax = attackDataInfo.RandomHorizontalFactorMax;
        RandomVerticalFactorMin = attackDataInfo.RandomVerticalFactorMin;
        RandomVerticalFactorMax = attackDataInfo.RandomVerticalFactorMax;
        RandomRotationFactorMin = attackDataInfo.RandomRotationFactorMin;
        RandomRotationFactorMax = attackDataInfo.RandomRotationFactorMax;
        
    }
}