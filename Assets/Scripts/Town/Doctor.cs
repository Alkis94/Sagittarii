using UnityEngine;
using System.Collections;

public class Doctor : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    private Animator animator;
    private float waveDelay = 0;

    // Use this for initialization
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && waveDelay < Time.time)
        {
            animator.SetTrigger("Wave");
            waveDelay = Time.time + 30f;
        }
    }
}
