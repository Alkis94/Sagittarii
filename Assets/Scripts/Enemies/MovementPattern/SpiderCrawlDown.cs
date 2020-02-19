using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderCrawlDown : MonoBehaviour
{
    private EnemyLoader enemyLoader;
    private Animator animator;
    private Rigidbody2D rigidbody2d;
    
    [SerializeField]
    private Transform webString;
    private Transform webStringInstance;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        enemyLoader = GetComponent<EnemyLoader>();

        webStringInstance = Instantiate(webString, transform.position, Quaternion.identity);
        StartCoroutine(FallingMovement());

        enemyLoader.enemyLoaded += OnEnemyLoad;
    }

    private void OnDisable()
    {
        enemyLoader.enemyLoaded -= OnEnemyLoad;
    }


    IEnumerator FallingMovement()
    {
        float movingTime;
        movingTime = Random.Range(1, 3);
        movingTime += Time.time;
        while (movingTime > Time.time)
        {
            rigidbody2d.velocity = new Vector2(0, -2);
            yield return new WaitForFixedUpdate();
            webStringInstance.localScale += new Vector3(0, 0.8f, 0);
        }
        FinishedFalling();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Platform")
        {
            StopAllCoroutines();
            FinishedFalling();
        }
    }

    private void FinishedFalling()
    {
        rigidbody2d.velocity = new Vector2(0, 0);
        animator.SetTrigger("FinishedFalling");
    }

    private void OnEnemyLoad(bool dead)
    {
        if(dead)
        {
            StopAllCoroutines();
        }
        else 
        {
            float temp = webStringInstance.position.y - transform.position.y;
            temp = Mathf.Abs(temp);
            webStringInstance.localScale = new Vector3(webStringInstance.localScale.x, temp * 20, 1);
            StopAllCoroutines();
            FinishedFalling();
        }
    }
}
