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

    private bool PlayerNotDead = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyProjectile" && PlayerNotDead)
        {
            PlayerStats.PlayerHealth -= 10;
            UIManager.Instance.UpdateHealth(PlayerStats.PlayerHealth, PlayerStats.PlayerMaximumHealth);
            if (PlayerStats.PlayerHealth < 1)
            {
                PlayerNotDead = false;
                if(OnDeath != null) OnDeath();
            }
            else
            {
                audioSource.clip = PlayerGotHitSound;
                audioSource.Play();
            }
        }

        if (other.tag == "HealthPickup" && PlayerStats.PlayerMaximumHealth > PlayerStats.PlayerHealth)
        {
            PlayerStats.PlayerHealth += 10;
            UIManager.Instance.UpdateHealth(PlayerStats.PlayerHealth, PlayerStats.PlayerMaximumHealth);
        }
        if (other.tag == "MaximumHealthPickup")
        {
            PlayerStats.PlayerHealth += 10;
            PlayerStats.PlayerMaximumHealth += 10;
            UIManager.Instance.UpdateHealth(PlayerStats.PlayerHealth, PlayerStats.PlayerMaximumHealth);
        }
        if (other.tag == "BatWingsPickup")
        {
            ItemHandler.PlayerHasBatWings = true;
            if (OnPlayerGotBatWings != null) OnPlayerGotBatWings();

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
            PlayerStats.PlayerDamage += 10;
        }

    }

}
