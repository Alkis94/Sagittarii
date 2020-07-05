using UnityEngine;

public class TreasureChest : MonoBehaviour, IInteractable
{
    private Animator animator;
    private MapType mapType;
    private RoomType roomType;
    private string roomKey;
    private bool isEnabled = false;
    private bool isClosed = true;

    private void OnEnable()
    {
        MapManager.OnRoomLoaded += GetInfo;
    }

    private void OnDisable()
    {
        MapManager.OnRoomLoaded -= GetInfo;
    }

    private void Start()
    {
        if(isEnabled)
        {
            animator = GetComponentInChildren<Animator>();
            if (ES3.KeyExists("isClosed", "Levels/" + mapType + "/Room" + roomKey))
            {
                isClosed = ES3.Load<bool>("isClosed", "Levels/" + mapType + "/Room" + roomKey);
            }

            if (!isClosed)
            {
                animator.SetTrigger("SetOpened");
            }
        }
    }

    public void EnableChest()
    {
        isEnabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void Interact()
    {
        if(isClosed && isEnabled)
        {
            animator.SetTrigger("Open");
            PickUpFactory.Instance.DropGold(transform.position, 1, 35, 65);
            isClosed = false;
            ES3.Save<bool>("isClosed", isClosed, "Levels/" + mapType + "/Room" + roomKey);
        }
    }

    private void GetInfo(MapType mapType, string roomKey, RoomType roomType)
    {
        this.mapType = mapType;
        this.roomKey = roomKey;
        this.roomType = roomType;
    }


}
