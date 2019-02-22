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
    public GameObject DamagePickupPrefab;
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



    public Item CreatePickup(Transform deadEnemyPosition, GameObject pickup)
    {
        var somePickup = Instantiate(pickup).GetComponent<Item>();
        somePickup.Initialize(deadEnemyPosition);
        return somePickup;
    }


}
