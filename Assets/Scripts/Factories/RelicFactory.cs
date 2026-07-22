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

        if(ES3.FileExists(ProfileManager.Instance.GetProfileRunPath() + "/UniqueItems"))
        {
            foreach(var relic in PlayerHasUniqueRelic)
            {
                if (ES3.KeyExists(relic.Key, ProfileManager.Instance.GetProfileRunPath() + "/UniqueItems"))
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

    private static readonly Dictionary<string, bool> PlayerHasUniqueRelic = new()
    {
          {"Trident", false},
          {"BearJaw", false},
          {"GreenFlame", false}
    };

    public static void PlayerGotUniqueRelic (string relicName)
    {
        PlayerHasUniqueRelic[relicName] = true;
        ES3.Save(relicName, true, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.UniqueItems);
    }

    public static bool CheckUniqueRelicPossession (string relicName)
    {
        return PlayerHasUniqueRelic[relicName];
    }

    private void CreateItem(string relic, Vector3 deadEnemyPosition)
    {
        if (PlayerHasUniqueRelic.ContainsKey(relic) && !PlayerHasUniqueRelic[relic])
        {
            Instantiate(relicsDictionery[relic], deadEnemyPosition, Quaternion.identity);
        }
    }
}

