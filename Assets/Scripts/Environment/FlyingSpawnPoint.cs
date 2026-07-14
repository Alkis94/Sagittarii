using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingSpawnPoint : MonoBehaviour
{
    [SerializeField]
    private Transform corner1;
    [SerializeField]
    private Transform corner2;

    [HideInInspector]
    public float MaxBoundX { private set; get; }
    [HideInInspector]
    public float MaxBoundY { private set; get; }
    [HideInInspector]
    public float MinBoundX { private set; get; }
    [HideInInspector]
    public float MinBoundY { private set; get; }

    private void Awake()
    {
        if(corner1.position.x >= corner2.position.x)
        {
            MaxBoundX = corner1.position.x;
            MinBoundX = corner2.position.x;
        }
        else
        {
            MaxBoundX = corner2.position.x;
            MinBoundX = corner1.position.x;
        }

        if (corner1.position.y >= corner2.position.y)
        {
            MaxBoundY = corner1.position.y;
            MinBoundY = corner2.position.y;
        }
        else
        {
            MaxBoundY = corner2.position.y;
            MinBoundY = corner1.position.y;
        }
    }
}
