using UnityEngine;
using System.Collections;


public class BirdBrain : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        StartCoroutine(Fly());
    }

    IEnumerator Fly()
    {
        while(true)
        {
            animator.SetBool("Flying", false);
            rigidbody2d.velocity = new Vector2(4f * transform.right.x, -1f);
            float randomDuration = Random.Range(4, 8);
            yield return new WaitForSeconds(randomDuration / 4);
            animator.SetBool("Flying", true);
            rigidbody2d.velocity = new Vector2(4f * transform.right.x, 0.25f);
            yield return new WaitForSeconds(randomDuration);
        }
    }


}
