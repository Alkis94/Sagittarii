using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour, IDamageable
{

    public static event Action OnPlayerDied = delegate { };

    [SerializeField]
    private CharacterClass characterClass;
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
    [SerializeField]
    private int armor = 0;
    [SerializeField]
    private float luck = 0;
    [SerializeField]
    private float speed = 8;
    [SerializeField]
    private float projectileSpeed = 10;
    [SerializeField]
    private int damage = 0;
    [SerializeField]
    private int lifesteal = 0;
    [SerializeField]
    private float lifestealChance = 0;
    [SerializeField]
    private int energysteal = 0;
    [SerializeField]
    private float energystealChance = 0;
    
    private void OnEnable()
    {
        EnemiesSerializer.OnRoomHasAliveEnemies += EnteredRoomWithEnemies;
        EnemyStats.OnEnemyWasKilled += EnemyWasKilled;      
    }

    private void OnDisable()
    {
        EnemiesSerializer.OnRoomHasAliveEnemies -= EnteredRoomWithEnemies;
        EnemyStats.OnEnemyWasKilled -= EnemyWasKilled;
    }

    private void EnemyWasKilled (DamageSource damageSource)
    {
        if(damageSource == DamageSource.projectile)
        {
            float randomNumber = UnityEngine.Random.Range(0f, 1f);

            if(randomNumber < LifestealChance)
            {
                CurrentHealth += Lifesteal;
            }

            randomNumber = UnityEngine.Random.Range(0f, 1f);

            if (randomNumber < EnergystealChance)
            {
                CurrentHealth += Energysteal;
            }

        }
    }

    private void Start()
    {
        UIManager.Instance.UpdateHealth(CurrentHealth, MaximumHealth);
        UIManager.Instance.UpdateEnergy(CurrentEnergy, MaximumEnergy);
        UIManager.Instance.UpdateGold(Gold);
        UIManager.Instance.UpdateAmmo(Ammo);
    }

    public void ApplyDamage(int damage, DamageSource damageSource, DamageType damageType = DamageType.normal)
    {
        if(damageSource == DamageSource.projectile)
        {
            int damageToTake = damage - armor;
            damageToTake = damageToTake > damage / 2 ? damageToTake : damage / 2;
            CurrentHealth -= damageToTake;
        }
        else if (damageSource == DamageSource.traps)
        {
            CurrentHealth -= damage;
        }
        else 
        {
            CurrentHealth -= damage;
        }
        
    }

    public void ApplyHeal(int healAmount)
    {
        CurrentHealth += healAmount;
    }

    public CharacterClass CharacterClass
    {
        get => characterClass;

        set => characterClass = value;
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

            UIManager.Instance.UpdateHealth(CurrentHealth, MaximumHealth);

            if (currentHealth <= 0)
            {
                OnPlayerDied?.Invoke();
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
            UIManager.Instance.UpdateHealth(CurrentHealth, MaximumHealth);
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

            if (newCurrentExhaustion < 0)
            {
                currentEnergy = 0;
            }
            else if (newCurrentExhaustion <= MaximumEnergy)
            {
                currentEnergy = newCurrentExhaustion;
            }
            else if (newCurrentExhaustion > MaximumEnergy)
            {
                currentEnergy = MaximumEnergy;
            }

            UIManager.Instance.UpdateEnergy(CurrentEnergy, MaximumEnergy);
        }
    }

    public int MaximumEnergy
    {
        get
        {
            return maximumEnergy;
        }

        set
        {
            maximumEnergy = value;
            UIManager.Instance.UpdateEnergy(CurrentEnergy, MaximumEnergy);
        }
    }

    public int Damage
    {
        get
        {
            return damage;
        }

        set
        {
            damage = value > 0 ? value : 0;
        }
    }

    public int Armor
    {
        get
        {
            return armor;
        }

        set
        {
            armor = value > 0 ? value : 0;
        }
    }

    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = Mathf.Clamp(value, 4, 10);
        }
    }

    public float ProjectileSpeed
    {
        get
        {
            return projectileSpeed;
        }

        set
        {
            projectileSpeed = Mathf.Clamp(value, 4, 20);
        }
    }

    public float Luck
    {
        get
        {
            return luck;
        }

        set
        {
            luck = Mathf.Clamp(value, -0.1f, 0.3f);
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
            UIManager.Instance.UpdateAmmo(Ammo);
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
            UIManager.Instance.UpdateGold(Gold);
        }
    }

    public int Lifesteal
    {
        get
        {
            return lifesteal;
        }

        set
        {
            lifesteal = value > 0 ? value : 0;
        }
    }

    public float LifestealChance
    {
        get
        {
            return lifestealChance;
        }

        set
        {
            lifestealChance = value > 0 ? value : 0;
        }
    }

    public int Energysteal
    {
        get
        {
            return energysteal;
        }

        set
        {
            energysteal = value > 0 ? value : 0;
        }
    }
   
    public float EnergystealChance
    {
        get
        {
            return energystealChance;
        }

        set
        {
            energystealChance = value > 0 ? value : 0;
        }
    }

    private void EnteredRoomWithEnemies()
    {
        CurrentEnergy -= 1;

        if (CurrentEnergy <= 0)
        {
            CurrentHealth -= (int)(MaximumHealth * 0.05f);
        }
    }

}

