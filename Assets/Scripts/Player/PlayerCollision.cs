using System.Collections;
using UnityEngine;


public class PlayerCollision : MonoBehaviour
{
    private PlayerAudio playerAudio;
    private PlayerStats playerStats;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        playerAudio = GetComponent<PlayerAudio>();
        playerStats = GetComponent<PlayerStats>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(playerStats.CurrentHealth > 0)
        {
            if (other.tag == "EnemyProjectile")
            {
                int damage = other.GetComponent<ProjectileDataInitializer>().Damage;
                playerStats.ApplyDamage(damage, DamageSource.projectile);
                StartCoroutine(FlashDarkRed());
                playerAudio.PlayGotHitSound();
            }

            if (other.tag == "Spikes")
            {
                playerStats.ApplyDamage(playerStats.MaximumHealth, DamageSource.traps);
                playerAudio.PlayGotHitSound();
            }
        }
    }

    IEnumerator FlashDarkRed()
    {
        spriteRenderer.color = new Color(1f, 0f, 0f, 1f);
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = new Color(1f, 1f, 1f);
    }

}
