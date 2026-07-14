using UnityEngine;
using Cinemachine;

public class MushroomTower : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;
    private CinemachineImpulseSource cinemachineImpulseSource;
    [SerializeField]
    private AudioClip breakSound;
    private bool isBroken = false;
    private int numberOfHitsToDestroy = 5;
    private MapType mapType;
    private RoomType roomType;
    private string roomKey;

    private void OnEnable()
    {
        MapManager.OnRoomLoaded += GetInfo;
    }

    private void OnDisable()
    {
        MapManager.OnRoomLoaded -= GetInfo;
    }


    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();

        if (ES3.KeyExists("isBroken", SaveManager.Instance.GetProfileRunPath() + SaveFolders.Levels + "/" + mapType + SaveFolders.Room + roomKey + SaveFolders.Props))
        {
            isBroken = ES3.Load<bool>("isBroken", SaveManager.Instance.GetProfileRunPath() + SaveFolders.Levels + "/" + mapType + SaveFolders.Room + roomKey + SaveFolders.Props);
        }

        if(isBroken)
        {
            transform.gameObject.layer = 14;
            animator.SetTrigger("AlreadyBroken");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isBroken && numberOfHitsToDestroy <= 0)
        {
            animator.SetTrigger("Break");
            audioSource.PlayOneShot(breakSound);
            cinemachineImpulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = 1;
            cinemachineImpulseSource.GenerateImpulse();
            isBroken = true;
            transform.gameObject.layer = 14;
            ES3.Save("isBroken", true, SaveManager.Instance.GetProfileRunPath() + SaveFolders.Levels + "/" + mapType + SaveFolders.Room + roomKey + SaveFolders.Props);

            if (ES3.FileExists(SaveManager.Instance.GetProfileRunPath() + SaveFolders.Bosses + "/MushroomBoss"))
            {
                if (ES3.KeyExists("MushroomsDestroyed", SaveManager.Instance.GetProfileRunPath() + SaveFolders.Bosses + "/MushroomBoss"))
                {
                    var mushroomsDestroyed = ES3.Load<int>("MushroomsDestroyed", SaveManager.Instance.GetProfileRunPath() + SaveFolders.Bosses + "/MushroomBoss");
                    mushroomsDestroyed++;
                    ES3.Save("MushroomsDestroyed", mushroomsDestroyed, SaveManager.Instance.GetProfileRunPath() + SaveFolders.Bosses + "/MushroomBoss");
                    if (mushroomsDestroyed >= 3 )
                    {
                        
                        ES3.Save("isLocked", false, SaveManager.Instance.GetProfileRunPath() + SaveFolders.Bosses + "/MushroomBoss");
                    }
                    
                }
                else
                {
                    ES3.Save("MushroomsDestroyed", true, SaveManager.Instance.GetProfileRunPath() + SaveFolders.Bosses + "/MushroomBoss");
                }
            }
            else
            {
                ES3.Save("isLocked", true, SaveManager.Instance.GetProfileRunPath() + SaveFolders.Bosses + "/MushroomBoss");
                ES3.Save("MushroomsDestroyed", 1, SaveManager.Instance.GetProfileRunPath() + SaveFolders.Bosses + "/MushroomBoss");
            }
        }
        else
        {
            numberOfHitsToDestroy--;
            cinemachineImpulseSource.GenerateImpulse();
            audioSource.Play();
        }
    }

    private void GetInfo(MapType mapType, string roomKey, RoomType roomType)
    {
        this.mapType = mapType;
        this.roomKey = roomKey;
        this.roomType = roomType;
    }
}

