using UnityEngine;

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

    void Start()
    {
        var mapType = MapManager.Instance.CurrentMapInfo.Type;
        var roomKey = MapManager.Instance.CurrentMapInfo.Coords.x.ToString() + MapManager.Instance.CurrentMapInfo.Coords.y.ToString();

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
