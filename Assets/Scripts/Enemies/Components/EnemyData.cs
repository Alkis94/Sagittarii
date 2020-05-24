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
    [SerializeField]
    private string enemyName;

    [Title("Enemy Stats")]
    [OdinSerialize] public int Health { get; private set; } = 10;
    [OdinSerialize] public float Speed { get; private set; } = 2;
    [OdinSerialize] public float DelayBeforeFirstAttack { get; private set; } = 3;


    [Title("Bools")]
    [OdinSerialize] public bool Damageable { get; set; } = true;
    [OdinSerialize] public bool Amputation { get; private set; } = false;
    [OdinSerialize] public bool ChangingDirections { get; private set; } = false;
    [ShowIf("@ ChangingDirections")]
    [OdinSerialize] public float ChangeDirectionFrequency { get; private set; } = 0;

    [Title("Drops")]
    [OdinSerialize] public int MinGoldGiven { get; private set; } = 5;
    [OdinSerialize] public int MaxGoldGiven { get; private set; } = 15;
    [OdinSerialize] public float GoldDropChance { get; private set; } = 0.1f;
    [OdinSerialize] public string Relic { get; private set; }
    [OdinSerialize] public float RelicDropChance { get; private set; } = 0.01f;

    private void OnEnable()
    {
        LoadFromJSON();
    }

    private void LoadFromJSON()
    {
        EnemyInfo enemyInfo;
        enemyInfo = new EnemyInfo();
        var fileContent = File.ReadAllText(Application.streamingAssetsPath + "/" + enemyName + "/" + enemyName + ".json");
        enemyInfo = JsonConvert.DeserializeObject<EnemyInfo>(fileContent);

        Health = enemyInfo.health;
        Speed = enemyInfo.speed;
        DelayBeforeFirstAttack = enemyInfo.delayBeforeFirstAttack;
        Damageable = enemyInfo.damageable;
        Amputation = enemyInfo.amputation;
        ChangingDirections = enemyInfo.changingDirections;
        ChangeDirectionFrequency = enemyInfo.changeDirectionFrequency;
        MinGoldGiven = enemyInfo.minGoldGiven;
        MaxGoldGiven = enemyInfo.maxGoldGiven;
        GoldDropChance = enemyInfo.goldDropChance;
        Relic = enemyInfo.relic;
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
    public bool changingDirections;
    public float changeDirectionFrequency;

    [Title("Drops")]
    public int minGoldGiven;
    public int maxGoldGiven;
    public float goldDropChance;
    public string relic;
    public float relicDropChance;
}