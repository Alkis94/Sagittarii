using UnityEngine;
using System.Collections;

public class SpecialRelic : MonoBehaviour
{
    [SerializeField]
    private string relic = "";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponentInChildren<SpecialRelicEnabler>().EnableRelic(relic);
        }
    }
}
