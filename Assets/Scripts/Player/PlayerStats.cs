using System;
using UnityEngine;

public static class PlayerStats
{
    private static int currentHealth = 100;
    private static int maximumHealth = 100;
    public static int damage = 10;
    public static int ammo = 500;
    public static int gold = 0;

    public static event Action OnPlayerHealthChanged = delegate { };

    static PlayerStats()
    {
        currentHealth = MaximumHealth;
    }

    public static int CurrentHealth
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

            OnPlayerHealthChanged?.Invoke();
        }
    }

    public static int MaximumHealth
    {
        get
        {
            return maximumHealth;
        }

        set
        {
            maximumHealth = value;
            OnPlayerHealthChanged?.Invoke();
        }
    }
}

