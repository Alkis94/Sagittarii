using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHands : MonoBehaviour
{

    protected SpriteRenderer PlayerHandsRenderer;
    //protected Animator PlayerHandsAnimator;
    //protected Vector3 ArrowEmitterPosition;

    //public GameObject ArrowEmitter;

    protected float HorizontalDirection;
    //protected float ArrowPower;
    protected bool IsFacingRight;
    protected int HandDirection;
    // Use this for initialization


    void Start ()
    {
        //PlayerHandsRenderer = GetComponent<SpriteRenderer>();
        //PlayerHandsAnimator = GetComponent<Animator>();
        PlayerHandsRenderer.flipX = false;
        IsFacingRight = true;
        //HandDirection = 1;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    protected void HandsFollowCursor()
    {
        // convert mouse position into world coordinates
        Vector2 MouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 Direction = ((MouseWorldPosition - (Vector2)transform.position)* HandDirection).normalized;
        transform.right = Direction;
    }

    protected void FlipPlayerHands()
    {
        HorizontalDirection = Input.GetAxis("Horizontal");

        if (HorizontalDirection > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
            //PlayerHandsRenderer.flipX = false;
            //PlayerHandsRenderer.flipY = false;
            //ArrowEmitterPosition = ArrowEmitter.transform.localPosition;
            //ArrowEmitter.transform.localPosition = Vector3.Scale(ArrowEmitterPosition, new Vector3(-1, -1, 1));
            HandDirection = 1;
        }
        else if (HorizontalDirection < 0 && IsFacingRight)
        {
            IsFacingRight = false;
            //PlayerHandsRenderer.flipX = true;
            //PlayerHandsRenderer.flipY = true;
            //ArrowEmitterPosition = ArrowEmitter.transform.localPosition;
            //ArrowEmitter.transform.localPosition = Vector3.Scale(ArrowEmitterPosition, new Vector3(-1, -1, 1));
            HandDirection = -1;
        }
    }

}
