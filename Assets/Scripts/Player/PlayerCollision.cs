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
            PlayerGotHit();
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
            OnPlayerGotDeadBird?.Invoke();
        }
        if (other.tag == "ImpFlamePickup")
        {
            ItemHandler.PlayerHasImpFlame = true;
            PlayerStats.Damage += 10;
        }

    }


    private void PlayerGotHit()
    {

        PlayerStats.ChangePlayerCurrentHealth(PlayerStats.CurrentHealth - 10);

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

}
