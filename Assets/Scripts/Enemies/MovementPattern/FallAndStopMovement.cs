using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallAndStopMovement : MovementPattern
{
    private Animator animator;
    private Rigidbody2D rigidbody2d;
    private float movingTime;
    private BoxCollider2D boxCollider2D;
    [SerializeField]
    private Transform web;
    

    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        movingTime = Random.Range(1, 3);
        movingTime += Time.time;
        StartCoroutine(FallingMovement());
    }

    public override void Move(float speed, int verticalDirection)
    {
        
    }

    IEnumerator FallingMovement()
    {
        while (movingTime > Time.time)
        {
            rigidbody2d.velocity = new Vector2(0, -2);
            yield return new WaitForFixedUpdate();
            web.localScale += new Vector3(0, 0.9f, 0);
        }
        FinishedFalling();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            StopAllCoroutines();
            FinishedFalling();
        }
    }

    private void FinishedFalling()
    {
        rigidbody2d.velocity = new Vector2(0, 0);
        animator.SetTrigger("FinishedFalling");
    }
}
