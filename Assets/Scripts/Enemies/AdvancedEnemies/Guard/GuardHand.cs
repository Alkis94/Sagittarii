using UnityEngine;
using System.Collections;

public class GuardHand : MonoBehaviour
{
    private Transform player;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private EnemyStats enemyStats;
    private EnemyAttackHandler enemyAttackHandler;
    private AudioSource audioSource;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyStats = GetComponentInParent<EnemyStats>();
        enemyAttackHandler = GetComponentInParent<EnemyAttackHandler>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (GameManager.GameState == GameStateEnum.unpaused)
        {
            HandFollowCursor();

            if (transform.position.x < player.position.x)
            {
                spriteRenderer.flipY = false;

            }
            else if (transform.position.x > player.position.x)
            {
                spriteRenderer.flipY = true;
            }
        }
    }

    protected void CallAttackFromAnimation()
    {
        audioSource.Play();
        enemyAttackHandler.Attack(enemyStats.AttackData[0]);
    }

    private void HandFollowCursor()
    {
        Vector2 Direction = (((Vector2)player.position - (Vector2)transform.position)).normalized;
        transform.right = Direction;
    }
}
