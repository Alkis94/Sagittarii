using System;
using System.Collections.Generic;
using UnityEngine;
using Factories;

public class PlayerCollision : MonoBehaviour
{
    private PlayerAudio playerAudio;
    private PlayerStats playerStats;

    private void Start()
    {
        playerAudio = GetComponent<PlayerAudio>();
        playerStats = GetComponent<PlayerStats>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(playerStats.CurrentHealth > 0)
        {
            if (other.tag == "EnemyProjectile")
            {
                int damage = other.GetComponent<Projectile>().Damage;
                PlayerGotHit(damage);
            }

            if (other.tag == "Spikes")
            {
                PlayerGotHit(playerStats.MaximumHealth);
            }
        }
    }

    private void PlayerGotHit(int damage)
    {
        playerStats.CurrentHealth -= damage;
        playerAudio.PlayGotHitSound();
    }
}
