using UnityEngine;
using System.Collections;
using Factories;

public class PlayerDeath : MonoBehaviour
{
    private PlayerAudio playerAudio;
    private Animator animator;
    private PlayerStats playerStats;

    void OnEnable()
    {
        PlayerStats.PlayerDied += Die;
    }

    void OnDisable()
    {
        PlayerStats.PlayerDied -= Die;
    }

    private void Start()
    {
        playerAudio = GetComponent<PlayerAudio>();
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
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
        gameObject.layer = 14;
        enabled = false;

        if (ES3.FileExists("Saves/Profile" + SaveProfile.SaveID))
        {
            ES3.DeleteFile("Saves/Profile" + SaveProfile.SaveID);
        }
    }

    private void PlayerDiedDelayedMenu()
    {
        MenuFactory.CreateMenuAndPause(MenuFactory.DefeatMenuDev);
    }
}
