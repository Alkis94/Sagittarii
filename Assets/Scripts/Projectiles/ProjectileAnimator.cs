using UnityEngine;

public class ProjectileAnimator : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private bool hasImpactAnimation = false;
    [SerializeField]
    private bool hasTravelAnimation = false;

    private void Start()
    {
        animator = GetComponent<Animator>();

        if(hasTravelAnimation)
        {
            animator.SetTrigger("Travel");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(hasImpactAnimation)
        {
            animator.SetTrigger("Impact");
        }
    }

}
