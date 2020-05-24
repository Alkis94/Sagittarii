using UnityEngine;
using System;

public class BossHealth : MonoBehaviour
{

    private EnemyStats EnemyStats;
    private int previousHealth;

    public static event Action<int> BossEngaged = delegate { };
    public static event Action<int> BossDamaged = delegate { };
    
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
        BossEngaged?.Invoke(EnemyStats.Health);
    }

    private void OnBossHealthChanged()
    {
        int damage = previousHealth - EnemyStats.Health;
        BossDamaged?.Invoke(damage);
        previousHealth = EnemyStats.Health;
    }
}
