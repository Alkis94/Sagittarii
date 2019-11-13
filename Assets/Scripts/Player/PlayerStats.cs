using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private  int currentHealth = 58;
    [SerializeField]
    private  int maximumHealth = 100;
    [SerializeField]
    private int ammo = 500;
    [SerializeField]
    private int gold = 20;

    public float speed = 8f;
    public  int damage = 10;
    

    public static event Action<int,int> OnPlayerHealthChanged = delegate { };
    public static event Action<int> OnPlayerGoldChanged = delegate { };
    public static event Action<int> OnPlayerAmmoChanged = delegate { };

    private void Start()
    {
        OnPlayerHealthChanged?.Invoke(currentHealth, maximumHealth);
        OnPlayerGoldChanged?.Invoke(gold);
        OnPlayerAmmoChanged?.Invoke(ammo);
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

