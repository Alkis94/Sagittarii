using UnityEngine;
using UnityEngine.SceneManagement;

public class BearJaw : MonoBehaviour
{
    private PlayerStats playerStats;
    private int damagedAdded = 0;
    private int killCounter = 0;

    private void OnEnable()
    {
        playerStats = GetComponentInParent<PlayerStats>();
        EnemyStats.OnEnemyWasKilled += AddDamgeEveryTen;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        EnemyStats.OnEnemyWasKilled -= AddDamgeEveryTen;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        ResetDefaults();
    }

    private void AddDamgeEveryTen(DamageSource damageSource)
    {
        if (damageSource == DamageSource.projectile)
        {
            killCounter++;
            if(killCounter >= 30)
            {
                damagedAdded++;
                playerStats.Damage += 1;
                killCounter = 0;
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Town")
        {
            ResetDefaults();
        }
    }

    private void ResetDefaults()
    {
        playerStats.Damage -= damagedAdded;
        damagedAdded = 0;
        killCounter = 0;
    }
}
