using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileImpactAnimator : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        animator.SetTrigger("Impact");
    }

}
