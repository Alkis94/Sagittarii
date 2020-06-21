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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();

            playerStats.MaximumHealth += maxHealth;
            playerStats.CurrentHealth += currentHealth;
            playerStats.MaximumEnergy += maxEnergy;
            playerStats.CurrentEnergy += currentEnergy;
            playerStats.Speed += speed;
            playerStats.Damage += damage;
            playerStats.Ammo += ammo;
            playerStats.Gold += gold;
        }
    }

}
