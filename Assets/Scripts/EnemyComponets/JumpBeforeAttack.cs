using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBeforeAttack : MonoBehaviour
{
    //In order to use this component you must have AttackFrequency = DelayBeforeFirstAttack

    public AudioSource attackSound;


    private Animator animator;
    private Rigidbody2D rigidbody2d;

    private EnemyCollision enemyCollision;
    private EnemyMovement enemyMovement;
    private EnemyData enemyData;

    public float ShotDelay;
    public float HorizontalJumpPower;
    public float VerticalJumpPower;

    private void OnEnable()
    {
        enemyCollision = GetComponent<EnemyCollision>();
        enemyCollision.OnDeath += StopAllCoroutines;
    }

    private void OnDisable()
    {
        enemyCollision.OnDeath -= StopAllCoroutines;
    }
    void Start()
    {

        enemyMovement = GetComponent<EnemyMovement>();
        enemyData = GetComponent<EnemyData>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(Jump());
    }

    private IEnumerator Jump()
    {
        while (true)
        {
            yield return new WaitForSeconds(enemyData.AttackFrequency - ShotDelay);
            animator.SetTrigger("Attack");
            rigidbody2d.AddForce(new Vector2(HorizontalJumpPower * enemyMovement.HorizontalDirection, VerticalJumpPower), ForceMode2D.Impulse);
            yield return new WaitForSeconds(ShotDelay);
            attackSound.Play();

        }
    }

    private void StopCorouritine()
    {
        StopAllCoroutines();
    }
}
