using UnityEngine;

public class ProjectileDataInitializer : MonoBehaviour
{

    public float DestroyDelay { private set; get; }
    public float Speed { private set; get; }
    public int Damage { private set; get; }


    private void Start()
    {
        Destroy(gameObject, DestroyDelay);
    }

    public void Initialize(Vector3 parentPosition,Vector3 spawnPositionOffset,float projectileSpeed,float projectileDestroyDelay,int damage,float projectileRotation, ProjectileMovementTypeEnum movementTypeEnum)
    {
        AddMovementComponent(movementTypeEnum);
        transform.position = parentPosition + spawnPositionOffset;
        transform.rotation = Quaternion.Euler(0f, 0f, projectileRotation);
        Speed = projectileSpeed;
        DestroyDelay = projectileDestroyDelay;
        Damage = damage;
    }

    public void Initialize(Vector3 parentPosition, Vector3 spawnPositionOffset, float projectileSpeed, float projectileDestroyDelay, int damage, Quaternion projectileRotation, ProjectileMovementTypeEnum movementTypeEnum)
    {
        AddMovementComponent(movementTypeEnum);
        transform.position = parentPosition + spawnPositionOffset;
        transform.rotation = projectileRotation;
        Speed = projectileSpeed;
        DestroyDelay = projectileDestroyDelay;
        Damage = damage;
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
            case ProjectileMovementTypeEnum.function:
                ProjectileFunctionMovement projectileFunctionMovement = gameObject.AddComponent<ProjectileFunctionMovement>() as ProjectileFunctionMovement;
                break;
            default:
                Debug.LogError("Error projectile movement type not found!");
                break;
        }
    }

}
