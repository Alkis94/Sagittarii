using UnityEngine;
using Factories;
using System;


public class PlayerFireProjectile : MonoBehaviour
{
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private GameObject arrowEmitter;
    [SerializeField]
    private float projectileSpeed = 1000;
    [SerializeField]
    private float projectileDestroyDelay = 30f;


    private Animator animator;
    private Vector3 arrowEmitterPosition;
    private PlayerStats playerStats;

    public Action<GameObject,GameObject,float,float,float,int> FireArrow = delegate {};
    

    private float attackHoldAnimationLength = 0.333f;
    private float attackHoldAnimationSpeed;
    private float timePassedHoldingArrow = 0f;
    private float arrowPower;


    void Start()
    {
        playerStats = GetComponentInParent<PlayerStats>();
        animator = GetComponent<Animator>();
        FireArrow = FireArrowSimple;
    }

    void Update()
    {
        if (playerStats.Ammo > 0)
        {
            if ((Input.GetButtonDown("Fire1") || Input.GetButton("Fire1")) && animator.GetCurrentAnimatorStateInfo(0).IsName("IdleHands"))
            {
                timePassedHoldingArrow = 0;
                animator.SetTrigger("PlayerAttackHold");
            }
            else if (Input.GetButton("Fire1") && animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttackHold"))
            {
                timePassedHoldingArrow += Time.deltaTime;
                attackHoldAnimationSpeed = animator.GetCurrentAnimatorStateInfo(0).speedMultiplier;

            }
            else if (Input.GetButtonUp("Fire1") && animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttackHold"))
            {
                arrowPower = timePassedHoldingArrow * attackHoldAnimationSpeed / attackHoldAnimationLength;
                arrowPower = arrowPower > 1 ? 1 : arrowPower;
                if (arrowPower > 0.01)
                {
                    animator.SetTrigger("PlayerAttackRelease");
                    FireArrow(arrowEmitter,projectile,projectileSpeed,projectileDestroyDelay,arrowPower,playerStats.damage);
                    playerStats.Ammo -= 1;
                }
                else
                {
                    animator.SetTrigger("PlayerAttackCanceled");
                }
            }
        }
    }

    private void FireArrowSimple(GameObject arrowEmitter, GameObject projectile, float projectileSpeed, float projectileDestroyDelay, float arrowPower , int damage)
    {
        ProjectileFactory.CreateProjectile(arrowEmitter.transform.position,  projectile, Vector3.zero, projectileSpeed * arrowPower, projectileDestroyDelay, damage, arrowEmitter.transform.rotation);
    }

}