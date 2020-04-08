using UnityEngine;
using System;

public class BossHealth : MonoBehaviour
{

    private EnemyData enemyData;
    private int previousHealth;

    public static event Action<int> BossEngaged = delegate { };
    public static event Action<int> BossDamaged = delegate { };
    
    private void OnEnable()
    {
        enemyData = GetComponent<EnemyData>();
        enemyData.EnemyHealthChanged += OnBossHealthChanged;
    }

    private void OnDisable()
    {
        enemyData.EnemyHealthChanged -= OnBossHealthChanged;
    }

    // Use this for initialization
    void Start()
    {
        previousHealth = enemyData.Health;
        BossEngaged?.Invoke(enemyData.Health);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnBossHealthChanged()
    {
        int damage = previousHealth - enemyData.Health;
        BossDamaged?.Invoke(damage);
        previousHealth = enemyData.Health;
    }
}
