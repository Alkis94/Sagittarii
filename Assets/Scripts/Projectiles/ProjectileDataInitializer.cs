using UnityEngine;

public class ProjectileDataInitializer : MonoBehaviour
{

    public float DestroyDelay { private set; get; }
    public float Speed { private set; get; }
    public int Damage { private set; get; }
    public FunctionMovementTypeEnum FunctionMovementType { get; private set; }

    public void Initialize(Vector3 parentPosition, Vector3 spawnPositionOffset, float projectileSpeed, float projectileDestroyDelay, int damage, float projectileRotation, ProjectileMovementTypeEnum movementTypeEnum, int layer, string tag)
    {
        AddMovementComponent(movementTypeEnum);
        transform.position = parentPosition + spawnPositionOffset;
        transform.rotation = Quaternion.Euler(0f, 0f, projectileRotation);
        Speed = projectileSpeed;
        if(Speed < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        DestroyDelay = projectileDestroyDelay;
        Damage = damage;
        gameObject.layer = layer;
        gameObject.tag = tag;
    }

    public void Initialize(Vector3 parentPosition, Vector3 spawnPositionOffset, float projectileSpeed, float projectileDestroyDelay, int damage, Quaternion projectileRotation, ProjectileMovementTypeEnum movementTypeEnum, int layer, string tag)
    {
        AddMovementComponent(movementTypeEnum);
        transform.position = parentPosition + spawnPositionOffset;
        transform.rotation = projectileRotation;
        Speed = projectileSpeed;
        if (Speed < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        DestroyDelay = projectileDestroyDelay;
        Damage = damage;
        gameObject.layer = layer;
        gameObject.tag = tag;
    }

    public void Initialize(Vector3 parentPosition, Vector3 spawnPositionOffset, float projectileSpeed, float projectileDestroyDelay, int damage, float projectileRotation, ProjectileMovementTypeEnum movementTypeEnum, FunctionMovementTypeEnum functionMovementType, int layer, string tag)
    {
        AddMovementComponent(movementTypeEnum);
        transform.position = parentPosition + spawnPositionOffset;
        transform.rotation = Quaternion.Euler(0f, 0f, projectileRotation);
        Speed = projectileSpeed;
        if (Speed < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        DestroyDelay = projectileDestroyDelay;
        Damage = damage;
        FunctionMovementType = functionMovementType;
        gameObject.layer = layer;
        gameObject.tag = tag;
    }

    private void AddMovementComponent(ProjectileMovementTypeEnum movementTypeEnum)
    {
        switch(movementTypeEnum)
        {
            case ProjectileMovementTypeEnum.straight:
                ProjectileStraightMovement projectileStraightMovement = gameObject.AddComponent<ProjectileStraightMovement>() as ProjectileStraightMovement;
                break;
            case ProjectileMovementTypeEnum.arrow:
                ProjectileArrowMovement projectileArrowMovement = gameObject.AddComponent<ProjectileArrowMovement>() as ProjectileArrowMovement;
                break;
            case ProjectileMovementTypeEnum.sin:
                ProjectileSinMovement projectileSinMovement = gameObject.AddComponent<ProjectileSinMovement>() as ProjectileSinMovement;
                break;
            case ProjectileMovementTypeEnum.circle:
                ProjectileCircleMovement projectileCircleMovement = gameObject.AddComponent<ProjectileCircleMovement>() as ProjectileCircleMovement;
                break;
            case ProjectileMovementTypeEnum.spiral:
                ProjectileSpiralMovement projectileSpiralMovement = gameObject.AddComponent<ProjectileSpiralMovement>() as ProjectileSpiralMovement;
                break;
            case ProjectileMovementTypeEnum.function:
                ProjectileFunctionMovement projectileFunctionMovement = gameObject.AddComponent<ProjectileFunctionMovement>() as ProjectileFunctionMovement;
                break;
            default:
                Debug.LogError("Error projectile movement type not found!");
                break;
        }
    }

}
