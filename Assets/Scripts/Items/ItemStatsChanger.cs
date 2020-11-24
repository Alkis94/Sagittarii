using UnityEngine;
using System.Collections;

public class ItemStatsChanger : MonoBehaviour
{
    [SerializeField][Range(0, 100)]
    private int healthPercentageHeal = 0;
    [SerializeField]
    private int maxHealthPercentageIncrease = 0;
    [SerializeField]
    private int currentHealth = 0;
    [SerializeField]
    private int maxHealth = 0;
    [SerializeField]
    private float healthMultiplier = 0;
    [SerializeField]
    private int currentEnergy = 0;
    [SerializeField]
    private int maxEnergy = 0;
    [SerializeField]
    private float speed = 0;
    [SerializeField]
    private int damage = 0;
    [SerializeField]
    private float damagePercentageIncrease = 0;
    [SerializeField]
    private float damageMultiplier = 0f;
    [SerializeField]
    private float damageTakenMultiplier = 0f;
    [SerializeField]
    private int ammo = 0;
    [SerializeField]
    private int gold = 0;
    [SerializeField]
    private int armor = 0;
    [SerializeField]
    private float luck = 0;
    [SerializeField]
    private int lifesteal = 0;
    [SerializeField]
    private float lifestealChance = 0;
    [SerializeField]
    private int energysteal = 0;
    [SerializeField]
    private float energystealChance = 0;
    [SerializeField]
    private int timeLimit = 0;
    [SerializeField]
    private int townTax = 0;
    [SerializeField]
    private int restCost = 0;
    [SerializeField]
    private float cooldown = 0;
    [SerializeField]
    private int extraLives = 0;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();
            playerStats.HealthMultiplier += healthMultiplier;
            playerStats.AddMaxHealth(maxHealthPercentageIncrease,true);
            playerStats.AddMaxHealth(maxHealth);
            playerStats.ApplyHeal(healthPercentageHeal, true);
            playerStats.ApplyHeal(currentHealth);
            playerStats.MaximumEnergy += maxEnergy;
            playerStats.CurrentEnergy += currentEnergy;
            playerStats.Speed += speed;
            playerStats.Damage += damage;
            playerStats.IncreaseDamageByPercentage(damagePercentageIncrease);
            playerStats.DamageMultiplier += damageMultiplier;
            playerStats.DamageTakenMultiplier += damageTakenMultiplier;
            playerStats.Ammo += ammo;
            playerStats.Gold += gold;
            playerStats.Luck += luck;
            playerStats.Armor += armor;
            playerStats.Lifesteal += lifesteal;
            playerStats.LifestealChance += lifestealChance;
            playerStats.Energysteal += energysteal;
            playerStats.EnergystealChance += energystealChance;
            playerStats.TimeLimit += timeLimit;
            playerStats.TownTax += townTax;
            playerStats.RestCost += restCost;
            playerStats.Cooldown += cooldown;
            playerStats.ExtraLives += extraLives;
        }
    }

}
