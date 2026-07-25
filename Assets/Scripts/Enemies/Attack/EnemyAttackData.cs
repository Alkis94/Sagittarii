using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

[CreateAssetMenu(fileName = "EnemyAttackData", menuName = "EnemyAttackData", order = 1)]
public class EnemyAttackData : AttackData
{
    [OdinSerialize] public int ProjectileAmount { get; private set; } = 1;
    [OdinSerialize] public float AttackFrequency { get; private set; } = 7;
    [OdinSerialize] public float ProjectileDestroyDelay { get; private set; } = 10f;
    [OdinSerialize] public float ProjectileSpeed { get; private set; } = 8;
    [OdinSerialize] public int Damage { get; private set; } = 10;

    private void OnEnable()
    {
        LoadFromJson();

        if (ProjectileSpawnPositionOffset == null)
        {
            ProjectileSpawnPositionOffset = new List<Vector3>();

            for (var i = 0; i < ProjectileAmount; i++)
            {
                ProjectileSpawnPositionOffset.Add(Vector3.zero);
            }
        }
        else if (ProjectileSpawnPositionOffset.Count < ProjectileAmount)
        {
            var limit = ProjectileAmount - ProjectileSpawnPositionOffset.Count;
            for (var i = 0; i < limit; i++)
            {
                ProjectileSpawnPositionOffset.Add(Vector3.zero);
            }
        }

        if (ProjectileRotations == null)
        {
            ProjectileRotations = new List<float>();

            for (var i = 0; i < ProjectileAmount; i++)
            {
                ProjectileRotations.Add(0);
            }
        }
        else if (ProjectileRotations.Count == 0)
        {
            for (var i = 0; i < ProjectileAmount; i++)
            {
                ProjectileRotations.Add(0);
            }
        }
        else if (ProjectileRotations.Count < ProjectileAmount)
        {
            var limit = ProjectileAmount - ProjectileRotations.Count;
            for (var i = 0; i < limit; i++)
            {
                ProjectileRotations.Add(ProjectileRotations[0]);
            }
        }
    }

    private  void LoadFromJson()
    {
        var attackerName = name.Substring(0, name.Length - 11);
        string fileContent;

        if (File.Exists(Application.streamingAssetsPath + "/Enemy/" + attackerName + "/" + name + ".json"))
        {
            fileContent = File.ReadAllText(Application.streamingAssetsPath + "/Enemy/" + attackerName + "/" + name + ".json");
        }
        else
        {
            attackerName = name.Substring(0, name.Length - 12);
            fileContent = File.ReadAllText(Application.streamingAssetsPath + "/Enemy/" + attackerName + "/" + name + ".json");
        }

        var attackDataInfo = JsonConvert.DeserializeObject<AttackDataInfo>(fileContent);

        ProjectileMovementType = attackDataInfo.ProjectileMovementType;
        FunctionMovementType = attackDataInfo.FunctionMovementType;
        AttackType = attackDataInfo.AttackType;
        AttackIsDirectionDependant = attackDataInfo.AttackIsDirectionDependant;
        AttackHasExternalSpawner = attackDataInfo.AttackHasExternalSpawner;

        ProjectileAmount = attackDataInfo.ProjectileAmount;
        AttackFrequency = attackDataInfo.AttackFrequency;
        Damage = attackDataInfo.Damage;
        ProjectileSpeed = attackDataInfo.ProjectileSpeed;
        ProjectileDestroyDelay = attackDataInfo.ProjectileDestroyDelay;
        ConsecutiveAttacks = attackDataInfo.ConsecutiveAttacks;
        ConsecutiveAttackDelay = attackDataInfo.ConsecutiveAttackDelay;
        UniversalSpawnPositionOffset = attackDataInfo.UniversalSpawnPositionOffset;

        ProjectileSpawnPositionOffset?.Clear();
        ProjectileRotations?.Clear();
        
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