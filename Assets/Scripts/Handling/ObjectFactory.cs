using UnityEngine;

public class ObjectFactory : MonoBehaviour
{
    //   # # # # # # # # # # # # 
    //   #                     #
    //   #  SINGLETON CLASS    #
    //   #                     #
    //   # # # # # # # # # # # # 


    public static ObjectFactory Instance = null; // Needed

    //   # # # # # # # # # # # # 
    //   #                     #
    //   #       ENEMIES       #
    //   #                     #
    //   # # # # # # # # # # # # 

    public GameObject BatPrefab;
    public GameObject CrowPrefab;
    public GameObject MedusaPrefab;
    public GameObject ImpPrefab;
    public GameObject WolfPrefab;

    //   # # # # # # # # # # # # 
    //   #                     #
    //   #   Projectiles       #
    //   #                     #
    //   # # # # # # # # # # # # 


    public GameObject ArrowPrefab;


    //   # # # # # # # # # # # # 
    //   #                     #
    //   #       Pickups       #
    //   #                     #
    //   # # # # # # # # # # # # 

    public GameObject HealthPickupPrefab;
    public GameObject MaximumHealthPickupPrefab;
    public GameObject BatWingsPickupPrefab;
    public GameObject WolfPawPickupPrefab;
    public GameObject ImpFlamePickupPrefab;
    public GameObject DeadBirdPickupPrefab;

    //   # # # # # # # # # # # # 
    //   #                     #
    //   #       Other         #
    //   #                     #
    //   # # # # # # # # # # # #

    public ParticleSystem DeathBloodSplatPrefab;
    public ParticleSystem PickupParticlesPrefab;




    void Awake()
    {
        // First we check if there are any other instances conflicting
        if (Instance != null && Instance != this)
        {
            // If that is the case, we destroy other instances
            Destroy(gameObject);
        }

        // Here we save our singleton instance
        Instance = this;
    }

    public void CreateFlyingEnemy(GameObject flyingEnemy,Vector3 screenPosition)
    {
        Instantiate(flyingEnemy, screenPosition, Quaternion.identity);
    }

    public T CreateGroundEnemy<T>(Transform spawnPointPosition, int horizontalFactor, GameObject groundEnemyPrefab) where T : IInitializable
    {
        var groundEnemy = Instantiate(groundEnemyPrefab).GetComponent<T>();
        groundEnemy.Initialize(spawnPointPosition, horizontalFactor);
        return groundEnemy;
    }


    public void CreateDeathBloodSplat(Transform enemyPosition)
    {
        Instantiate(Instance.DeathBloodSplatPrefab, enemyPosition.transform.position,Quaternion.identity);
    }

    public Object CreatePickupParticles(Transform enemyPosition)
    {
        var pickupParticles = Instantiate(Instance.PickupParticlesPrefab, enemyPosition.transform.position, Instance.PickupParticlesPrefab.transform.rotation);
        return pickupParticles;
    }

    public Arrow CreateArrow (float arrowPower, GameObject arrowEmitter, float verticalFactor)
    {
        var arrow = Instantiate(Instance.ArrowPrefab).GetComponent<Arrow>();
        arrow.Initialize(arrowPower,arrowEmitter, verticalFactor);
        return arrow;
    }

    public T CreateProjectile<T>(Transform projectilePosition, Vector2 direction,GameObject projectile) where T : IInitializableProjectile
    {
        var someProjectile = Instantiate(projectile).GetComponent<T>();
        someProjectile.Initialize(projectilePosition, direction.x, direction.y);
        return someProjectile;
    }

    public ProjectileNew CreateProjectileNew(Transform projectilePosition, Vector2 direction, GameObject projectile,Vector3 spawnPositionOffset,float projectileSpeed,float projectileDestroyDelay,float projectileRotation) 
    {
        var someProjectile = Instantiate(projectile).GetComponent<ProjectileNew>();
        someProjectile.Initialize(projectilePosition, direction, spawnPositionOffset,projectileSpeed,projectileDestroyDelay,projectileRotation);
        return someProjectile;
    }

    public Pickup CreatePickup(Transform deadEnemyPosition, GameObject pickup)
    {
        var somePickup = Instantiate(pickup).GetComponent<Pickup>();
        somePickup.Initialize(deadEnemyPosition);
        return somePickup;
    }


}
