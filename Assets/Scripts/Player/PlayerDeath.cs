using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Factories;

public class PlayerDeath : MonoBehaviour
{
    public AudioClip PlayerDiedSound;

    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        PlayerCollision.OnDeath+= Die;
    }


    void OnDisable()
    {
        PlayerCollision.OnDeath -= Die;
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
        MenuFactory.CallDefeatMenu();
    }

 
}
