using System;
using UnityEngine;

public class BossDoor : MonoBehaviour
{

    public static event Action<string> DoorEntered = delegate { };

    protected MapType mapType;
    protected BoxCollider2D boxCollider2D;
    protected Animator animator;
    protected bool isOpen = true;

    protected virtual void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    protected void DoorEnter(string levelToLoad)
    {
        BoxCollider2D playerCollider;
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
        if(boxCollider2D.IsTouching(playerCollider))
        {
            DoorEntered?.Invoke(levelToLoad);
        }
    }

    protected void ChangeDoorState()
    {
        if (isOpen)
        {
            animator.SetTrigger("Close");
            animator.SetBool("isOpen", false);
        }
        else
        {
            animator.SetTrigger("Open");
            animator.SetBool("isOpen", true);
        }
        isOpen = !isOpen;
    }

    protected void SetMapType(MapType mapType,string roomKey,RoomType roomType)
    {
        this.mapType = mapType;
    }
}
