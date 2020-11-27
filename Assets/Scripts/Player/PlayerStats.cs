using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{

    public static event Action<DamageSource> OnPlayerDied = delegate { };

    [SerializeField]
    private CharacterClass characterClass;
    [SerializeField]
    private  int currentHealth = 100;
    [SerializeField]
    private  int maximumHealth = 100;
    [SerializeField]
    private float healthMultiplier = 1f;
    [SerializeField]
    private int currentEnergy = 30;
    [SerializeField]
    private int maximumEnergy = 30;
    [SerializeField]
    private int ammo = 500;
    [SerializeField]
    private int gold = 0;
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
    private float damageMultiplier = 1f;
    [SerializeField]
    private float damageTakenMultiplier = 1f;
    [SerializeField]
    private int lifesteal = 0;
    [SerializeField]
    private float lifestealChance = 0;
    [SerializeField]
    private int energysteal = 0;
    [SerializeField]
    private float energystealChance = 0;
    [SerializeField]
    private int timeLimit = 600;
    [SerializeField]
    private int townTax = 100;
    [SerializeField]
    private int restCost = 10;
    [SerializeField]
    private float cooldown = 10;
    [SerializeField]
    private int extraLives = 0;



    private DamageSource lastDamageSource = DamageSource.projectile;
    
    private void OnEnable()
    {
        EnemiesManager.OnRoomHasAliveEnemies += EnteredRoomWithEnemies;
        EnemyStats.OnEnemyWasKilled += EnemyWasKilled;
        PlayerDeath.OnPlayerRessurected += Ressurection;
    }

    private void OnDisable()
    {
        EnemiesManager.OnRoomHasAliveEnemies -= EnteredRoomWithEnemies;
        EnemyStats.OnEnemyWasKilled -= EnemyWasKilled;
        PlayerDeath.OnPlayerRessurected += Ressurection;
    }

    private void Ressurection ()
    {
        CurrentHealth = MaximumHealth;
        CurrentEnergy = MaximumEnergy;
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
        UIManager.Instance.SetHealth(CurrentHealth, MaximumHealth);
        UIManager.Instance.UpdateEnergy(CurrentEnergy, MaximumEnergy);
        UIManager.Instance.UpdateGold(Gold);
        UIManager.Instance.UpdateAmmo(Ammo);
    }

    public void ApplyDamage(int damage, DamageSource damageSource = DamageSource.projectile, DamageType damageType = DamageType.normal)
    {
        lastDamageSource = damageSource;

        if(damageSource == DamageSource.projectile)
        {
            int damageToTake = (int)(damage * DamageTakenMultiplier) - armor;
            damageToTake = damageToTake > damage / 2 ? damageToTake : damage / 2;
            CurrentHealth -= damageToTake;
        }
        else if (damageSource == DamageSource.traps)
        {
            CurrentHealth -= (int)(damage * DamageTakenMultiplier);
        }
        else 
        {
            CurrentHealth -= (int)(damage * DamageTakenMultiplier);
        }
        
    }

    public void ApplyHeal(int healAmount, bool isPercentage = false)
    {
        if(isPercentage)
        {
            float percentage = (float)healAmount / 100;
            CurrentHealth += (int)(MaximumHealth * percentage);
        }
        else
        {
            CurrentHealth += (int)(healAmount * healthMultiplier);
        }
    }

    public void AddMaxHealth(int healthAmount, bool isPercentage = false)
    {
        if (isPercentage)
        {
            float percentage = (float)healthAmount / 100;
            CurrentHealth += (int)(CurrentHealth * percentage);
        }
        else
        {
            MaximumHealth += (int)(healthAmount * healthMultiplier);
        }
    }

    public void AddDamage(int damageAmount, bool isPercentage)
    {
        if(isPercentage)
        {
            Damage += (Damage * damageAmount / 100);
        }
        else
        {
            Damage += (int)(damageAmount * DamageMultiplier);
        }
        
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
                OnPlayerDied?.Invoke(lastDamageSource);
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

    public float HealthMultiplier
    {
        get
        {
            return healthMultiplier;
        }

        set
        {
            healthMultiplier = value;
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
                ApplyDamage((int)(MaximumHealth * 0.05f), DamageSource.exhaustion);
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

    public float DamageMultiplier
    {
        get
        {
            return damageMultiplier;
        }

        set
        {
            damageMultiplier = value;
        }
    }

    public float DamageTakenMultiplier
    {
        get
        {
            return damageTakenMultiplier;
        }

        set
        {
            damageTakenMultiplier = value;
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
            luck = Mathf.Clamp(value, 0.0f, 0.3f);
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
            if(value - gold != 0)
            {
                UIManager.Instance.GoldGained += value - gold;
            }
            gold = value;
        }
    }

    public int GoldWithoutGainedGold
    {
        get
        {
            return gold;
        }

        set
        {
            gold = value;
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

    public int TimeLimit
    {
        get
        {
            return timeLimit;
        }

        set
        {
            timeLimit = value > 180 ? value : 180;
        }
    }

    public int TownTax
    {
        get
        {
            return townTax;
        }

        set
        {
            townTax = value;
        }
    }

    public int RestCost
    {
        get
        {
            return restCost;
        }

        set
        {
            restCost = value;
        }
    }

    public float Cooldown
    {
        get
        {
            return cooldown;
        }

        set
        {
            cooldown = value;
            GetComponent<SpecialAbility>().Cooldown = cooldown;
        }
    }

    public int ExtraLives
    {
        get
        {
            return extraLives;
        }

        set
        {
            extraLives = value;
        }
    }


    private void EnteredRoomWithEnemies()
    {
        CurrentEnergy --;
    }

    

}

