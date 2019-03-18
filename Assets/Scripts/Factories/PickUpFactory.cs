using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickUpFactory : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> pickupsList;
    private Dictionary<string, GameObject> pickupsDictionery;

    //public ParticleSystem DeathBloodSplatPrefab;



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

    public void CreatePickup(Vector3 deadEnemyPosition, string pickup)
    {
        Instantiate(pickupsDictionery[pickup],deadEnemyPosition,Quaternion.identity);
    }
}
