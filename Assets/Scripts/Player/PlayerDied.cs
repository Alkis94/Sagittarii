using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDied : MonoBehaviour
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
        PlayerCollision.OnDeath+= PlayerDie;
    }


    void OnDisable()
    {
        PlayerCollision.OnDeath -= PlayerDie;
    }

    private void PlayerDie()
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
        Time.timeScale = 0;
        //GameHandler.Instance.CallPlayerDiedMenu();
    }

 
}
