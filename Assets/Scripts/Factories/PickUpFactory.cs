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


    private static PickUpFactory instance = null;

    [SerializeField]
    private List<GameObject> pickupsList;
    private Dictionary<string, GameObject> pickupsDictionery;

    public ParticleSystem DeathBloodSplatPrefab;

    void Awake()
    {
        // First we check if there are any other instances conflicting
        if (instance != null && instance != this)
        {
            // If that is the case, we destroy other instances
            Destroy(gameObject);
        }

        // Here we save our singleton instance
        instance = this;
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
        pickupsDictionery = new Dictionary<string, GameObject>();

        for (int i=0; i < pickupsList.Count; i++)
        {
            pickupsDictionery.Add(pickupsList[i].name, pickupsList[i]);
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
        Instantiate(pickupsDictionery[pickup],deadEnemyPosition,Quaternion.identity);
    }
}
