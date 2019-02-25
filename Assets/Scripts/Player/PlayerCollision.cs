using System;
using System.Collections.Generic;
using UnityEngine;
using Factories;

public class PlayerCollision : MonoBehaviour
{
    private AudioSource audioSource;
    private Animator animator;


    public AudioClip PlayerGotHitSound;
    public AudioClip PlayerDiedSound;


    public static event Action OnDeath = delegate { };
    public static event Action OnPlayerGotBatWings = delegate { };
    public static event Action OnPlayerGotTrident = delegate { };
    

    private bool PlayerNotDead = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyProjectile" && PlayerNotDead)
        {
            PlayerGotHit();
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

    private void PlayerGotHit()
    {
        PlayerStats.ChangePlayerCurrentHealth(PlayerStats.CurrentHealth - 10);

        if (PlayerStats.CurrentHealth < 1)
        {
            PlayerNotDead = false;
            Die();
            OnDeath?.Invoke();
        }
        else
        {
            audioSource.clip = PlayerGotHitSound;
            audioSource.Play();
        }
    }

    private void Die()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        audioSource.clip = PlayerDiedSound;
        audioSource.Play();
        animator.SetTrigger("PlayerDied");
        Invoke("PlayerDiedDelayedMenu", 3);
    }

    private void PlayerDiedDelayedMenu()
    {
        MenuFactory.CreateMenuAndPause(MenuFactory.DefeatMenu);
    }

}
