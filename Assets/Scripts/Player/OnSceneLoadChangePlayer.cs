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
    [SerializeField]
    private float playerTownSpeed = 6f;



    void OnEnable()
    {
        playerStats = GetComponent<PlayerStats>();
        currentPlayerSpeed = playerStats.speed;
        hands = transform.GetChild(0).gameObject;
        animator = GetComponent<Animator>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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
            transform.position = GameObject.FindGameObjectWithTag("PlayerSpawnPoint").transform.position;
        }
    }

}
