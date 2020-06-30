using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSceneChangeManager : MonoBehaviour
{
    private GameObject hands;
    private Animator animator;
    private PlayerStats playerStats;
    [SerializeField]
    private RuntimeAnimatorController townController;
    [SerializeField]
    private RuntimeAnimatorController bodyController;

    private Direction lastDoorTakenPlacement;


    private void OnEnable()
    {
        playerStats = GetComponent<PlayerStats>();
        hands = transform.GetChild(0).gameObject;
        animator = GetComponent<Animator>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        RoomChanger.OnRoomChangerEntered += NextPlayerSpawnPointDirection;
        MapChanger.OnMapChangerEnteredPlayerDirection += NextPlayerSpawnPointDirection;
        BossDoor.DoorEntered += NextPlayerSpawnPointDirection;
        NextPlayerSpawnPointDirection();
        transform.position = NextPlayerSpawnPointPosition();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        RoomChanger.OnRoomChangerEntered -= NextPlayerSpawnPointDirection;
        MapChanger.OnMapChangerEnteredPlayerDirection -= NextPlayerSpawnPointDirection;
        BossDoor.DoorEntered -= NextPlayerSpawnPointDirection;
    }

    private void Awake()
    {
        lastDoorTakenPlacement = Direction.west;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Town")
        {
            hands.SetActive(false);
            animator.runtimeAnimatorController = townController;
        }
        else
        { 
            animator.runtimeAnimatorController = bodyController;
            hands.SetActive(true);
        }

        transform.position = NextPlayerSpawnPointPosition();
    }

    private void NextPlayerSpawnPointDirection(string levelToLoad = null)
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

}
