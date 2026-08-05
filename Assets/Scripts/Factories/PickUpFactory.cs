using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PickUpFactory : MonoBehaviour
{
    public static PickUpFactory Instance { get; private set; } = null;

    [SerializeField]
    private List<GameObject> pickupsList;
    private Dictionary<string, GameObject> pickupsDictionary;
    private PlayerStats playerStats;

    private readonly float healthDropRate = 0.1f;
    private readonly float energyDropRate = 0.075f;

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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == SceneNames.Town)
        {
            playerStats = FindObjectOfType<PlayerStats>();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        pickupsDictionary = new Dictionary<string, GameObject>();

        for (int i = 0; i < pickupsList.Count; i++)
        {
            pickupsDictionary.Add(pickupsList[i].name, pickupsList[i]);
        }
    }

    private void CreatePickup(Vector3 spawnPosition, string pickup)
    {
        Instantiate(pickupsDictionary[pickup], spawnPosition, Quaternion.identity);
    }

    public bool DropPickup(Vector3 spawnPosition)
    {
        var dropChance = healthDropRate + (playerStats.Luck / 2);
        dropChance = dropChance < 0.3f ? dropChance : 0.3f;

        if (Random.value < dropChance)
        {
            CreatePickup(spawnPosition, "HealthPickup");
            return true;
        }

        dropChance = energyDropRate + (playerStats.Luck / 2);
        dropChance = dropChance < 0.3f ? dropChance : 0.3f;

        if (Random.value < dropChance)
        {
            CreatePickup(spawnPosition, "EnergyPickup");
            return true;
        }

        return false;
    }

    public bool DropGold(Vector3 spawnPosition, float goldDropChance, int minGoldGiven, int maxGoldGiven, bool dropRandomCoins = false)
    {
        var dropChance = goldDropChance + (playerStats.Luck / 4);

        if (Random.value < dropChance)
        {
            var minCopperCoins = minGoldGiven / CoinPickup.copperValue;
            var maxCopperCoins = maxGoldGiven / CoinPickup.copperValue;
            var finalGoldGiven = Random.Range(minCopperCoins, maxCopperCoins + 1) * CoinPickup.copperValue;
            
            int GoldCoins;
            int SilverCoins;
            int CooperCoins;

            if (dropRandomCoins)
            {
                GoldCoins = Random.Range(0, finalGoldGiven / CoinPickup.goldValue);
                SilverCoins = Random.Range(0, (finalGoldGiven - GoldCoins * CoinPickup.goldValue) / CoinPickup.silverValue);
                CooperCoins = (finalGoldGiven - (GoldCoins * CoinPickup.goldValue + SilverCoins * CoinPickup.silverValue)) / CoinPickup.copperValue;
            }
            else
            {
                 GoldCoins = finalGoldGiven / CoinPickup.goldValue;
                 SilverCoins = (finalGoldGiven - GoldCoins * CoinPickup.goldValue) / CoinPickup.silverValue;
                 CooperCoins = (finalGoldGiven - (GoldCoins * CoinPickup.goldValue + SilverCoins * CoinPickup.silverValue)) / CoinPickup.copperValue;
            }
            

            for (var i = 0; i < GoldCoins; i++)
            {
                CreatePickup(spawnPosition, "Gold");
            }

            for (var i = 0; i < SilverCoins; i++)
            {
                CreatePickup(spawnPosition, "Silver");
            }

            for (var i = 0; i < CooperCoins; i++)
            {
                CreatePickup(spawnPosition, "Copper");
            }

            return true;
        }

        return false;
    }
}
