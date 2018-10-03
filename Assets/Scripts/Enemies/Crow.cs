using UnityEngine;
using System.Collections;

public class Crow : Enemy
{

    // Use this for initialization
    void Start ()
    {
        Health = C.CROW_HEALTH;
        Speed = C.CROW_SPEED;
        EnemyRB2D = GetComponent<Rigidbody2D>();
        EnemyAnimator = GetComponent<Animator>();
        EnemyRenderer = GetComponent<SpriteRenderer>();
        EnemyBoxCollider = GetComponent<BoxCollider2D>();
        InvokeRepeating("CrowAttack", C.CROW_ATTACK_FREQUENCY, C.CROW_ATTACK_FREQUENCY);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Health > 1)
        {
            EnemyMove();
            EnemyChangeDirection();
        }
    }


    private void CrowAttack()
    {
        ObjectFactory.Instance.CreateProjectile<CrowProjectile>(transform,0,0,ObjectFactory.Instance.CrowProjectilePrefab);
        ObjectFactory.Instance.CreateProjectile<CrowProjectile>(transform, 1, 0, ObjectFactory.Instance.CrowProjectilePrefab);
        ObjectFactory.Instance.CreateProjectile<CrowProjectile>(transform, -1, 0, ObjectFactory.Instance.CrowProjectilePrefab);
    }

    protected override void EnemyDie()
    {
        float randomNumber = Random.Range(0f, 1f);
        if (randomNumber < C.DEAD_BIRD_DROP_RATE && ItemHandler.Instance.DeadBirdNotDropped)
        {
            ObjectFactory.Instance.CreatePickup(transform,ObjectFactory.Instance.DeadBirdPickupPrefab);
            ItemHandler.Instance.DeadBirdNotDropped = false;
        }
        base.EnemyDie();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Arrow")
        {
            Health -= 10 * ItemHandler.Instance.ImpFlameMultiplier;
            if (Health < 1)
            {
                EnemyDie();
            }
        }
    }
}
