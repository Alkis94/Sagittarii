using UnityEngine;
using System.Collections;

public class SpawnedSideSpikes : MonoBehaviour
{
    private Animator animator;
    private BoxCollider2D boxCollider2D;
    private MapType mapType;
    private string roomKey;

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
        mapType = MapManager.Instance.CurrentMap;
        roomKey = MapManager.Instance.CurrentMapCoords.x.ToString() + MapManager.Instance.CurrentMapCoords.y.ToString();

        if (ES3.FileExists("Levels/" + mapType + "/Room" + roomKey + "/Enemies"))
        {
            gameObject.SetActive(false);
        }

        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void RemoveSpikes()
    {
        animator.SetTrigger("Vanish");
        boxCollider2D.enabled = false;
    }

}
