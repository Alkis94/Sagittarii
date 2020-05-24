using System;
using UnityEngine;
using System.Collections;

public class EnemyGotShot : MonoBehaviour
{

    public event Action<bool,Vector2> EnemyDiedAndHow = delegate { };
   
    private EnemyStats enemyStats;
    private EnemyWasCriticalHit enemyWasCriticalHit;
    private EnemyWasHit enemyWasHit;
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
        enemyWasCriticalHit = GetComponentInChildren<EnemyWasCriticalHit>();
        enemyWasHit = GetComponentInChildren<EnemyWasHit>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (enemyWasCriticalHit != null)
        {
            enemyWasCriticalHit.OnCriticalHit += CriticalHit;
        }

        enemyWasHit.OnHit += Hit;
        enemyStats.EnemyDied += EnemyDiedFrom;
    }

    private void OnDisable()
    {
        if (enemyWasCriticalHit != null)
        {
            enemyWasCriticalHit.OnCriticalHit -= CriticalHit;
        }

        enemyWasHit.OnHit -= Hit;
        enemyStats.EnemyDied -= EnemyDiedFrom;
    }

    private void Hit(int damage)
    {
        if (enemyStats.Damageable)
        {
            LastHitCritical = false;
            ProcessHit();
            StartCoroutine(FlashRed());
            enemyStats.Health -= damage;
        }
    }

    private void CriticalHit(int damage,Vector2 projectileVelocityOnHit)
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
        EnemyDiedAndHow?.Invoke(LastHitCritical,projectileVelocityOnHit);
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

    

