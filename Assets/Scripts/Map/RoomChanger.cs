using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class RoomChanger : MonoBehaviour
{

    public static event Action<Direction> OnRoomChangerEntered = delegate { };
    [SerializeField]
    private Direction doorPlacement;
    private BoxCollider2D boxCollider2D;
    [SerializeField]
    private string levelToLoad;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            OnRoomChangerEntered?.Invoke(doorPlacement);
        }
    }
}
