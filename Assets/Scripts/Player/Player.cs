using UnityEngine;

public partial class Player: MonoBehaviour 
{
	private Animator PlayerAnimator;
	private Rigidbody2D PlayerRB2D;
	public Transform GroundCheck;
    public AudioSource JumpSound;
    public AudioSource PlayerGotHitSound;
    public AudioSource PlayerDiedSound;
    public GameObject PlayerHands;

    private bool DoubleJump;
    private bool TripleJump;
    private bool PlayerHasBatWings;
    private bool IsFacingRight;
    private bool PlayerNotDead;

    private float HorizontalDirection;
    private float Speed;
    private int MaximumHealth;
    private int Health;


    void Start () 
	{
		PlayerAnimator = GetComponent<Animator> ();
		PlayerRB2D = GetComponent<Rigidbody2D> ();
		Speed = C.PLAYER_SPEED;
        MaximumHealth = C.PLAYER_MAXIMUM_HEALTH;
        Health = C.PLAYER_MAXIMUM_HEALTH;
        DoubleJump = false;
        TripleJump = false;
        IsFacingRight = true;
        PlayerNotDead = true;
        PlayerHasBatWings = false;
    }

	void Update () 
	{
        if (Health > 0)
        {
            if (!GameHandler.Instance.GamePaused)
            {
                if (Input.GetButton("Horizontal"))
                {
                    PlayerMoveHorizontally();
                }
                if (Input.GetButtonUp("Horizontal"))
                {
                    PlayerStoppedMoving();
                }
                if (Input.GetButtonDown("Jump"))
                {
                    PlayerStoppedMoving();
                    PlayerJump();
                }
            }
            if (Input.GetButtonDown("Cancel")&& !GameHandler.Instance.GamePaused)
            {
                GameHandler.Instance.CallPausedMenu();
            }
            else if (Input.GetButtonDown("Cancel") && GameHandler.Instance.GamePaused)
            {
                GameHandler.Instance.UnpauseGame();

            }
        }
	}



    private void PlayerMoveHorizontally()
    {

        HorizontalDirection = Input.GetAxis("Horizontal");
        if (HorizontalDirection > 0 && !IsFacingRight)
        {
            FlipSprite();
        }
        else if (HorizontalDirection < 0 && IsFacingRight)
        {
            FlipSprite();
        }

        //if (transform.position.x <= -C.PLAYER_BOUNDARY)
        //{
        //    transform.position = new Vector2(-C.PLAYER_BOUNDARY, transform.position.y);
        //}
        //else if (transform.position.x >= C.PLAYER_BOUNDARY)
        //{
        //    transform.position = new Vector2(C.PLAYER_BOUNDARY, transform.position.y);
        //}


        Vector3 movement = new Vector3(HorizontalDirection, 0, 0);
        transform.Translate(movement * Time.deltaTime * Speed);

        if (PlayerIsGrounded())
        {
            PlayerAnimator.SetBool("PlayerIsMoving", true);
        }

    }



    //here is what we need to change when the player stops moving

    private void PlayerStoppedMoving()
	{
		PlayerAnimator.SetBool ("PlayerIsMoving", false);
	}

	//The function called when player inputs a Jump order

	private void PlayerJump()
	{
		if (PlayerIsGrounded()) 
		{
			DoubleJump = true;
            PlayerAnimator.SetTrigger ("PlayerJumped");
			PlayerRB2D.velocity = new Vector2(PlayerRB2D.velocity.x,0);
			PlayerRB2D.AddForce (new Vector2 (0, 10), ForceMode2D.Impulse);
            JumpSound.Play();
        }
		else if (DoubleJump && !PlayerIsGrounded()) 
		{
			DoubleJump = false;
            TripleJump = true;
            PlayerAnimator.SetTrigger ("PlayerJumped");
			PlayerRB2D.velocity = new Vector2(PlayerRB2D.velocity.x,0);
			PlayerRB2D.AddForce (new Vector2 (0, 10), ForceMode2D.Impulse);
            JumpSound.Play();
        }
        else if (TripleJump && !PlayerIsGrounded() && PlayerHasBatWings)
        {
            TripleJump = false;
            PlayerAnimator.SetTrigger("PlayerJumped");
            PlayerRB2D.velocity = new Vector2(PlayerRB2D.velocity.x, 0);
            PlayerRB2D.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
            JumpSound.Play();
        }
    }


	//This function checks if the player is touching ground or not

	private bool PlayerIsGrounded()
	{
		return Physics2D.Linecast(transform.position, GroundCheck.position, 1 << LayerMask.NameToLayer("GroundLayer"));
	}

    private void PlayerDie()
    {
        PlayerDiedSound.Play();
        Destroy(PlayerHands);
        PlayerAnimator.SetTrigger("PlayerDied");
        Invoke("PlayerDiedDelayedMenu", 3);

    }

    private void PlayerDiedDelayedMenu()
    {
        Time.timeScale = 0;
        GameHandler.Instance.CallPlayerDiedMenu();
    }

	private void FlipSprite()
	{
			IsFacingRight = !IsFacingRight;

			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyProjectile" && PlayerNotDead)
        {
            Health -= 10;
            UIManager.Instance.UpdateHealth(Health, MaximumHealth);
            if(Health < 1)
            {
                PlayerNotDead = false;
                PlayerDie();
            }
            else
            {
                 PlayerGotHitSound.Play();
            }
        }

        if(other.tag == "HealthPickup" && MaximumHealth != Health)
        {
            Health += 10;
            UIManager.Instance.UpdateHealth(Health, MaximumHealth);
        }
        if (other.tag == "MaximumHealthPickup")
        {
            Health += 10;
            MaximumHealth += 10;
            UIManager.Instance.UpdateHealth(Health, MaximumHealth);
        }
        if(other.tag == "BatWingsPickup")
        {
            PlayerHasBatWings = true;
        }
        if(other.tag == "WolfPawPickup")
        {
            ItemHandler.Instance.WolfPawMultiplier += 1;
        }
        if (other.tag == "DeadBirdPickup")
        {
            ItemHandler.Instance.PlayerHasDeadBird = true;
        }
        if (other.tag == "ImpFlamePickup")
        {
            ItemHandler.Instance.PlayerHasImpFlame = true;
            ItemHandler.Instance.ImpFlameMultiplier += 1;
        }
    }
}
