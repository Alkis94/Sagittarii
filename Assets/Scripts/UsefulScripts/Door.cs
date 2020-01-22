using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Door : MonoBehaviour
{

    private BoxCollider2D boxCollider2D;
    [SerializeField]
    private string levelToLoad;
    public static event Action<string> DoorEntered = delegate { };

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        PlayerInput.DoorEntered += OnDoorEntered;
    }

    private void OnDisable()
    {
        PlayerInput.DoorEntered -= OnDoorEntered;
    }

    private void OnDoorEntered()
    {
        BoxCollider2D playerCollider;
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
        if(boxCollider2D.IsTouching(playerCollider))
        {
            DoorEntered?.Invoke(levelToLoad);
        }
    }
}
