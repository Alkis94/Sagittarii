using UnityEngine;
using System.Collections;
using System;

public class MapChanger : MonoBehaviour
{

    public static event Action<MapType, MapType> OnMapChangerEntered = delegate { };
    public static event Action<Direction> OnMapChangerEnteredPlayerDirection = delegate { };

    

    [SerializeField]
    private MapType currentMap;
    [SerializeField]
    private MapType nextMap;
    [SerializeField]
    private Direction doorPlacement;

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && enabled == true)
        {
            OnMapChangerEntered?.Invoke(currentMap,nextMap);
            OnMapChangerEnteredPlayerDirection?.Invoke(doorPlacement);
        }
    }

}
