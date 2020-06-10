using UnityEngine;
using System.Collections;

public class ItemShopkeeper : MonoBehaviour
{

    private BoxCollider2D boxCollider2D;
    private Animator animator;
    private float waveDelay = 0;


    // Use this for initialization
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(AnimateKeeper());
    }

    IEnumerator AnimateKeeper()
    {
        while (true)
        {
            float randomDuration = Random.Range(5, 20);
            yield return new WaitForSeconds(randomDuration);
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("ShopkeeperStatic"))
            {
                animator.SetTrigger("Idle");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && waveDelay < Time.time)
        {
            animator.SetTrigger("Wave");
            waveDelay = Time.time + 15f;
        }
    }
}
