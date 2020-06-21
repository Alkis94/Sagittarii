using UnityEngine;
using System.Collections;

public class UniqueRelic : MonoBehaviour
{
    [SerializeField]
    private string relic = "";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponentInChildren<UniqueRelicEnabler>().EnableRelic(relic);
        }
    }
}
