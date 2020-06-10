using UnityEngine;
using System.Collections;

public class AmmoShopkeeper : MonoBehaviour
{

    private BoxCollider2D boxCollider2D;
    private Animator animator;


    // Use this for initialization
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(AnimateKeeper());
    }

    IEnumerator AnimateKeeper()
    {
        while(true)
        {
            float randomDuration = Random.Range(5, 10);
            yield return new WaitForSeconds(randomDuration);
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("ShopkeeperStatic"))
            {
                float randomNumber = Random.Range(0f, 1f);
                if(randomNumber < 0.7f)
                {
                    animator.SetTrigger("Idle1");
                }
                else
                {
                    animator.SetTrigger("Idle2");
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            animator.SetTrigger("Wave");
        }
    }
}
