using UnityEngine;
using System.Collections;

public class SpawnedSideSpikes : MonoBehaviour
{
    private Animator animator;
    private BoxCollider2D boxCollider2D;

    private void OnEnable()
    {
        RoomManager.OnRoomFinished += RemoveSpikes;
    }

    private void OnDisable()
    {
        RoomManager.OnRoomFinished -= RemoveSpikes;
    }

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void RemoveSpikes()
    {
        animator.SetTrigger("Vanish");
        boxCollider2D.enabled = false;
    }

}
