using UnityEngine;
using System;

public class BossHealth : MonoBehaviour
{

    private EnemyStats EnemyStats;
    private int previousHealth;
    
    private void OnEnable()
    {
        EnemyStats = GetComponent<EnemyStats>();
        EnemyStats.EnemyHealthChanged += OnBossHealthChanged;
    }

    private void OnDisable()
    {
        EnemyStats.EnemyHealthChanged -= OnBossHealthChanged;
    }

    // Use this for initialization
    void Start()
    {
        previousHealth = EnemyStats.Health;
        UIManager.Instance.EnableBossHealth(EnemyStats.Health);
    }

    private void OnBossHealthChanged()
    {
        int damage = previousHealth - EnemyStats.Health;
        UIManager.Instance.UpdateBossHealth(damage);
        previousHealth = EnemyStats.Health;
    }
}
