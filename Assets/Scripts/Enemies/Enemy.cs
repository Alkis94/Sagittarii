using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    protected int HorizontalDirection;
    protected float VerticalDirection;
    protected int Health;
    protected int Speed;
    protected float AttackFrequency;
    private float randomNumber;

    public AudioSource DeathCry;
    protected SpriteRenderer EnemyRenderer;
    protected Animator EnemyAnimator;
    protected Rigidbody2D EnemyRB2D;
    protected BoxCollider2D EnemyBoxCollider;

    // Use this for initialization

    void Awake()
    {
        randomNumber = Random.Range(0f, 2f);
        HorizontalDirection = randomNumber > 1 ? 1 : -1;
        VerticalDirection = 0;
    }

    protected virtual void EnemyDie()
    {
        ObjectFactory.Instance.CreateDeathBloodSplat(transform);
        EnemyAnimator.SetTrigger("Die");
        CancelInvoke();
        EnemyRB2D.isKinematic = false;
        EnemyBoxCollider.isTrigger = false;
        EnemyRenderer.sortingLayerName = "DeadEnemies";
        DestroyObject(gameObject, 10.0f);
        UIManager.Instance.UpdateScore();
        gameObject.layer = 14;
        DeathCry.Play();
        transform.gameObject.tag = "DeadEnemy";

        randomNumber = Random.Range(0f, 1f);
        if (randomNumber < C.HEALTH_PICKUP_DROP_RATE)
        {
            ObjectFactory.Instance.CreatePickup(transform,ObjectFactory.Instance.HealthPickupPrefab);
        }
    }

    protected void EnemyFlipSprite ()
    {
        if (HorizontalDirection > 0)
        {
            EnemyRenderer.flipX = true;
        }
        else if (HorizontalDirection < 0)
        {
            EnemyRenderer.flipX = false;
        }
    }


     protected virtual void EnemyMove()
    {
        EnemyFlipSprite();
        Vector3 Movement = new Vector3(HorizontalDirection, VerticalDirection, 0);
        transform.Translate(Movement * Time.deltaTime * Speed);
    }


    protected void EnemyChangeDirection()
    {
        if(transform.position.x > C.ENEMY_BOUNDARY)
        {
            HorizontalDirection = -1;
        }

        if (transform.position.x < -C.ENEMY_BOUNDARY)
        {
            HorizontalDirection = 1;
        }
    }



}
