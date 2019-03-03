using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private  int currentHealth = 100;
    private  int maximumHealth = 100;
    public  int damage = 10;
    public  int ammo = 500;
    [HideInInspector]
    private int gold = 0;

    public static event Action<int,int> OnPlayerHealthChanged = delegate { };
    public static event Action<int> OnPlayerGoldChanged = delegate { };

    private void OnEnable()
    {
        EnemyGotShot.OnEnemyDeathGiveGold += UpdateGold;
    }

    private void OnDisable()
    {
        EnemyGotShot.OnEnemyDeathGiveGold -= UpdateGold;
    }

    private void Start()
    {
        OnPlayerHealthChanged?.Invoke(currentHealth, maximumHealth);
        OnPlayerGoldChanged?.Invoke(gold);
    }

    public  int CurrentHealth
    {
        get
        {
            return currentHealth;
        }

        set
        {
            int newCurrentHealth = value;

            if (newCurrentHealth <= MaximumHealth)
            {
                currentHealth = newCurrentHealth;
            }

            if (newCurrentHealth > MaximumHealth)
            {
                currentHealth = MaximumHealth;
            }

            OnPlayerHealthChanged?.Invoke(currentHealth,maximumHealth);
        }
    }

    public  int MaximumHealth
    {
        get
        {
            return maximumHealth;
        }

        set
        {
            maximumHealth = value;
            OnPlayerHealthChanged?.Invoke(currentHealth, maximumHealth);
        }
    }

    public int Gold
    {
        get
        {
            return gold;
        }

        set
        {
            gold = value;
            OnPlayerGoldChanged?.Invoke(gold);
        }
    }


    private void UpdateGold(int goldGiven)
    {
        Gold += goldGiven;
    }
}

