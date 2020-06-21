using UnityEngine;
using System.Collections.Generic;

public class PickUpFactory : MonoBehaviour
{

    private static PickUpFactory instance = null;

    [SerializeField]
    private List<GameObject> pickupsList;
    private Dictionary<string, GameObject> pickupsDictionery;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
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

    public void CreatePickup(Vector3 deadEnemyPosition, string pickup)
    {
        Instantiate(pickupsDictionery[pickup],deadEnemyPosition,Quaternion.identity);
    }
}
