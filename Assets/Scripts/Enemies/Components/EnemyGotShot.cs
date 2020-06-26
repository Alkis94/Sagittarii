using System;
using UnityEngine;
using System.Collections;

public class EnemyGotShot : MonoBehaviour
{
    private EnemyStats enemyStats;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip gotHitSound;
    public Vector2 ProjectileVelocityOnHit { get; private set; }


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyStats = GetComponent<EnemyStats>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Hit(int damage)
    {
        if (enemyStats.Damageable)
        {
            ProcessHit();
            StartCoroutine(FlashRed());
            enemyStats.ApplyDamage(damage, DamageSource.projectile);
        }
    }

    public void CriticalHit(int damage,Vector2 projectileVelocityOnHit)
    {
        if (enemyStats.Damageable)
        {
            ProcessHit();
            StartCoroutine(FlashDarkRed());
            ProjectileVelocityOnHit = projectileVelocityOnHit;
            enemyStats.ApplyDamage(damage,  DamageSource.projectile, DamageType.critical);
        }
    }

    private void ProcessHit()
    {
        if(gotHitSound != null)
        {
            audioSource.PlayOneShot(gotHitSound);
        }
    }

    IEnumerator FlashRed()
    {
        spriteRenderer.color = new Color(1f, 0.5f, 0.25f, 1f);
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }

    IEnumerator FlashDarkRed()
    {
        spriteRenderer.color = new Color(1f, 0f, 0f, 1f);
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = new Color(1f, 1f, 1f);
    }
}

    

