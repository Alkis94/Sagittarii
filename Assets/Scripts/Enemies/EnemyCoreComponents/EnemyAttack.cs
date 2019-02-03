using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAttack : MonoBehaviour
{

    private EnemyData enemyData;
    private EnemyCollision enemyCollision;
    private AttackPatern attackPatern;
    

    private void OnEnable()
    {
        enemyCollision = GetComponent<EnemyCollision>();
        enemyCollision.OnDeath += CancelInvoke;
    }

    private void OnDisable()
    {
        enemyCollision.OnDeath -= CancelInvoke;
    }

    private void Start()
    {
        enemyData = GetComponent<EnemyData>();
        attackPatern = GetComponent<AttackPatern>();
        InvokeRepeating("Attack", enemyData.DelayBeforeFirstAttack, enemyData.AttackFrequency);
    }

    private void Attack()
    {
        attackPatern.Attack(enemyData);
    }

}
