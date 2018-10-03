using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeftHand : MonoBehaviour
{

    private SpriteRenderer PlayerHandRenderer;

    private float HorizontalDirection;
    private bool IsFacingRight;
    private int HandDirection;

    // Use this for initialization
    void Start ()
    {
        PlayerHandRenderer = GetComponent<SpriteRenderer>();
        PlayerHandRenderer.flipX = false;
        IsFacingRight = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        HandFollowCursor();
        FlipPlayerHand();
    }


    private void HandFollowCursor()
    {
        // convert mouse position into world coordinates
        Vector2 MouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 Direction = ((MouseWorldPosition - (Vector2)transform.position)).normalized;
        transform.right = Direction;
    }

    private void FlipPlayerHand()
    {
        HorizontalDirection = Input.GetAxis("Horizontal");

        if (HorizontalDirection > 0 && !IsFacingRight)
        {
            PlayerHandRenderer.flipX = false;
            PlayerHandRenderer.flipY = false;
            IsFacingRight = true;
            //HandDirection = 1;
        }
        else if (HorizontalDirection < 0 && IsFacingRight)
        {
            PlayerHandRenderer.flipX = true;
            PlayerHandRenderer.flipY = true;
            IsFacingRight = false;
            //HandDirection = -1;
        }
    }


}
