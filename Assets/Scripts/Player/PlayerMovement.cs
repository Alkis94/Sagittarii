// This script controls the player's movement and physics within the game

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const float skinWidth = .015f;
   // public bool drawDebugRaycasts = true;	//Should the environment checks be visualized

	[Header("Movement Properties")]

	public float coyoteDuration = .05f;		//How long the player can jump after falling
	public float maxFallSpeed = -25f;		//Max speed player can fall

	[Header("Jump Properties")]
	public float jumpForce = 1;			//Initial force of jump
	public float hangingJumpForce = 15f;	//Force of wall hanging jumo
	public float jumpHoldForce = 1.9f;		//Incremental force when jump is held
	public float jumpHoldDuration = .1f;	//How long the jump key can be held

	[Header("Environment Check Properties")]
	public float footOffset = - 0.065f;			//X Offset of feet raycast
	public float groundDistance = 0;		//Distance player is considered to be on the ground
	public LayerMask groundLayer;			//Layer of the ground

	[Header ("Status Flags")]
	public bool isOnGround;					//Is the player on the ground?
    public bool isFalling;                  //Is the player falling?
	public bool isJumping;					//Is player jumping?

    private Animator animator;
    private PlayerInput input;                     //The current inputs for the player
    private BoxCollider2D bodyCollider;             //The collider component
    private Rigidbody2D rigidBody;					//The rigidbody component
    private PlayerStats playerStats;
    private PlayerAudio playerAudio;
	
	float jumpTime;							//Variable to hold jump duration
	float coyoteTime;						//Variable to hold coyote duration

    private Vector2 bottomLeftCorner;
    private Vector2 bottomRightCorner;

    private int animatorVelocityY_ID;
    private int animatorVelocityX_ID;
    private int animatorIsGrounded_ID;


    void Start ()
	{
		//Get a reference to the required components
		input = GetComponent<PlayerInput>();
		rigidBody = GetComponent<Rigidbody2D>();
		bodyCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
        playerAudio = GetComponent<PlayerAudio>();

        animatorVelocityX_ID = Animator.StringToHash("VelocityX");
        animatorVelocityY_ID = Animator.StringToHash("VelocityY");
        animatorIsGrounded_ID = Animator.StringToHash("IsGrounded");
    }

    private void Update()
    {
        animator.SetFloat(animatorVelocityY_ID, rigidBody.velocity.y);
        animator.SetFloat(animatorVelocityX_ID, Mathf.Abs(rigidBody.velocity.x));
        animator.SetBool(animatorIsGrounded_ID, isOnGround);
    }

    void FixedUpdate()
	{
		//Check the environment to determine status
		PhysicsCheck();

		//Process ground and air movements
		GroundMovement();		
		MidAirMovement();
    }

    

	void PhysicsCheck()
	{
		//Start by assuming the player isn't on the ground
		isOnGround = false;

        Bounds bounds = GetComponent<BoxCollider2D>().bounds;
        bounds.Expand(skinWidth * -2);
        bottomLeftCorner = new Vector2(bounds.min.x, bounds.min.y);
        bottomRightCorner = new Vector2(bounds.max.x, bounds.min.y);

        //Cast rays for the left and right foot
        RaycastHit2D leftCheck = Physics2D.Raycast(bottomLeftCorner, Vector2.down, groundDistance,groundLayer);
        //Debug.DrawRay(bottomLeftCorner, Vector2.down , Color.red);
        RaycastHit2D rightCheck = Physics2D.Raycast(bottomRightCorner, Vector2.down, groundDistance, groundLayer);
        //Debug.DrawRay(bottomRightCorner, Vector2.down, Color.red);


        //If either ray hit the ground, the player is on the ground
        if (leftCheck || rightCheck)
        {
            isOnGround = true;
            if(isFalling)
            {
                isFalling = false;
                playerAudio.PlayGroundImpactSound();
            }
        }
        
            
	}

	void GroundMovement()
	{

		//Calculate the desired velocity based on inputs
		float xVelocity = playerStats.speed * input.horizontal;

		//Apply the desired velocity 
		rigidBody.velocity = new Vector2(xVelocity, rigidBody.velocity.y);

		//If the player is on the ground, extend the coyote time window
		if (isOnGround)
        {
            coyoteTime = Time.time + coyoteDuration;
      
        }
	}

	void MidAirMovement()
	{

        //If the jump key is pressed AND the player isn't already jumping AND EITHER
        //the player is on the ground or within the coyote time window...
        if (input.jumpPressed && !isJumping && (isOnGround || coyoteTime > Time.time))
		{
			//...The player is no longer on the groud and is jumping...
			isOnGround = false;
			isJumping = true;

			//...record the time the player will stop being able to boost their jump...
			jumpTime = Time.time + jumpHoldDuration;

			//...add the jump force to the rigidbody...
			rigidBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            playerAudio.PlayJumpSound();
		}
		//Otherwise, if currently within the jump time window...
		else if (isJumping)
		{
           
			//...and the jump button is held, apply an incremental force to the rigidbody...
			if (input.jumpHeld)
            {
                rigidBody.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);
            }
				

			//...and if jump time is past, set isJumping to false
			if (jumpTime <= Time.time)
				isJumping = false;
		}

		//If player is falling to fast, reduce the Y velocity to the max
		if (rigidBody.velocity.y < maxFallSpeed)
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, maxFallSpeed);

        if(rigidBody.velocity.y < 0)
        {
            isFalling = true;
        }
	}
}
