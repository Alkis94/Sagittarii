using UnityEngine;
using System;
using System.Collections;

public class PlayerDeath : MonoBehaviour
{
    public static event Action OnPlayerRessurected = delegate { };

    private PlayerAudio playerAudio;
    private Animator animator;
    private PlayerStats playerStats;
    private GameObject bloodSplat;

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
        bloodSplat = Resources.Load("DeathBloodSplat") as GameObject;
    }

    private void Die(DamageSource damageSource)
    {
        StartCoroutine(DieCoroutine(damageSource));
    }

    private IEnumerator DieCoroutine(DamageSource damageSource)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
       
        if(damageSource == DamageSource.projectile || damageSource == DamageSource.exhaustion)
        {
            animator.SetTrigger("Die");
            playerAudio.PlayDeathSound();
        }
        else
        {
            animator.SetTrigger("DieSplatter");
            playerAudio.PlaySplatterDeathSound();
        }

        Instantiate(bloodSplat, transform.position, Quaternion.identity);
        gameObject.layer = 14;
        enabled = false;

        if (playerStats.ExtraLives > 0)
        {
            yield return new WaitForSeconds(3f);
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            gameObject.layer = 10;
            enabled = true;
            playerStats.ExtraLives--;
            GetComponent<PlayerInput>().enabled = true;
            OnPlayerRessurected?.Invoke();
            SceneFader.Instance.LoadSceneWithFade("Town");
            yield break;
        }

        yield return new WaitForSeconds(3f);

        UIManager.Instance.CallDeathUI();
        

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
}
