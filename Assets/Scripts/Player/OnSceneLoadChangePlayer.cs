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
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        RoomChanger.OnRoomChangerEntered -= NextPlayerSpawnPointDirection;
        MapChanger.OnMapChangerEnteredPlayerDirection -= NextPlayerSpawnPointDirection;
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

    private void NextPlayerSpawnPointDirection(Direction lastDoorTakenPlacement)
    {
        this.lastDoorTakenPlacement = lastDoorTakenPlacement;
    }

    private void NextPlayerSpawnPointPosition()
    {
        if (lastDoorTakenPlacement == Direction.west)
        {
            newPlayerPositionOnSceneChange = GameObject.FindGameObjectWithTag("DoorEast").transform.position;
        }
        else if (lastDoorTakenPlacement == Direction.east)
        {
            newPlayerPositionOnSceneChange = GameObject.FindGameObjectWithTag("DoorWest").transform.position;
        }
        else if (lastDoorTakenPlacement == Direction.north)
        {
            newPlayerPositionOnSceneChange = GameObject.FindGameObjectWithTag("DoorSouth").transform.position;
            newPlayerPositionOnSceneChange = new Vector3(newPlayerPositionOnSceneChange.x, newPlayerPositionOnSceneChange.y + 2, newPlayerPositionOnSceneChange.z);
        }
        else if (lastDoorTakenPlacement == Direction.south)
        {
            newPlayerPositionOnSceneChange = GameObject.FindGameObjectWithTag("DoorNorth").transform.position;
        }
    }

}
