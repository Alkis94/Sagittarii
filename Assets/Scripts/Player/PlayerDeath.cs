using UnityEngine;
using Factories;

public class PlayerDeath : MonoBehaviour
{
    private PlayerAudio playerAudio;
    private Animator animator;
    private PlayerStats playerStats;

    void OnEnable()
    {
        PlayerStats.OnPlayerDied += Die;
    }

    void OnDisable()
    {
        PlayerStats.OnPlayerDied -= Die;
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

        if (ES3.DirectoryExists("Saves/Profile" + SaveProfile.SaveID))
        {
            ES3.DeleteDirectory("Saves/Profile" + SaveProfile.SaveID);
        }

        if (ES3.DirectoryExists(("Levels/")))
        {
            ES3.DeleteDirectory("Levels/");
            ES3.DeleteDirectory("Levels/");
        }
    }

    private void PlayerDiedDelayedMenu()
    {
        MenuFactory.CreateMenuAndPause(MenuFactory.DefeatMenu);
    }
}
