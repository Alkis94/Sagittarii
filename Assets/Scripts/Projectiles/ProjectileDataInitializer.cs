using UnityEngine;

public class ProjectileDataInitializer : MonoBehaviour
{
    public float Speed { private set; get; }
    public int Damage { private set; get; }
    public FunctionMovementTypeEnum FunctionMovementType { get; private set; }

    public void Initialize(ProjectileSpawnInfo spawnInfo, int layer, string tag)
    {
        AddMovementComponent(spawnInfo.movementTypeEnum);
        transform.SetPositionAndRotation(spawnInfo.spawnPosition + spawnInfo.spawnPositionOffset, Quaternion.Euler(0f, 0f, spawnInfo.rotation));
        Speed = spawnInfo.speed;
        if (Speed < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        Damage = spawnInfo.damage;
        FunctionMovementType = spawnInfo.functionMovementType;
        gameObject.layer = layer;
        gameObject.tag = tag;
        Destroy(gameObject, spawnInfo.destroyDelay);
    }

    private void AddMovementComponent(ProjectileMovementTypeEnum movementTypeEnum)
    {
        switch(movementTypeEnum)
        {
            case ProjectileMovementTypeEnum.straight:
                gameObject.AddComponent<ProjectileStraightMovement>();
                break;
            case ProjectileMovementTypeEnum.arrow:
                gameObject.AddComponent<ProjectileArrowMovement>();
                break;
            case ProjectileMovementTypeEnum.sin:
                gameObject.AddComponent<ProjectileSinMovement>();
                break;
            case ProjectileMovementTypeEnum.circle:
                gameObject.AddComponent<ProjectileCircleMovement>();
                break;
            case ProjectileMovementTypeEnum.spiral:
                gameObject.AddComponent<ProjectileSpiralMovement>();
                break;
            case ProjectileMovementTypeEnum.function:
                gameObject.AddComponent<ProjectileFunctionMovement>();
                break;
            case ProjectileMovementTypeEnum.none:
                break;
            default:
                Debug.LogError("Error projectile movement type not found!");
                break;
        }
    }
}

public struct ProjectileSpawnInfo
{
    public Vector3 spawnPosition;
    public GameObject projectile;
    public Vector3 spawnPositionOffset;
    public float speed;
    public float destroyDelay;
    public int damage;
    public float rotation;
    public ProjectileMovementTypeEnum movementTypeEnum;
    public FunctionMovementTypeEnum functionMovementType;
}
