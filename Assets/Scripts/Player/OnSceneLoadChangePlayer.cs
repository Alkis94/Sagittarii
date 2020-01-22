using System.Collections;
using System.Collections.Generic;
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
    private Vector3 newPlayerPositionOnSceneChange;
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

        NextPlayerSpawnPointPosition();
        transform.position = newPlayerPositionOnSceneChange;
    }

    private void NextPlayerSpawnPointDirection(string levelToLoad)
    {
        this.lastDoorTakenPlacement = Direction.middle;
    }

    private void NextPlayerSpawnPointDirection(Direction lastDoorTakenPlacement)
    {
        this.lastDoorTakenPlacement = lastDoorTakenPlacement;
    }



    private void NextPlayerSpawnPointPosition()
    {
        if (lastDoorTakenPlacement == Direction.west)
        {
            newPlayerPositionOnSceneChange = GameObject.FindGameObjectWithTag("PlayerSpawnEast").transform.position;
        }
        else if (lastDoorTakenPlacement == Direction.east)
        {
            newPlayerPositionOnSceneChange = GameObject.FindGameObjectWithTag("PlayerSpawnWest").transform.position;
        }
        else if (lastDoorTakenPlacement == Direction.north)
        {
            newPlayerPositionOnSceneChange = GameObject.FindGameObjectWithTag("PlayerSpawnSouth").transform.position;
        }
        else if (lastDoorTakenPlacement == Direction.south)
        {
            newPlayerPositionOnSceneChange = GameObject.FindGameObjectWithTag("PlayerSpawnNorth").transform.position;
        }
        else if(lastDoorTakenPlacement == Direction.middle)
        {
            newPlayerPositionOnSceneChange = GameObject.FindGameObjectWithTag("PlayerSpawn").transform.position;
        }
        else
        {
            Debug.LogError("Player spawn not found!");
        }
    }

}
