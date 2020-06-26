using UnityEngine;
using System.Collections.Generic;

public class PickUpFactory : MonoBehaviour
{

    public static PickUpFactory Instance { get; private set; } = null;

    [SerializeField]
    private List<GameObject> pickupsList;
    private Dictionary<string, GameObject> pickupsDictionery;
    private PlayerStats playerStats;

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

    private void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        pickupsDictionery = new Dictionary<string, GameObject>();

        for (int i=0; i < pickupsList.Count; i++)
        {
            pickupsDictionery.Add(pickupsList[i].name, pickupsList[i]);
        }
    }

    private void CreatePickup(Vector3 spawnPosition, string pickup)
    {
        Instantiate(pickupsDictionery[pickup], spawnPosition, Quaternion.identity);
    }

    public bool DropPickup(Vector3 spawnPosition, string pickup, float dropRate)
    {
        float randomNumber = Random.Range(0f, 1f);
        float dropChance = dropRate + playerStats.Luck;
        
        if (randomNumber < dropChance)
        {
            CreatePickup(spawnPosition, pickup);
            return true;
        }
        return false;
    }

    public void DropGold(Vector3 spawnPosition, float goldDropChance, int minGoldGiven, int maxGoldGiven)
    {
        float randomNumber = Random.Range(0f, 1f);
        float dropChance = goldDropChance + (playerStats.Luck / 2);

        if (randomNumber < dropChance)
        {
            int minCopperCoins = minGoldGiven / CoinPickup.copperValue;
            int maxCopperCoins = maxGoldGiven / CoinPickup.copperValue;
            int finalGoldGiven = Random.Range(minCopperCoins, maxCopperCoins + 1) * CoinPickup.copperValue;

            int GoldCoins = finalGoldGiven / CoinPickup.goldValue;
            int SilverCoins = (finalGoldGiven - GoldCoins * CoinPickup.goldValue) / CoinPickup.silverValue;
            int CooperCoins = (finalGoldGiven - (GoldCoins * CoinPickup.goldValue + SilverCoins * CoinPickup.silverValue)) / CoinPickup.copperValue;

            for (int i = 0; i < GoldCoins; i++)
            {
                CreatePickup(spawnPosition, "Gold");
            }

            for (int i = 0; i < SilverCoins; i++)
            {
                CreatePickup(spawnPosition, "Silver");
            }

            for (int i = 0; i < CooperCoins; i++)
            {
                CreatePickup(spawnPosition, "Copper");
            }
        }
    }
}
