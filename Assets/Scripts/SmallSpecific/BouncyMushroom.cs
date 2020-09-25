using UnityEngine;

public class BouncyMushroom : MonoBehaviour
{
    private bool playerIsTouching = false;
    private Animator animator;
    private AudioSource audioSource;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Jump") && playerIsTouching)
        {
            animator.SetTrigger("Bounce");
            audioSource.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerIsTouching = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerIsTouching = false;
        }
    }
}
