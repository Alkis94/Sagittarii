using UnityEngine;

public class BossDoorInside : BossDoor, IInteractable
{
    private void OnEnable()
    {
        RoomManager.OnRoomFinished += OpenDoor;
        MapManager.OnRoomLoaded += SetMapType;
    }

    private void OnDisable()
    {
        RoomManager.OnRoomFinished -= OpenDoor;
        MapManager.OnRoomLoaded -= SetMapType;
    }

    protected override void Start()
    {
        base.Start();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
 
        if (!ES3.FileExists("Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + mapType))
        {
            ChangeDoorState();
        }
    }

    public void Interact()
    {
        if (isOpen)
        {
            DoorEnter("LastRoom");
        }
    }

    private void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            animator.SetTrigger("Open");
            animator.SetBool("isOpen", true);
        }
    }
}
