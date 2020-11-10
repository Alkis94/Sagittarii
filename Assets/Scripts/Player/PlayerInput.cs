using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


//We first ensure this script runs before all other player scripts to prevent laggy
//inputs
[DefaultExecutionOrder(-100)]
public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private GameObject map;
    [SerializeField]
    private GameObject pauseMenu;

    [HideInInspector] public float horizontal;		
	[HideInInspector] public bool jumpHeld;			
	[HideInInspector] public bool jumpPressed;

    private const float jumpPressedBeforeGroundGraceDelay = 0.2f;
    private float jumpPressedGraceCooldown = 0;


	private bool readyToClear;                              //Bool used to keep input in sync
    private bool isTeleporting = false;
    private Coroutine teleportCoroutine = null;


    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private LayerMask collisionMask;
    private SpecialAbility specialAbility;
    private PlayerStats playerStats;
    private Animator animator;
    private AudioSource audioSource;
    private float timePassedHoldingAttack = 0;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        specialAbility = GetComponent<SpecialAbility>();
        playerStats = GetComponent<PlayerStats>();
        animator = GetComponentsInChildren<Animator>()[1];
        audioSource = GetComponentInChildren<AudioSource>();
    }

    void OnEnable()
    {
        PlayerStats.OnPlayerDied += DisableInput;
        SceneManager.sceneLoaded += ResetAnimator;
    }


    void OnDisable()
    {
        PlayerStats.OnPlayerDied -= DisableInput;
        SceneManager.sceneLoaded -= ResetAnimator;
    }


    void Update()
	{
        if(GameManager.GameState == GameStateEnum.unpaused)
        {
            if (Input.GetButtonDown("Cancel") && GameManager.GameState == GameStateEnum.unpaused)
            {
                pauseMenu.SetActive(true);
                GameManager.GameState = GameStateEnum.paused;
            }

            if (playerStats.Ammo > 0)
            {
                float projectilePower;
                float attackHoldAnimationSpeed = 1;
                float attackHoldAnimationLength = 0.333f;

                if ((Input.GetButtonDown("Fire1") || Input.GetButton("Fire1")) && animator.GetCurrentAnimatorStateInfo(0).IsName("IdleHands"))
                {
                    projectilePower = 0;
                    timePassedHoldingAttack = 0;
                    animator.SetTrigger("AttackHold");
                    audioSource.Play();
                }
                else if (Input.GetButton("Fire1") && animator.GetCurrentAnimatorStateInfo(0).IsName("AttackHold"))
                {
                    timePassedHoldingAttack += Time.deltaTime;
                    attackHoldAnimationSpeed = animator.GetCurrentAnimatorStateInfo(0).speedMultiplier;

                }
                else if (Input.GetButtonUp("Fire1") && animator.GetCurrentAnimatorStateInfo(0).IsName("AttackHold"))
                {
                    projectilePower = timePassedHoldingAttack * attackHoldAnimationSpeed / attackHoldAnimationLength;
                    projectilePower = projectilePower > 1 ? 1 : projectilePower;
                    if (projectilePower > 0.4)
                    {
                        animator.SetTrigger("AttackRelease");
                       
                    }
                    else
                    {
                        animator.SetTrigger("AttackCanceled");
                    }
                }
            }

            if (Input.GetButtonDown("Fire2"))
            {
                specialAbility.CastSpecialAbility();
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                var result = Physics2D.OverlapCircle(transform.position, 0.5f, 1 << LayerMask.NameToLayer("Interactables"));
                if (result != null)
                {
                    IInteractable interactable = result.gameObject.GetComponent<IInteractable>();
                    if (interactable != null)
                    {
                        interactable.Interact();
                    }
                }
            }

            if (Input.GetKeyUp(KeyCode.S))
            {
                if (boxCollider2D.IsTouchingLayers(collisionMask))
                {
                    gameObject.layer = 19; // PlayerNoPlatform Layer
                    StartCoroutine(ReturnToNormalLayer());
                }
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                map.SetActive(!map.activeInHierarchy);
            }

            ClearInput();
            ProcessInputs();
            horizontal = Mathf.Clamp(horizontal, -1f, 1f);

            if (isTeleporting)
            {
                if (Input.anyKey)
                {
                    if (!Input.GetKeyDown(KeyCode.F) && !Input.GetButtonDown("Cancel") && !Input.GetKey(KeyCode.F) && !Input.GetButton("Cancel"))
                    {
                        StopCoroutine(teleportCoroutine);
                        spriteRenderer.color = new Color(1f, 1f, 1f);
                        teleportCoroutine = null;
                        isTeleporting = false;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.F) && !isTeleporting && teleportCoroutine == null)
            {
                if (SceneManager.GetActiveScene().name != "Town")
                {
                    teleportCoroutine = StartCoroutine(Teleport());
                }
            }
        }
        else if (GameManager.GameState == GameStateEnum.paused)
        {
            if(Input.GetButtonDown("Cancel"))
            {
                pauseMenu.SetActive(false);
                GameManager.GameState = GameStateEnum.unpaused;
            }
        }
    }

    void FixedUpdate()
	{
		//In FixedUpdate() we set a flag that lets inputs to be cleared out during the 
		//next Update(). This ensures that all code gets to use the current inputs
		readyToClear = true;
	}

	void ClearInput()
	{
		if (!readyToClear)
			return;

		horizontal		= 0f;
		jumpHeld		= false;
		readyToClear	= false;

        if(jumpPressedGraceCooldown < Time.time)
        {
            jumpPressed = false;
        }
	}

	void ProcessInputs()
	{
		//Accumulate horizontal axis input
		horizontal		+= Input.GetAxis("Horizontal");

        //Accumulate button inputs
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressedGraceCooldown = Time.time + jumpPressedBeforeGroundGraceDelay;
        }

        jumpPressed	= jumpPressed || Input.GetButtonDown("Jump");
		jumpHeld = jumpHeld || Input.GetButton("Jump");
	}

    private void DisableInput(DamageSource damageSource)
    {
        horizontal = 0f;
        jumpPressed = false;
        jumpHeld = false;
        enabled = false;
    }

    IEnumerator Teleport()
    {
        yield return new WaitForSeconds(0.1f);
        isTeleporting = true;
        for(int i = 0; i < 6; i++)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f);
            yield return new WaitForSeconds(0.25f);
            spriteRenderer.color = new Color(0f, 0f, 1f);
            yield return new WaitForSeconds(0.25f);
        }
        spriteRenderer.color = new Color(1f, 1f, 1f);
        isTeleporting = false;
        teleportCoroutine = null;
        if(playerStats.CurrentHealth > 0)
        {
            UIManager.Instance.LoadSceneWithFade("Town");
        }
    }

    IEnumerator ReturnToNormalLayer()
    {
        float returnDelay = Time.time + .5f;
        while (true)
        {
            //we stop the player from being able to jump while falling, so he doesn't bug jump through the platform.
            jumpPressed = false;
            jumpHeld = false;
            if (Time.time > returnDelay)
            {
                gameObject.layer = 10; // Player Layer.
                yield break;
            }
            yield return null;
        }
    }

    private void ResetAnimator(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Town")
        {
            animator.SetTrigger("ResetHands");
        }
    }

}



// This is used to call QuickQuitMenu so you can test indintual scenes as builds!
//Replace the above "Cancel" to use!

//if (Input.GetButtonDown("Cancel") && !GameState.GamePaused)
//{
//    MenuFactory.CreateMenuAndPause(MenuFactory.QuickQuitMenu);
//}
//else if (Input.GetButtonDown("Cancel") && GameState.GamePaused)
//{
//    MenuFactory.DestroyMenuAndUnpause();
//}
