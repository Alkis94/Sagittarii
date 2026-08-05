using UnityEngine;
using System.Collections.Generic;

public class RelicFactory : MonoBehaviour
{
    public static RelicFactory Instance = null;

    [SerializeField]
    private List<GameObject> relicsList;
    private Dictionary<string, GameObject> relicsDictionary = new();


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
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

    private void Start()
    {
        for (int i = 0; i < relicsList.Count; i++)
        {
            relicsDictionary.Add(relicsList[i].name, relicsList[i]);
        }
    }

    private static readonly Dictionary<string, bool> PlayerHasUniqueRelic = new()
    {
          {"Trident", false},
          {"BearJaw", false},
          {"GreenFlame", false}
    };

    public static void PlayerAcquiredUniqueRelic(string relicName)
    {
        PlayerHasUniqueRelic[relicName] = true;
        ES3.Save(relicName, true, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.UniqueItems);
    }

    public static bool CheckUniqueRelicPossession(string relicName)
    {
        return PlayerHasUniqueRelic[relicName];
    }

    public bool DropRelic(List<string> relics, List<float> relicChance, Vector3 deadEnemyPosition)
    {
        for (var i = 0; i < relics.Count; i++)
        {
            var rolledValue = Random.value;
            if (rolledValue <= relicChance[i])
            {
                CreateItem(relics[i], deadEnemyPosition);
                return true;
            }
        }

        return false;
    }

    private void CreateItem(string relic, Vector3 deadEnemyPosition)
    {
        if (PlayerHasUniqueRelic.ContainsKey(relic))
        {
            if (!PlayerHasUniqueRelic[relic])
            {
                Instantiate(relicsDictionary[relic], deadEnemyPosition, Quaternion.identity);
            }
        }
        else if (relicsDictionary.ContainsKey(relic))
        {
            Instantiate(relicsDictionary[relic], deadEnemyPosition, Quaternion.identity);
        }
    }
}

