using UnityEngine;

public class MushroomKingdom : MonoBehaviour
{
    private MapType mapType;
    private Animator animator;
    private AudioSource audioSource;
    private int enemiesDiedCounter = 0;

    private void OnEnable()
    {
        EnemyStats.OnEnemyWasKilled += CountEemiesTillChange;
        MapManager.OnRoomLoaded += SetMapType;
    }

    private void OnDisable()
    {
        EnemyStats.OnEnemyWasKilled -= CountEemiesTillChange;
        MapManager.OnRoomLoaded -= SetMapType;
    }

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (ES3.KeyExists("Dead0", "Saves/Profile" + SaveProfile.SaveID + "/Bosses/MushroomBoss"))
        {
            bool dead = ES3.Load<bool>("Dead0", "Saves/Profile" + SaveProfile.SaveID + "/Bosses/MushroomBoss");
            gameObject.SetActive(!dead);
        }
    }

    private void CountEemiesTillChange(DamageSource damageSource)
    {
        enemiesDiedCounter++;
        if (enemiesDiedCounter > 15)
        {
            animator.SetTrigger("Die");
            audioSource.Play();
            Invoke("DisableScript", 2f);
        }
    }

    private void DisableScript()
    {
        enabled = false;
    }

    protected void SetMapType(MapType mapType, string roomKey, RoomType roomType)
    {
        this.mapType = mapType;
    }

}
