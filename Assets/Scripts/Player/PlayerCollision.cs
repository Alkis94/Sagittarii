using System;
using System.Collections.Generic;
using UnityEngine;
using Factories;

public class PlayerCollision : MonoBehaviour
{
    private PlayerAudio playerAudio;
    private Animator animator;
    private PlayerStats playerStats;

    public static event Action OnDeath = delegate { };
    public static event Action OnPlayerGotBatWings = delegate { };
    public static event Action OnPlayerGotTrident = delegate { };
    

    private bool playerNotDead = true;

    private void Start()
    {
        playerAudio = GetComponent<PlayerAudio>();
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyProjectile" && playerNotDead)
        {
            int damage = other.GetComponent<Projectile>().Damage;
            PlayerGotHit(damage);
        }
        if (other.tag == "BatWingsPickup")
        {
            RelicFactory.PlayerHasRelic["BatWings"] = true;
            OnPlayerGotBatWings?.Invoke();
        }
        if (other.tag == "Trident")
        {
            RelicFactory.PlayerHasRelic["Trident"] = true;
            OnPlayerGotTrident?.Invoke();
        }

    }

    private void PlayerGotHit(int damage)
    {
        playerStats.CurrentHealth -= damage;

        if (playerStats.CurrentHealth < 1)
        {
            playerNotDead = false;
            Die();
            OnDeath?.Invoke();
        }
        else
        {
            playerAudio.PlayGotHitSound();
        }
    }

    private void Die()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        playerAudio.PlayDeathSound();
        animator.SetTrigger("PlayerDied");
        Invoke("PlayerDiedDelayedMenu", 3);
    }

    private void PlayerDiedDelayedMenu()
    {
        MenuFactory.CreateMenuAndPause(MenuFactory.DefeatMenu);
    }

}
