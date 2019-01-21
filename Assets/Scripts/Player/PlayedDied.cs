using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayedDied : MonoBehaviour
{

    private Animator animator;
    public AudioSource PlayerDiedSound;


    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
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
        PlayerDiedSound.Play();
        animator.SetTrigger("PlayerDied");
        Invoke("PlayerDiedDelayedMenu", 3);

    }

    private void PlayerDiedDelayedMenu()
    {
        Time.timeScale = 0;
        GameHandler.Instance.CallPlayerDiedMenu();
    }

 
}
