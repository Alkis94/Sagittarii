using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSceneChangeManager : MonoBehaviour
{
    private GameObject hands;
    private Animator animator;
    private Rigidbody2D rigidbody2d;
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
        rigidbody2d = GetComponent<Rigidbody2D>();
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

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "Town")
        {
            MakeChangesForTown();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        rigidbody2d.gravityScale = 3;

        if (scene.name == "Town")
        {
            MakeChangesForTown();
        }
        else
        { 
            animator.runtimeAnimatorController = bodyController;
            hands.SetActive(true);
        }

        transform.position = NextPlayerSpawnPointPosition();
    }

    private void MakeChangesForTown()
    {
        animator.runtimeAnimatorController = townController;
        hands.SetActive(false);
    }

    public void NextPlayerSpawnPointDirection(string levelToLoad = null)
    {
        lastDoorTakenPlacement = Direction.middle;
    }

    private void NextPlayerSpawnPointDirection(Direction lastDoorTakenPlacement)
    {
        this.lastDoorTakenPlacement = lastDoorTakenPlacement;
    }

    private Vector3 NextPlayerSpawnPointPosition()
    {
        //Try to find the correct next spawn depending on the last door taken.
        if (lastDoorTakenPlacement == Direction.east)
        {
            GameObject thisSpawn = GameObject.FindGameObjectWithTag("PlayerSpawnWest");
            if(thisSpawn != null)
            {
                return thisSpawn.transform.position;
            }
        }
        else if (lastDoorTakenPlacement == Direction.west)
        {
            GameObject thisSpawn = GameObject.FindGameObjectWithTag("PlayerSpawnEast");
            if (thisSpawn != null)
            {
                return thisSpawn.transform.position;
            }
        }
        else if (lastDoorTakenPlacement == Direction.north)
        {
            GameObject thisSpawn = GameObject.FindGameObjectWithTag("PlayerSpawnSouth");
            if (thisSpawn != null)
            {
                return thisSpawn.transform.position;
            }
        }
        else if (lastDoorTakenPlacement == Direction.south)
        {
            GameObject thisSpawn = GameObject.FindGameObjectWithTag("PlayerSpawnNorth");
            if (thisSpawn != null)
            {
                return thisSpawn.transform.position;
            }
        }
        else if (lastDoorTakenPlacement == Direction.middle)
        {
            GameObject thisSpawn = GameObject.FindGameObjectWithTag("PlayerSpawnMiddle");
            if (thisSpawn != null)
            {
                return thisSpawn.transform.position;
            }
        }


        //If the correct spawn was not found return any possible spawn position for the player
        GameObject anySpawn = GameObject.FindGameObjectWithTag("PlayerSpawnMiddle");
        if(anySpawn != null)
        {
            return anySpawn.transform.position;
        }
        anySpawn = GameObject.FindGameObjectWithTag("PlayerSpawnWest");
        if (anySpawn != null)
        {
            return anySpawn.transform.position;
        }
        anySpawn = GameObject.FindGameObjectWithTag("PlayerSpawnEast");
        if (anySpawn != null)
        {
            return anySpawn.transform.position;
        }
        anySpawn = GameObject.FindGameObjectWithTag("PlayerSpawnNorth");
        if (anySpawn != null)
        {
            return anySpawn.transform.position;
        }
        anySpawn = GameObject.FindGameObjectWithTag("PlayerSpawnSouth");
        if (anySpawn != null)
        {
            return anySpawn.transform.position;
        }

        
        Debug.Log("No player spawn position found in the scene!");
        return transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DoorNorth" && enabled == true)
        {
            rigidbody2d.gravityScale = 0;
        }
    }

}
