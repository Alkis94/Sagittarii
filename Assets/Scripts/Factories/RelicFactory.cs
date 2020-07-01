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

        if(ES3.FileExists("Saves/Profile" + SaveProfile.SaveID + "/UniqueItems"))
        {
            foreach(var relic in PlayerHasUniqueRelic)
            {
                if (ES3.KeyExists(relic.Key, "Saves/Profile" + SaveProfile.SaveID + "/UniqueItems"))
                {
                    PlayerHasUniqueRelic[relic.Key] = true;
                }
            }
            
        }
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

    private static Dictionary<string, bool> PlayerHasUniqueRelic = new Dictionary<string, bool>()
    {
          {"Trident",false},
          {"BearJaw",false},
          {"GreenFlame",false}
    };

    public static void PlayerGotUniqueRelic (string relicName)
    {
        PlayerHasUniqueRelic[relicName] = true;
        ES3.Save<bool>(relicName, true, "Saves/Profile" + SaveProfile.SaveID + "/UniqueItems");
    }

    public static bool CheckUniqueRelicPossession (string relicName)
    {
        return PlayerHasUniqueRelic[relicName];
    }


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

