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

	private bool readyToClear;                              //Bool used to keep input in sync
    private bool isTeleporting = false;
    private Coroutine teleportCoroutine = null;


    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private LayerMask collisionMask;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        PlayerStats.OnPlayerDied += DisableInput;
    }


    void OnDisable()
    {
        PlayerStats.OnPlayerDied -= DisableInput;
    }


    void Update()
	{

        if (Input.GetButtonDown("Cancel") && GameManager.GameState == GameStateEnum.unpaused)
        {
            pauseMenu.SetActive(true);
            GameManager.GameState = GameStateEnum.paused;
        }

        else if (Input.GetButtonDown("Cancel") && GameManager.GameState == GameStateEnum.paused)
        {
            pauseMenu.SetActive(false);
            GameManager.GameState = GameStateEnum.unpaused;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            var result = Physics2D.OverlapCircle(transform.position, 0.5f, 1 << LayerMask.NameToLayer("Interactables"));
            if(result != null)
            {
                IInteractable interactable = result.gameObject.GetComponent<IInteractable>();
                if(interactable != null)
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
                if(!Input.GetKeyDown(KeyCode.F) && !Input.GetButtonDown("Cancel") && !Input.GetKey(KeyCode.F) && !Input.GetButton("Cancel"))
                {
                    Debug.Log("Canceled");
                    StopCoroutine(teleportCoroutine);
                    spriteRenderer.color = new Color(1f, 1f, 1f);
                    teleportCoroutine = null;
                    isTeleporting = false;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F) && !isTeleporting && teleportCoroutine == null)
        {
            if(SceneManager.GetActiveScene().name != "Town")
            {
                teleportCoroutine = StartCoroutine(Teleport());
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
		jumpPressed		= false;
		jumpHeld		= false;
		readyToClear	= false;
	}

	void ProcessInputs()
	{
		//Accumulate horizontal axis input
		horizontal		+= Input.GetAxis("Horizontal");

		//Accumulate button inputs
		jumpPressed		= jumpPressed || Input.GetButtonDown("Jump");
		jumpHeld		= jumpHeld || Input.GetButton("Jump");
	}

    private void DisableInput()
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
        SceneManager.LoadScene("Town");
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
