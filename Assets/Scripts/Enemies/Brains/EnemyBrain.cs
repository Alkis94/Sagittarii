using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  EnemyBrain : MonoBehaviour
{
    [HideInInspector]
    public EnemyData enemyData;
    protected EnemyGotShot enemyGotShot;
    protected SpriteRenderer spriteRenderer;

    //This timer will help enemies that get stuck somewhere not to change directions too rapidly
    protected float cannotChangeDirectionTime = 0f;

    protected float speed;

    protected virtual void OnEnable()
    {
        enemyGotShot.OnDeath += OnEnemyDiedStopAll;
    }

    protected virtual void OnDisable()
    {
        CancelInvoke();
        StopAllCoroutines();
        enemyGotShot.OnDeath -= OnEnemyDiedStopAll;
    }

    protected virtual void Awake()
    {
        enemyGotShot = GetComponent<EnemyGotShot>();
        enemyData = GetComponent<EnemyData>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        if (enemyData.changingDirections)
        {
            StartCoroutine(ChangingDirectionsOverTime(enemyData.changeDirectionFrequency));
        }

        StartFacingRandomDirection();
    }

    protected abstract void ChangeHorizontalDirection();

    public IEnumerator ChangingDirectionsOverTime(float changeDirectionFrequency)
    {
        while(true)
        {
            ChangeHorizontalDirection();
            yield return new WaitForSeconds(changeDirectionFrequency);
        }
    }


    protected void StartFacingRandomDirection()
    {
        float random = Random.Range(0f, 1f);
        if (random < 0.5f)
        {
            ChangeHorizontalDirection();
        }
    }

    protected void OnEnemyDiedStopAll(bool criticalDeath)
    {
        CancelInvoke();
        StopAllCoroutines();
    }
}
