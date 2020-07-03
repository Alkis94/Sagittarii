using UnityEngine;
using System.Collections;

public class TreasureChest : MonoBehaviour, IInteractable
{

    private Animator animator;
    private bool isClosed = true;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        if(isClosed)
        {
            animator.SetTrigger("Open");
            PickUpFactory.Instance.DropGold(transform.position, 1, 35, 65);
            isClosed = false;
        }
    }
}
