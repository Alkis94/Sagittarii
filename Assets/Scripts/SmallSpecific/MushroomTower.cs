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

        if (ES3.KeyExists("isBroken", "Levels/" + mapType + "/Room" + roomKey + "/Props"))
        {
            isBroken = ES3.Load<bool>("isBroken", "Levels/" + mapType + "/Room" + roomKey + "/Props");
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
            ES3.Save<bool>("isBroken", true, "Levels/" + mapType + "/Room" + roomKey + "/Props");

            if (ES3.FileExists("Saves/Profile" + SaveProfile.SaveID + "/Bosses/MushroomBoss"))
            {
                if (ES3.KeyExists("MushroomsDestroyed", "Saves/Profile" + SaveProfile.SaveID + "/Bosses/MushroomBoss"))
                {
                    int mushroomsDestroyed = ES3.Load<int>("MushroomsDestroyed", "Saves/Profile" + SaveProfile.SaveID + "/Bosses/MushroomBoss");
                    mushroomsDestroyed++;
                    ES3.Save<int>("MushroomsDestroyed", mushroomsDestroyed, "Saves/Profile" + SaveProfile.SaveID + "/Bosses/MushroomBoss");

                    if (mushroomsDestroyed >= 3 )
                    {
                        
                        ES3.Save<bool>("isLocked", false, "Saves/Profile" + SaveProfile.SaveID + "/Bosses/MushroomBoss");
                    }
                    
                }
                else
                {
                    ES3.Save<bool>("MushroomsDestroyed", 1, "Saves/Profile" + SaveProfile.SaveID + "/Bosses/MushroomBoss");
                }
            }
            else
            {
                ES3.Save<bool>("isLocked", true, "Saves/Profile" + SaveProfile.SaveID + "/Bosses/MushroomBoss");
                ES3.Save<int>("MushroomsDestroyed", 1, "Saves/Profile" + SaveProfile.SaveID + "/Bosses/MushroomBoss");
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

