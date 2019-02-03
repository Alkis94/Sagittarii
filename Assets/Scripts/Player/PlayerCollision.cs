using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    
    public AudioClip PlayerGotHitSound;
    private AudioSource audioSource;

    public static event Action OnDeath = delegate { };
    public static event Action OnPlayerGotBatWings = delegate { };
    public static event Action OnPlayerGotDeadBird = delegate { };

    public static event Action OnPlayerHealthChanged = delegate { };

    private bool PlayerNotDead = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyProjectile" && PlayerNotDead)
        {
            PlayerGotHit();
        }

        if (other.tag == "HealthPickup" && PlayerStats.MaximumHealth > PlayerStats.CurrentHealth)
        {
            IncreaseCurrentHealthAndCallUpdateUI();
        }
        if (other.tag == "MaximumHealthPickup")
        {
            PlayerStats.MaximumHealth += 10;
            IncreaseCurrentHealthAndCallUpdateUI();
        }
        if (other.tag == "BatWingsPickup")
        {
            ItemHandler.PlayerHasBatWings = true;
            OnPlayerGotBatWings?.Invoke();

        }
        if (other.tag == "WolfPawPickup")
        {
            ItemHandler.WolfPawMultiplier += 1;
        }
        if (other.tag == "DeadBirdPickup")
        {
            ItemHandler.ItemDropped["DeadBird"] = true;
            if (OnPlayerGotDeadBird != null) OnPlayerGotDeadBird();
        }
        if (other.tag == "ImpFlamePickup")
        {
            ItemHandler.PlayerHasImpFlame = true;
            PlayerStats.Damage += 10;
        }

    }


    private void PlayerGotHit()
    {
        PlayerStats.CurrentHealth -= 10;
        OnPlayerHealthChanged?.Invoke();
        if (PlayerStats.CurrentHealth < 1)
        {
            PlayerNotDead = false;
            OnDeath?.Invoke();
        }
        else
        {
            audioSource.clip = PlayerGotHitSound;
            audioSource.Play();
        }
    }

    private void IncreaseCurrentHealthAndCallUpdateUI()
    {
        if (PlayerStats.MaximumHealth > PlayerStats.CurrentHealth)
        {
            PlayerStats.CurrentHealth += 10;
            OnPlayerHealthChanged?.Invoke();
        }
    }
}
