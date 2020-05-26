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
    private bool LastHitCritical = false;
    private Vector2 projectileVelocityOnHit;
    

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyStats = GetComponent<EnemyStats>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        enemyStats.EnemyDied += EnemyDiedFrom;
    }

    private void OnDisable()
    {
        enemyStats.EnemyDied -= EnemyDiedFrom;
    }

    public void Hit(int damage)
    {
        if (enemyStats.Damageable)
        {
            LastHitCritical = false;
            ProcessHit();
            StartCoroutine(FlashRed());
            enemyStats.Health -= damage;
        }
    }

    public void CriticalHit(int damage,Vector2 projectileVelocityOnHit)
    {
        if (enemyStats.Damageable)
        {
            LastHitCritical = true;
            ProcessHit();
            StartCoroutine(FlashDarkRed());
            this.projectileVelocityOnHit = projectileVelocityOnHit;
            enemyStats.Health -= damage * 3;           
        }
    }

    private void ProcessHit()
    {
        if(gotHitSound != null)
        {
            audioSource.PlayOneShot(gotHitSound);
        }
    }

    private void EnemyDiedFrom()
    {
        GetComponent<EnemyLoader>().ChangeEnemyStatusForSave(LastHitCritical);
        GetComponent<EnemyDeath>().ProcessDeath(LastHitCritical, projectileVelocityOnHit);
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

    

