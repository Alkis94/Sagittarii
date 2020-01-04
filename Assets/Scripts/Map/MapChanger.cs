using UnityEngine;
using System.Collections;
using System;

public class MapChanger : MonoBehaviour
{

    public static event Action<MapType, MapType> OnMapChangerEntered = delegate { };
    public static event Action<Direction> OnMapChangerEnteredPlayerDirection = delegate { };

    private SpriteRenderer spriteRenderer;
    private Transform playerTransform;
    private float distance;
    private float alphaValue;
    private Color alphaChanger;
    [SerializeField]
    private MapType currentMap;
    [SerializeField]
    private MapType nextMap;
    [SerializeField]
    private Direction doorPlacement;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        distance = Vector3.Distance(transform.position, playerTransform.position);
        alphaValue = distance > 10 ? 0 : 1 / (distance * 2);
        alphaChanger = new Color(255, 255, 255, alphaValue);
        spriteRenderer.color = alphaChanger;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            OnMapChangerEntered?.Invoke(currentMap,nextMap);
            OnMapChangerEnteredPlayerDirection?.Invoke(doorPlacement);
        }
    }

    
}
