using UnityEngine;
using System.Collections;

public class ItemStatsChanger : MonoBehaviour
{
    [SerializeField]
    private int currentHealth = 0;
    [SerializeField]
    private int maxHealth = 0;
    [SerializeField]
    private int currentEnergy = 0;
    [SerializeField]
    private int maxEnergy = 0;
    [SerializeField]
    private float speed = 0;
    [SerializeField]
    private int damage = 0;
    [SerializeField]
    private int ammo = 0;
    [SerializeField]
    private int gold = 0;
    [SerializeField]
    private int armor = 0;
    [SerializeField]
    private float luck = 0;
    [SerializeField]
    private float effectChance = 0;
    [SerializeField]
    private int lifesteal = 0;
    [SerializeField]
    private float lifestealChance = 0;
    [SerializeField]
    private int energysteal = 0;
    [SerializeField]
    private float energystealChance = 0;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();

            playerStats.MaximumHealth += maxHealth;
            playerStats.ApplyHeal(currentHealth);
            playerStats.MaximumEnergy += maxEnergy;
            playerStats.CurrentEnergy += currentEnergy;
            playerStats.Speed += speed;
            playerStats.Damage += damage;
            playerStats.Ammo += ammo;
            playerStats.Gold += gold;
            playerStats.Luck += luck;
            playerStats.Armor += armor;
            playerStats.Lifesteal += lifesteal;
            playerStats.LifestealChance += lifestealChance;
            playerStats.Energysteal += energysteal;
            playerStats.EnergystealChance += energystealChance;
            playerStats.EffectChance += effectChance;
        }
    }

}
