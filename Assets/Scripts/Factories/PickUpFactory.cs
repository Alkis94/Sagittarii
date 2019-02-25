using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickUpFactory : MonoBehaviour
{
    //   # # # # # # # # # # # # 
    //   #                     #
    //   #  SINGLETON CLASS    #
    //   #                     #
    //   # # # # # # # # # # # # 


    private static PickUpFactory Instance = null;

    [SerializeField]
    private List<GameObject> PickupsList;


    private Dictionary<string, GameObject> PickupsDictionery;
    

    //   # # # # # # # # # # # # 
    //   #                     #
    //   #       Other         #
    //   #                     #
    //   # # # # # # # # # # # #

    public ParticleSystem DeathBloodSplatPrefab;

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

    private void OnEnable()
    {
        EnemyDeath.OnDeathDropPickup += CreatePickup;
    }

    private void OnDisable()
    {
        EnemyDeath.OnDeathDropPickup -= CreatePickup;
    }


    private void Start()
    {
        PickupsDictionery = new Dictionary<string, GameObject>();

        for (int i=0; i < PickupsList.Count; i++)
        {
            PickupsDictionery.Add(PickupsList[i].name, PickupsList[i]);
        }
    }

    //public void CreateFlyingEnemy(GameObject flyingEnemy,Vector3 screenPosition)
    //{
    //    Instantiate(flyingEnemy, screenPosition, Quaternion.identity);
    //}

    //public T CreateGroundEnemy<T>(Transform spawnPointPosition, int horizontalFactor, GameObject groundEnemyPrefab) where T : IInitializable
    //{
    //    var groundEnemy = Instantiate(groundEnemyPrefab).GetComponent<T>();
    //    groundEnemy.Initialize(spawnPointPosition, horizontalFactor);
    //    return groundEnemy;
    //}

    public void CreatePickup(Vector3 deadEnemyPosition, string pickup)
    {
        Instantiate(PickupsDictionery[pickup],deadEnemyPosition,Quaternion.identity);
    }
}
