using UnityEngine;
using System.Collections;

public class Medusa : Enemy
{
    public GameObject MedusaProjectile;
    private bool MedusaAngry;
    private bool MedusaVeryAngry;

    // Use this for initialization
    void Start ()
    {
        MedusaAngry = false;
        MedusaVeryAngry = false;
        Health = C.MEDUSA_HEALTH;
        Speed = C.MEDUSA_SPEED;
        AttackFrequency = C.MEDUSA_ATTACK_FREQUENCY;
        EnemyRB2D = GetComponent<Rigidbody2D>();
        EnemyAnimator = GetComponent<Animator>();
        EnemyRenderer = GetComponent<SpriteRenderer>();
        EnemyBoxCollider = GetComponent<BoxCollider2D>();
        Invoke("MedusaAttack", AttackFrequency);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Health > C.MEDUSA_VERY_ANGRY_HEALTH-10)
        {
            EnemyMove();
        }
        if(Health > 1)
        {
            EnemyChangeDirection();
        }
    }

    private void MedusaAttack()
    {
        Instantiate(MedusaProjectile, gameObject.transform.position, Quaternion.identity);
        Invoke("MedusaAttack", AttackFrequency);
    }

    private void MedusaFrenzyAttack()
    {
        //ObjectFactory.Instance.CreateProjectile<MedusaProjectile>(transform, 0,0,ObjectFactory.Instance.MedusaProjectilePrefab);
        //ObjectFactory.Instance.CreateProjectile<MedusaProjectile>(transform, 1, 0, ObjectFactory.Instance.MedusaProjectilePrefab);
        //ObjectFactory.Instance.CreateProjectile<MedusaProjectile>(transform, -1, 0, ObjectFactory.Instance.MedusaProjectilePrefab);
        //ObjectFactory.Instance.CreateProjectile<CrowProjectile>(transform, 0.5f, 0, ObjectFactory.Instance.CrowProjectilePrefab);
        //ObjectFactory.Instance.CreateProjectile<CrowProjectile>(transform, -0.5f, 0, ObjectFactory.Instance.CrowProjectilePrefab);
        Invoke("MedusaFrenzyAttack", AttackFrequency);
    }

    private void MedusaDeathAttack()
    {
        //ObjectFactory.Instance.CreateProjectile<MedusaProjectile>(transform, 0,0, ObjectFactory.Instance.MedusaProjectilePrefab);
        //ObjectFactory.Instance.CreateProjectile<MedusaProjectile>(transform, 0.5f, 0, ObjectFactory.Instance.MedusaProjectilePrefab);
        //ObjectFactory.Instance.CreateProjectile<MedusaProjectile>(transform, -0.5f, 0, ObjectFactory.Instance.MedusaProjectilePrefab);
        //ObjectFactory.Instance.CreateProjectile<MedusaProjectile>(transform, 1, 0, ObjectFactory.Instance.MedusaProjectilePrefab);
        //ObjectFactory.Instance.CreateProjectile<MedusaProjectile>(transform, -1, 0, ObjectFactory.Instance.MedusaProjectilePrefab);
        //ObjectFactory.Instance.CreateProjectile<MedusaProjectile>(transform, 1.5f, 0, ObjectFactory.Instance.MedusaProjectilePrefab);
        //ObjectFactory.Instance.CreateProjectile<MedusaProjectile>(transform, -1.5f, 0, ObjectFactory.Instance.MedusaProjectilePrefab);
        //ObjectFactory.Instance.CreateProjectile<MedusaProjectile>(transform, -2, 0, ObjectFactory.Instance.MedusaProjectilePrefab);
        //ObjectFactory.Instance.CreateProjectile<MedusaProjectile>(transform, 2, 0, ObjectFactory.Instance.MedusaProjectilePrefab);
    }

    protected override void EnemyDie()
    {
        ObjectFactory.Instance.CreatePickup(transform,ObjectFactory.Instance.MaximumHealthPickupPrefab);
        MedusaDeathAttack();
        EnemyBoxCollider.size = new Vector3(0.4f, 0.6f, 0);
        base.EnemyDie();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Arrow")
        {
            Health -= PlayerStats.PlayerDamage;
            if(Health < C.MEDUSA_ANGRY_HEALTH && !MedusaAngry)
            {
                MedusaAngry = true;
                Speed *= 2;
                AttackFrequency *= 0.5f;
                EnemyAnimator.SetTrigger("Run");
            }
            if (Health < C.MEDUSA_VERY_ANGRY_HEALTH && !MedusaVeryAngry)
            {
                CancelInvoke();
                Invoke("MedusaFrenzyAttack", 0.5f);
                AttackFrequency *= 0.25f;
                MedusaVeryAngry = true;
                EnemyAnimator.SetTrigger("Turn");
            }
            if (Health < 1)
            {
                EnemyDie();
            }
        }
    }
}
