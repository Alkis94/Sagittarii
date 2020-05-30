using UnityEngine;
using System.Collections;

public abstract class EnemyLoader : MonoBehaviour
{
    public int EnemyKey { get; set; }
    public string RoomKey { get; set; }
    public MapType MapType { get; set; }

    [SerializeField]
    protected Sprite deadEnemySprite;
    [SerializeField]
    protected Sprite criticalDeathEnemySprite;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rigidbody2d;
    protected EnemyGotShot enemyGotShot;

    protected bool dead = false;
    protected bool criticalDeath = false;


    public abstract void Load();
    public abstract void ChangeEnemyStatusToDead(bool criticalDeath);

    public virtual bool IsDead()
    {
        if (dead)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            rigidbody2d = GetComponent<Rigidbody2D>();
            enemyGotShot = GetComponent<EnemyGotShot>();
            animator.enabled = false;
            if (criticalDeath)
            {
                spriteRenderer.sprite = criticalDeathEnemySprite;
            }
            else
            {
                spriteRenderer.sprite = deadEnemySprite;
            }

            GetComponent<EnemyBrain>().enabled = false;
            rigidbody2d.gravityScale = 1;
            spriteRenderer.sortingLayerName = "DeadEnemies";
            gameObject.layer = 14;
            enemyGotShot.enabled = false;
            foreach (Transform trans in GetComponentsInChildren<Transform>(true))
            {
                trans.gameObject.layer = 14;
            }

            transform.parent = null;
            return true;
        }
        return false;
    }
}
