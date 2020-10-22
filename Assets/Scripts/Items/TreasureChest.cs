using UnityEngine;

public class TreasureChest : MonoBehaviour, IInteractable
{
    [SerializeField]
    private int minGoldDrop = 0;
    [SerializeField]
    private int maxGoldDrop = 100;
    private Animator animator;
    private AudioSource audioSource;
    private MapType mapType;
    private RoomType roomType;
    private string roomKey;
    private bool isEnabled = false;
    private bool isClosed = true;

    private void OnEnable()
    {
        MapManager.OnRoomLoaded += GetInfo;
        isEnabled = transform.GetChild(0).gameObject.activeInHierarchy;
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
            audioSource = GetComponent<AudioSource>();
            if (ES3.KeyExists("isClosed", "Levels/" + mapType + "/Room" + roomKey + "/Props"))
            {
                isClosed = ES3.Load<bool>("isClosed", "Levels/" + mapType + "/Room" + roomKey + "/Props");
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
        if (isClosed && isEnabled)
        {
            animator.SetTrigger("Open");
            audioSource.Play();
            PickUpFactory.Instance.DropGold(transform.position, 1, minGoldDrop, maxGoldDrop, true);
            isClosed = false;
            ES3.Save<bool>("isClosed", isClosed, "Levels/" + mapType + "/Room" + roomKey + "/Props");
        }
    }

    private void GetInfo(MapType mapType, string roomKey, RoomType roomType)
    {
        this.mapType = mapType;
        this.roomKey = roomKey;
        this.roomType = roomType;
    }


}
