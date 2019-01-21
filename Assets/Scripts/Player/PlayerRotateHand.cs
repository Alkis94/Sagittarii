using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotateHand : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private Vector3 MousePosition;

    private float HorizontalDirection;
    private int HandDirection;

    // Use this for initialization
    void Start ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        HandFollowCursor();

        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (transform.position.x < MousePosition.x)
        {
            spriteRenderer.flipY = false;
        }
        else if (transform.position.x > MousePosition.x)
        {
            spriteRenderer.flipY = true;
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
