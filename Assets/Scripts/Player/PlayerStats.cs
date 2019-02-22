using System;
using UnityEngine;

public static class PlayerStats
{
    public static int CurrentHealth { get; private set; } = 100;
    public static int MaximumHealth { get; private set; } = 100;
    public static int Damage = 10;
    public static int Ammo = 500;
    public static int Gold = 0;

    public static event Action OnPlayerHealthChanged = delegate { };

    public static void ChangePlayerCurrentHealth (int newCurrentHealth)
    {
        if(CurrentHealth <= MaximumHealth)
        {
            CurrentHealth = newCurrentHealth;
        }

        if(CurrentHealth > MaximumHealth)
        {
            CurrentHealth = MaximumHealth;
        }

        OnPlayerHealthChanged?.Invoke();
    }

    public static void ChangePlayerMaximumHealth(int newMaxHealth)
    {
        MaximumHealth = newMaxHealth;
        OnPlayerHealthChanged?.Invoke();
    }

}

