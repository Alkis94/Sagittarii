using UnityEngine;
using System.Collections;

public class CollisionDetectTest : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(gameObject.name + " was hit!");
    }
}
