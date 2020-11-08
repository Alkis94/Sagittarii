using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

[CreateAssetMenu(fileName = "EnemyData", menuName = "EnemyData", order = 2)]
public class EnemyData : SerializedScriptableObject
{

    [OdinSerialize] public string EnemyName { get; private set; }

    [Title("Enemy Stats")]
    [OdinSerialize] public int Health { get; private set; } = 10;
    [OdinSerialize] public float Speed { get; private set; } = 2;
    [OdinSerialize] public float DelayBeforeFirstAttack { get; private set; } = 3;


    [Title("Bools")]
    [OdinSerialize] public bool Damageable { get; set; } = true;
    [OdinSerialize] public bool Amputation { get; private set; } = false;
    [OdinSerialize] public bool HasBlood { get; private set; } = true;
    [OdinSerialize] public bool HasCriticalDeath { get; private set; } = false;
    [OdinSerialize] public bool ShakeBeforeDeath { get; private set; } = false;
    [OdinSerialize] public bool ChangingDirections { get; private set; } = false;
    [ShowIf("@ ChangingDirections")]
    [OdinSerialize] public float ChangeDirectionFrequency { get; private set; } = 0;

    [Title("Drops")]
    [OdinSerialize] public int MinGoldGiven { get; private set; } = 5;
    [OdinSerialize] public int MaxGoldGiven { get; private set; } = 15;
    [OdinSerialize] public float GoldDropChance { get; private set; } = 0.1f;
    [OdinSerialize] public List<string> Relics { get; private set; }
    [OdinSerialize] public List<float> RelicDropChance { get; private set; }

    private void OnEnable()
    {
        LoadFromJSON();
    }

    private void LoadFromJSON()
    {
        EnemyInfo enemyInfo;
        enemyInfo = new EnemyInfo();
        var fileContent = File.ReadAllText(Application.streamingAssetsPath + "/Enemy/" + EnemyName + "/" + EnemyName + ".json");
        enemyInfo = JsonConvert.DeserializeObject<EnemyInfo>(fileContent);

        Health = enemyInfo.health;
        Speed = enemyInfo.speed;
        DelayBeforeFirstAttack = enemyInfo.delayBeforeFirstAttack;
        Damageable = enemyInfo.damageable;
        Amputation = enemyInfo.amputation;
        HasBlood = enemyInfo.hasBlood;
        HasCriticalDeath = enemyInfo.hasCriticalDeath;
        ShakeBeforeDeath = enemyInfo.shakeBeforeDeath;
        ChangingDirections = enemyInfo.changingDirections;
        ChangeDirectionFrequency = enemyInfo.changeDirectionFrequency;
        MinGoldGiven = enemyInfo.minGoldGiven;
        MaxGoldGiven = enemyInfo.maxGoldGiven;
        GoldDropChance = enemyInfo.goldDropChance;
        Relics = enemyInfo.relics;
        RelicDropChance = enemyInfo.relicDropChance;

    }
}

public struct EnemyInfo
{
    [Title("Enemy Stats")]
    public int health;
    public float speed;
    public float delayBeforeFirstAttack;


    [Title("Bools")]
    public bool damageable;
    public bool amputation;
    public bool hasBlood;
    public bool hasCriticalDeath;
    public bool shakeBeforeDeath;
    public bool changingDirections;
    public float changeDirectionFrequency;

    [Title("Drops")]
    public int minGoldGiven;
    public int maxGoldGiven;
    public float goldDropChance;
    public List<string> relics;
    public List<float> relicDropChance;
}