using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private  int currentHealth = 58;
    [SerializeField]
    private  int maximumHealth = 100;
    [SerializeField]
    private int currentEnergy = 20;
    [SerializeField]
    private int maximumEnergy = 20;
    [SerializeField]
    private int ammo = 500;
    [SerializeField]
    private int gold = 20;

    public float speed = 8f;
    public  int damage = 10;
    

    public static event Action<int,int> OnPlayerHealthChanged = delegate { };
    public static event Action<int> OnPlayerGoldChanged = delegate { };
    public static event Action<int> OnPlayerAmmoChanged = delegate { };
    public static event Action<int, int> OnPlayerEnergyChanged = delegate { };
    public static event Action PlayerDied = delegate { };

    private void Start()
    {
        OnPlayerHealthChanged?.Invoke(currentHealth, maximumHealth);
        OnPlayerGoldChanged?.Invoke(gold);
        OnPlayerAmmoChanged?.Invoke(ammo);
        OnPlayerEnergyChanged?.Invoke(currentEnergy, maximumEnergy);
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

            if(currentHealth <= 0)
            {
                PlayerDied?.Invoke();
            }
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

    public int CurrentEnergy
    {
        get
        {
            return currentEnergy;
        }

        set
        {
            int newCurrentExhaustion = value;

            if (newCurrentExhaustion <= MaximumExhaustion)
            {
                currentEnergy = newCurrentExhaustion;
            }

            if (newCurrentExhaustion > MaximumExhaustion)
            {
                currentEnergy = MaximumExhaustion;
            }

            OnPlayerEnergyChanged?.Invoke(currentEnergy, maximumEnergy);
        }
    }

    public int MaximumExhaustion
    {
        get
        {
            return maximumEnergy;
        }

        set
        {
            maximumEnergy = value;
            OnPlayerEnergyChanged?.Invoke(currentEnergy, maximumEnergy);
        }
    }

    public int Ammo
    {
        get
        {
            return ammo;
        }

        set
        {
            ammo = value;
            OnPlayerAmmoChanged?.Invoke(ammo);
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

}

