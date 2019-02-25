using UnityEngine;
using Factories;
using System;


public class PlayerFireProjectile : MonoBehaviour
{
    [SerializeField]
    private GameObject Projectile;
    [SerializeField]
    private GameObject ArrowEmitter;
    [SerializeField]
    private float ProjectileSpeed;
    [SerializeField]
    private float ProjectileDestroyDelay = 30f;


    private Animator animator;
    private Vector3 ArrowEmitterPosition;
    public Action<GameObject,GameObject,float,float,float> FireArrow = delegate {};
    public static event Action OnPlayerFiredProjectile = delegate { };

    private float AttackHoldAnimationLength = 0.333f;
    private float TimePassedHoldingArrow = 0f;
    private float ArrowPower;


    void Start()
    {
        animator = GetComponent<Animator>();
        FireArrow = FireArrowSimple;
    }

    void Update()
    {
        if (PlayerStats.Ammo > 0)
        {
            if ((Input.GetButtonDown("Fire1") || Input.GetButton("Fire1")) && animator.GetCurrentAnimatorStateInfo(0).IsName("IdleHands"))
            {
                TimePassedHoldingArrow = 0;
                animator.SetTrigger("PlayerAttackHold");
            }
            else if (Input.GetButton("Fire1") && animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttackHold"))
            {
                TimePassedHoldingArrow += Time.deltaTime;
                ArrowPower += Time.deltaTime;
            }
            else if (Input.GetButtonUp("Fire1") && animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttackHold"))
            {
                ArrowPower = TimePassedHoldingArrow / AttackHoldAnimationLength;
                ArrowPower = ArrowPower > 1 ? 1 : ArrowPower;
                if (ArrowPower > 0.01)
                {
                    animator.SetTrigger("PlayerAttackRelease");
                    FireArrow(ArrowEmitter,Projectile,ProjectileSpeed,ProjectileDestroyDelay,ArrowPower);
                    PlayerStats.Ammo -= 1;
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
        ProjectileFactory.CreateProjectile(ArrowEmitter.transform.position,  Projectile, Vector3.zero, ProjectileSpeed*ArrowPower, ProjectileDestroyDelay, ArrowEmitter.transform.rotation);
    }

}