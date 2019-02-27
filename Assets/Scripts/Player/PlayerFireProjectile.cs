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
    public Action<GameObject,GameObject,float,float,float> FireArrow = delegate {};
    public static event Action OnPlayerFiredProjectile = delegate { };

    private float attackHoldAnimationLength = 0.333f;
    private float timePassedHoldingArrow = 0f;
    private float arrowPower;


    void Start()
    {
        animator = GetComponent<Animator>();
        FireArrow = FireArrowSimple;
    }

    void Update()
    {
        if (PlayerStats.ammo > 0)
        {
            if ((Input.GetButtonDown("Fire1") || Input.GetButton("Fire1")) && animator.GetCurrentAnimatorStateInfo(0).IsName("IdleHands"))
            {
                timePassedHoldingArrow = 0;
                animator.SetTrigger("PlayerAttackHold");
            }
            else if (Input.GetButton("Fire1") && animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttackHold"))
            {
                timePassedHoldingArrow += Time.deltaTime;
                arrowPower += Time.deltaTime;
            }
            else if (Input.GetButtonUp("Fire1") && animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttackHold"))
            {
                arrowPower = timePassedHoldingArrow / attackHoldAnimationLength;
                arrowPower = arrowPower > 1 ? 1 : arrowPower;
                if (arrowPower > 0.01)
                {
                    animator.SetTrigger("PlayerAttackRelease");
                    FireArrow(arrowEmitter,projectile,projectileSpeed,projectileDestroyDelay,arrowPower);
                    PlayerStats.ammo -= 1;
                    OnPlayerFiredProjectile.Invoke();
                }
                else
                {
                    animator.SetTrigger("PlayerAttackCanceled");
                }
            }
        }
    }

    private void FireArrowSimple(GameObject arrowEmitter, GameObject projectile, float projectileSpeed, float projectileDestroyDelay, float arrowPower)
    {
        ProjectileFactory.CreateProjectile(arrowEmitter.transform.position,  projectile, Vector3.zero, projectileSpeed * arrowPower, projectileDestroyDelay, arrowEmitter.transform.rotation);
    }

}