using UnityEngine;
using System.Collections.Generic;

public class RelicFactory : MonoBehaviour
{

    private static RelicFactory instance = null;

    [SerializeField]
    private List<GameObject> relicsList;
    private Dictionary<string, GameObject> relicsDictionery;

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
        EnemyDeath.OnDeathDropRelic += CreateItem;
    }

    private void OnDisable()
    {
        EnemyDeath.OnDeathDropRelic -= CreateItem;
    }

    private void Start()
    {
        relicsDictionery = new Dictionary<string, GameObject>();

        for (int i = 0; i < relicsList.Count; i++)
        {
            relicsDictionery.Add(relicsList[i].name, relicsList[i]);
        }
    }

    public static Dictionary<string, bool> PlayerHasUniqueRelic = new Dictionary<string, bool>()
    {
          {"Trident",false},
          {"BearJaw",false},
          {"GreenFlame",false}
    };


    private void CreateItem(string relic, Vector3 deadEnemyPosition)
    {

        bool ItemShouldDrop = true;

        if (PlayerHasUniqueRelic.ContainsKey(relic))
        {
            if(PlayerHasUniqueRelic[relic])
            {
                ItemShouldDrop = false;
            }
        }

        if(ItemShouldDrop)
        {
            Instantiate(relicsDictionery[relic], deadEnemyPosition, Quaternion.identity);
        }
        
    }
}

