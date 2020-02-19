using System;
using System.Collections.Generic;
using UnityEngine;
using Factories;

public class PlayerCollision : MonoBehaviour
{
    private PlayerAudio playerAudio;
    private PlayerStats playerStats;

    
    public static event Action OnPlayerGotBatWings = delegate { };
    public static event Action OnPlayerGotTrident = delegate { };

    void OnEnable()
    {
        PlayerStats.PlayerDied += DisableScript;
    }


    void OnDisable()
    {
        PlayerStats.PlayerDied -= DisableScript;
    }

    private void Start()
    {
        playerAudio = GetComponent<PlayerAudio>();
        playerStats = GetComponent<PlayerStats>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyProjectile")
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
        playerAudio.PlayGotHitSound();
    }

    private void DisableScript()
    {
       enabled = false;
    }
   

}
