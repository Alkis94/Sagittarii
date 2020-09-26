using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BossDoor : MonoBehaviour, IInteractable
{

    public static event Action<string> DoorEntered = delegate { };

    private MapType mapType;
    private BoxCollider2D boxCollider2D;
    private Animator animator;
    [SerializeField]
    private string levelToLoad;
    [SerializeField]
    private bool isOpen = false;

    [SerializeField]
    private bool isInside = false;

    [SerializeField]
    private bool changeDoorStateOnStart = false;

    private void OnEnable()
    {
        RoomFinish.OnRoomFinished += OpenDoor;
        MapManager.OnRoomLoaded += SetMapType;
    }

    private void OnDisable()
    {
        RoomFinish.OnRoomFinished -= OpenDoor;
        MapManager.OnRoomLoaded -= SetMapType;
    }

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("isOpen", isOpen);

        if (isInside)
        {
            if(ES3.FileExists("Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + mapType))
            {
                bool dead = ES3.Load<bool>("Dead0", "Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + mapType);
                if (!dead)
                {
                    ChangeDoorState();
                }
            }
            else
            {
                ChangeDoorState();
            }
        }

        if (changeDoorStateOnStart)
        {
            ChangeDoorState();
        }
    }

    public void Interact()
    {
        if (isOpen)
        {
            DoorEnter();
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
        if(!isOpen)
        {
            animator.SetTrigger("Open");
            animator.SetBool("isOpen", true);
            isOpen = true;
        }
    }

    private void SetMapType(MapType mapType,string roomKey,RoomType roomType)
    {
        this.mapType = mapType;
    }
}
