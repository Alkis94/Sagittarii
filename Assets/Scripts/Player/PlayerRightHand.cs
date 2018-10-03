using UnityEngine;
using System.Collections;

public class PlayerRightHand : MonoBehaviour
{

	private SpriteRenderer PlayerHandRenderer;
	private Animator PlayerHandAnimator;
    private Vector3 ArrowEmitterPosition;


   public GameObject ArrowEmitter;

   private float HorizontalDirection;
   private float ArrowPower;
   private bool IsFacingRight;
   private int HandDirection;




    void Start () 
	{

        PlayerHandRenderer = GetComponent<SpriteRenderer>();
        PlayerHandAnimator = GetComponent<Animator>();
        PlayerHandRenderer.flipX = false;
        IsFacingRight = true;
        HandDirection = 1;
    }

    void Update () 
	{

            HandFollowCursor();
            FlipPlayerHands();

            if (Input.GetButtonDown("Fire1"))
            {
                ArrowPower = 0;
                PlayerHandAnimator.SetTrigger("PlayerAttackHold");
                UIManager.Instance.ResetPower();
            }
            if (Input.GetButton("Fire1"))
            {
                ArrowPower += Time.deltaTime * ItemHandler.Instance.WolfPawMultiplier;
                UIManager.Instance.PowerIncreasing();
            }
            if (Input.GetButtonUp("Fire1"))
            {

                ArrowPower = ArrowPower > 1 ? 1 : ArrowPower;
                if (ArrowPower > 0.3)
                {
                    PlayerHandAnimator.SetTrigger("PlayerAttackRelease");
                    FireArrow(ArrowPower);
                }
                else
                {
                    PlayerHandAnimator.SetTrigger("PlayerAttackCanceled");
                }
            }
        
        
	}

	


	public void FireArrow(float arrowPower)
	{
        if(!ItemHandler.Instance.PlayerHasDeadBird)
        {
            ObjectFactory.Instance.CreateArrow(arrowPower, ArrowEmitter, 0);
        }
        else
        {

            //Item = DeadBird , behavior:

            ObjectFactory.Instance.CreateArrow(arrowPower, ArrowEmitter, 0.15f);
            ObjectFactory.Instance.CreateArrow(arrowPower, ArrowEmitter, 0);
            ObjectFactory.Instance.CreateArrow(arrowPower, ArrowEmitter, -0.15f);
        }
        
    }

    private void HandFollowCursor()
    {
        // convert mouse position into world coordinates
        Vector2 MouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 Direction = ((MouseWorldPosition - (Vector2)transform.position)).normalized;
        transform.right = Direction;
    }

    private void FlipPlayerHands()
    {
        HorizontalDirection = Input.GetAxis("Horizontal");

        if (HorizontalDirection > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
            PlayerHandRenderer.flipX = false;
            PlayerHandRenderer.flipY = false;
            ArrowEmitterPosition = ArrowEmitter.transform.localPosition;
            ArrowEmitter.transform.localPosition = Vector3.Scale(ArrowEmitterPosition, new Vector3(-1, -1, 1));
            //HandDirection = 1;
        }
        else if (HorizontalDirection < 0 && IsFacingRight)
        {
            IsFacingRight = false;
            PlayerHandRenderer.flipX = true;
            PlayerHandRenderer.flipY = true;
            ArrowEmitterPosition = ArrowEmitter.transform.localPosition;
            ArrowEmitter.transform.localPosition = Vector3.Scale(ArrowEmitterPosition, new Vector3(-1, -1, 1));
            //HandDirection = -1;
        }
    }

}
