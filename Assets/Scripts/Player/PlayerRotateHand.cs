using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotateHand : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private Vector3 mousePosition;

    private float horizontalDirection;
    private int handDirection;


    void Start ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

	void Update ()
    {
        if(GameManager.GameState == GameStateEnum.unpaused)
        {
            HandFollowCursor();
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (transform.position.x < mousePosition.x)
            {
                spriteRenderer.flipY = false;

            }
            else if (transform.position.x > mousePosition.x)
            {
                spriteRenderer.flipY = true;


            }
        }
    }

    private void HandFollowCursor()
    {
        // convert mouse position into world coordinates
        Vector2 MouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 Direction = ((MouseWorldPosition - (Vector2)transform.position)).normalized;
        transform.right = Direction;

    }

}
