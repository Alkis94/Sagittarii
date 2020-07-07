using UnityEngine;
using System.Collections;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[CreateAssetMenu(fileName = "PlayerAttackData", menuName = "PlayerAttackData", order = 1)]
public class PlayerAttackData : AttackData
{

    private void OnEnable()
    {
        LoadFromJson();
    }

    private  void LoadFromJson()
    {
        AttackDataInfo attackDataInfo = new AttackDataInfo();

        string fileContent;
        string attackerName = name.Substring(0, name.Length - 11);
        if (File.Exists(Application.streamingAssetsPath + "/Player/" + attackerName + "/" + name + ".json"))
        {
            fileContent = File.ReadAllText(Application.streamingAssetsPath + "/Player/" + attackerName + "/" + name + ".json");
        }
        else
        {
            attackerName = name.Substring(0, name.Length - 12);
            fileContent = File.ReadAllText(Application.streamingAssetsPath + "/Player/" + attackerName + "/" + name + ".json");
        }

        attackDataInfo = JsonConvert.DeserializeObject<AttackDataInfo>(fileContent);

        ProjectileMovementType = attackDataInfo.ProjectileMovementType;
        FunctionMovementType = attackDataInfo.FunctionMovementType;
        AttackType = attackDataInfo.AttackType;
        AttackIsDirectionDependant = attackDataInfo.AttackIsDirectionDependant;

        ConsecutiveAttacks = attackDataInfo.ConsecutiveAttacks;
        ConsecutiveAttackDelay = attackDataInfo.ConsecutiveAttackDelay;
        UniversalSpawnPositionOffset = attackDataInfo.UniversalSpawnPositionOffset;

        ProjectileSpawnPositionOffset.Clear();
        ProjectileRotations.Clear();

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
