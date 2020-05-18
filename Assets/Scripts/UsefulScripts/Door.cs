using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Door : MonoBehaviour
{

    public static event Action<string> DoorEntered = delegate { };

    private BoxCollider2D boxCollider2D;
    private Animator animator;
    [SerializeField]
    private string levelToLoad;
    [SerializeField]
    private bool isOpen = false;

    [SerializeField]
    private bool changeDoorStateOnStart = false;

    private void OnEnable()
    {
        RoomFinish.OnRoomFinished += OpenDoor;
    }

    private void OnDisable()
    {
        RoomFinish.OnRoomFinished -= OpenDoor;
    }

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("isOpen", isOpen);

        if(changeDoorStateOnStart)
        {
            ChangeDoorState();
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if(isOpen)
            {
                DoorEnter();
            }
        }
    }

    private void DoorEnter()
    {
        BoxCollider2D playerCollider;
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
        if(boxCollider2D.IsTouching(playerCollider))
        {
            DoorEntered?.Invoke(levelToLoad);
        }
    }

    private void ChangeDoorState()
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

    private void OpenDoor()
    {
        animator.SetTrigger("Open");
        animator.SetBool("isOpen", true);
        isOpen = true;
    }
}
