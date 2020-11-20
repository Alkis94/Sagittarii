using UnityEngine;
using System;

public class BossHealth : MonoBehaviour
{

    private EnemyStats enemyStats;
    private int previousHealth;
    
    private void OnEnable()
    {
        enemyStats = GetComponent<EnemyStats>();
        enemyStats.EnemyHealthChanged += OnBossHealthChanged;
    }

    private void OnDisable()
    {
        enemyStats.EnemyHealthChanged -= OnBossHealthChanged;
    }

    // Use this for initialization
    void Start()
    {
        previousHealth = enemyStats.Health;
        UIManager.Instance.EnableBossHealth(enemyStats.Health);
    }

    private void OnBossHealthChanged()
    {
        int damage = previousHealth - enemyStats.Health;
        UIManager.Instance.UpdateBossHealth(damage);
        previousHealth = enemyStats.Health;
    }
}
