using UnityEngine;
using System.Collections;

public class EnemyCriticalHit : MonoBehaviour
{

    public bool criticalHit { private set; get; } = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Arrow")
        {
            criticalHit = true;
        }
    }
}
