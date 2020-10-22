using UnityEngine;

public class BreakableProps : MonoBehaviour
{
    [SerializeField]
    private int minGoldDrop = 0;
    [SerializeField]
    private int maxGoldDrop = 100;
    private const float goldDropChance = 0.1f;
    private Animator animator;
    private AudioSource audioSource;
    private MapType mapType;
    private RoomType roomType;
    private string roomKey;
    private bool isEnabled = false;
    private bool isBroken = false;

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
        if (ES3.KeyExists("isEnabled" + transform.GetSiblingIndex(), "Levels/" + mapType + "/Room" + roomKey + "/Props"))
        {
            isEnabled = ES3.Load<bool>("isEnabled" + transform.GetSiblingIndex(), "Levels/" + mapType + "/Room" + roomKey + "/Props");
        }
        else
        {
            float randomNumber = Random.Range(0f, 1f);
            if(randomNumber < goldDropChance)
            {
                isEnabled = true;
            }

            ES3.Save<bool>("isEnabled" + transform.GetSiblingIndex(), isEnabled, "Levels/" + mapType + "/Room" + roomKey + "/Props");
        }

        if (isEnabled)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            animator = GetComponentInChildren<Animator>();
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            transform.gameObject.layer = 14;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isBroken && isEnabled)
        {
            animator.SetTrigger("Broken");
            PickUpFactory.Instance.DropGold(transform.position, 1, minGoldDrop, maxGoldDrop, true);
            audioSource.Play();
            isBroken = true;
            ES3.Save<bool>("isEnabled" + transform.GetSiblingIndex(), false, "Levels/" + mapType + "/Room" + roomKey + "/Props");
            transform.gameObject.layer = 14;
        }
    }

    private void GetInfo(MapType mapType, string roomKey, RoomType roomType)
    {
        this.mapType = mapType;
        this.roomKey = roomKey;
        this.roomType = roomType;
    }
}
