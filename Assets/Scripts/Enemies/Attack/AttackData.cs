using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;


[CreateAssetMenu(fileName = "AttackData", menuName = "AttackData", order = 1)]
public class AttackData : SerializedScriptableObject
{

    //Attack Type
    [OdinSerialize] public ProjectileMovementTypeEnum projectileMovementType { get; private set; } = ProjectileMovementTypeEnum.straight;
    [OdinSerialize] public AttackTypeEnum AttackType { get; private set; } = AttackTypeEnum.perimetrical;
    [ShowIf("@ AttackType == AttackTypeEnum.perimetrical")]
    [OdinSerialize] public bool AttackIsDirectionDependant { get; private set; } = false;
    
    
    //Attack Stats
    [OdinSerialize] public int ProjectileAmount { get; private set; } = 1;
    [OdinSerialize] public int ConsecutiveAttacks { get; private set; } = 1;
    [ShowIf("@ ConsecutiveAttacks > 1")]
    [OdinSerialize] public float ConsecutiveAttackDelay { get; private set; } = 0;
    [OdinSerialize] public int Damage { get; private set; } = 10;
    [OdinSerialize] public float ProjectileSpeed { get; private set; } = 5;
    [OdinSerialize] public float ProjectileDestroyDelay { get; private set; } = 10f;

    //Placement
    [OdinSerialize] public List<float> ProjectileRotations { get; private set; }
    [OdinSerialize] public List<Vector3> ProjectileSpawnPositionOffset { get; private set; }
    [OdinSerialize] public Vector3 UniversalSpawnPositionOffset { get; private set; } 

    //Parts
    [OdinSerialize] public GameObject Projectile { get; private set; }
    [OdinSerialize] public AudioClip AttackSound { get; private set; } = null;

    //randomness
    [OdinSerialize] public bool Randomness { get; private set; } = false;
    [ShowIf("@ Randomness")]
    [OdinSerialize] public float RandomHorizontalFactorMin { get; private set; } = 0;
    [ShowIf("@ Randomness")]
    [OdinSerialize] public float RandomHorizontalFactorMax { get; private set; } = 0;
    [ShowIf("@ Randomness")]
    [OdinSerialize] public float RandomVerticalFactorMin { get; private set; } = 0;
    [ShowIf("@ Randomness")]
    [OdinSerialize] public float RandomVerticalFactorMax { get; private set; } = 0;
    [ShowIf("@ Randomness")]
    [OdinSerialize] public float RandomRotationFactorMin { get; private set; } = 0;
    [ShowIf("@ Randomness")]
    [OdinSerialize] public float RandomRotationFactorMax { get; private set; } = 0;

    private void OnEnable()
    {
        if(ProjectileSpawnPositionOffset == null)
        {
            ProjectileSpawnPositionOffset = new List<Vector3>();

            for (int i = 0; i < ProjectileAmount; i++)
            {
                ProjectileSpawnPositionOffset.Add(Vector3.zero);
            }
        }
        else if (ProjectileSpawnPositionOffset.Count == 0)
        {
            for (int i = 0; i < ProjectileAmount; i++)
            {
               ProjectileSpawnPositionOffset.Add(Vector3.zero);
            }
        }
        else if (ProjectileSpawnPositionOffset.Count == 1)
        {
            for (int i = 0; i < ProjectileAmount - 1; i++)
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
        else if (ProjectileRotations.Count == 1)
        {
            for (int i = 0; i < ProjectileAmount - 1; i++)
            {
                ProjectileRotations.Add(ProjectileRotations[0]);
            }
        }
    }
}
