using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private  int currentHealth = 58;
    [SerializeField]
    private  int maximumHealth = 100;
    [SerializeField]
    private int currentExhaustion = 0;
    [SerializeField]
    private int maximumExhaustion = 20;
    [SerializeField]
    private int ammo = 500;
    [SerializeField]
    private int gold = 20;

    public float speed = 8f;
    public  int damage = 10;
    

    public static event Action<int,int> OnPlayerHealthChanged = delegate { };
    public static event Action<int> OnPlayerGoldChanged = delegate { };
    public static event Action<int> OnPlayerAmmoChanged = delegate { };
    public static event Action<int, int> OnPlayerExhaustionChanged = delegate { };
    public static event Action PlayerDied = delegate { };

    private void Start()
    {
        OnPlayerHealthChanged?.Invoke(currentHealth, maximumHealth);
        OnPlayerGoldChanged?.Invoke(gold);
        OnPlayerAmmoChanged?.Invoke(ammo);
        OnPlayerExhaustionChanged?.Invoke(currentExhaustion, maximumExhaustion);
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

    public int CurrentExhaustion
    {
        get
        {
            return currentExhaustion;
        }

        set
        {
            int newCurrentExhaustion = value;

            if (newCurrentExhaustion <= MaximumExhaustion)
            {
                currentExhaustion = newCurrentExhaustion;
            }

            if (newCurrentExhaustion > MaximumExhaustion)
            {
                currentExhaustion = MaximumExhaustion;
            }

            OnPlayerExhaustionChanged?.Invoke(currentExhaustion, maximumExhaustion);
        }
    }

    public int MaximumExhaustion
    {
        get
        {
            return maximumExhaustion;
        }

        set
        {
            maximumExhaustion = value;
            OnPlayerExhaustionChanged?.Invoke(currentExhaustion, maximumExhaustion);
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

