using UnityEngine;
using System.Collections.Generic;

public class RelicFactory : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> relicsList;
    private Dictionary<string, GameObject> relicsDictionery;

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

    public static Dictionary<string, bool> PlayerHasRelic = new Dictionary<string, bool>()
    {
            {"Trident",false},
            {"BatWings",false}
    };


    private  void CreateItem(string relic, Vector3 deadEnemyPosition)
    {
         Instantiate(relicsDictionery[relic], deadEnemyPosition, Quaternion.identity);
    }
}

