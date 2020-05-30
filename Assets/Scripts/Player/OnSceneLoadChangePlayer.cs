using UnityEngine;
using UnityEngine.SceneManagement;

public class OnSceneLoadChangePlayer : MonoBehaviour
{
    private GameObject hands;
    private Animator animator;
    private PlayerStats playerStats;
    [SerializeField]
    private RuntimeAnimatorController townController;
    [SerializeField]
    private RuntimeAnimatorController bodyController;

    private float currentPlayerSpeed;
    private Direction lastDoorTakenPlacement;
    [SerializeField]
    private float playerTownSpeed = 6f;

    private void OnEnable()
    {
        playerStats = GetComponent<PlayerStats>();
        currentPlayerSpeed = playerStats.speed;
        hands = transform.GetChild(0).gameObject;
        animator = GetComponent<Animator>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        RoomChanger.OnRoomChangerEntered += NextPlayerSpawnPointDirection;
        MapChanger.OnMapChangerEnteredPlayerDirection += NextPlayerSpawnPointDirection;
        Door.DoorEntered += NextPlayerSpawnPointDirection;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        RoomChanger.OnRoomChangerEntered -= NextPlayerSpawnPointDirection;
        MapChanger.OnMapChangerEnteredPlayerDirection -= NextPlayerSpawnPointDirection;
        Door.DoorEntered -= NextPlayerSpawnPointDirection;
    }

    private void Awake()
    {
        lastDoorTakenPlacement = Direction.west;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Town")
        {
            currentPlayerSpeed = playerStats.speed;
            playerStats.speed = playerTownSpeed;
            hands.SetActive(false);
            animator.runtimeAnimatorController = townController;
        }
        else
        { 
            playerStats.speed = currentPlayerSpeed;
            animator.runtimeAnimatorController = bodyController;
            hands.SetActive(true);
        }

        transform.position = NextPlayerSpawnPointPosition();
    }

    private void NextPlayerSpawnPointDirection(string levelToLoad)
    {
        this.lastDoorTakenPlacement = Direction.middle;
    }

    private void NextPlayerSpawnPointDirection(Direction lastDoorTakenPlacement)
    {
        this.lastDoorTakenPlacement = lastDoorTakenPlacement;
    }



    private Vector3 NextPlayerSpawnPointPosition()
    {
        GameObject thisSpawn = GameObject.FindGameObjectWithTag("PlayerSpawnWest");
        if (lastDoorTakenPlacement == Direction.east && thisSpawn != null)
        {
            return thisSpawn.transform.position;
        }

        thisSpawn = GameObject.FindGameObjectWithTag("PlayerSpawnEast");
        if (lastDoorTakenPlacement == Direction.west && thisSpawn != null)
        {
            return thisSpawn.transform.position;
        }

        thisSpawn = GameObject.FindGameObjectWithTag("PlayerSpawnSouth");
        if (lastDoorTakenPlacement == Direction.north && thisSpawn != null)
        {
            return thisSpawn.transform.position;
        }

        thisSpawn = GameObject.FindGameObjectWithTag("PlayerSpawnNorth");
        if (lastDoorTakenPlacement == Direction.south && thisSpawn != null)
        {
            return thisSpawn.transform.position;
        }

        thisSpawn = GameObject.FindGameObjectWithTag("PlayerSpawnMiddle");
        if (lastDoorTakenPlacement == Direction.middle && thisSpawn != null)
        {
            return thisSpawn.transform.position;
        }

        return transform.position;
    }

    private void SavePlayer()
    {
        ES3.Save<int>("CurrentHealth", playerStats.CurrentHealth,"Profile" + SaveProfileHandler.SaveID + "/Player");
        ES3.Save<int>("MaximumHealth", playerStats.MaximumHealth, "Profile" + SaveProfileHandler.SaveID + "/Player");
        ES3.Save<int>("CurrentEnergy", playerStats.CurrentEnergy, "Profile" + SaveProfileHandler.SaveID + "/Player");
        ES3.Save<int>("MaximumEnergy", playerStats.MaximumEnergy, "Profile" + SaveProfileHandler.SaveID + "/Player");
        ES3.Save<int>("Gold", playerStats.Gold, "Profile" + SaveProfileHandler.SaveID + "/Player");
        ES3.Save<int>("Ammo", playerStats.Ammo, "Profile" + SaveProfileHandler.SaveID + "/Player");
    }

    //private Vector3 ReturnAnySpawnPointPosition()
    //{
    //    GameObject tmp = GameObject.FindGameObjectWithTag("PlayerSpawnWest");
    //    if(tmp != null)
    //    {
    //        return tmp.transform.position;
    //    }

    //    tmp = GameObject.FindGameObjectWithTag("PlayerSpawnEast");
    //    if (tmp != null)
    //    {
    //        return tmp.transform.position;
    //    }

    //    tmp = GameObject.FindGameObjectWithTag("PlayerSpawnNorth");
    //    if (tmp != null)
    //    {
    //        return tmp.transform.position;
    //    }

    //    tmp = GameObject.FindGameObjectWithTag("PlayerSpawnSouth");
    //    if (tmp != null)
    //    {
    //        return tmp.transform.position;
    //    }

    //    tmp = GameObject.FindGameObjectWithTag("PlayerSpawn");
    //    if (tmp != null)
    //    {
    //        return tmp.transform.position;
    //    }

    //    return transform.position;
    //}

}
