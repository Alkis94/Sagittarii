using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour
{
    public float TargetDistance { get; set; } 
    public float DespawnTimer { get; set; }

    void Start()
    {
        LeanTween.moveX(gameObject, TargetDistance, DespawnTimer);
        Destroy(gameObject, DespawnTimer);
    }
}
