using UnityEngine;

public class MushroomKingdom : MonoBehaviour
{
    private Animator animator;
    private int enemiesDiedCounter = 0;

    private void OnEnable()
    {
        EnemyStats.OnEnemyWasKilled += CountEemiesTillChange;
    }

    private void OnDisable()
    {
        EnemyStats.OnEnemyWasKilled -= CountEemiesTillChange;
    }

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void CountEemiesTillChange(DamageSource damageSource)
    {
        enemiesDiedCounter++;
        if (enemiesDiedCounter > 15)
        {
            animator.SetTrigger("Die");
        }
    }

}
