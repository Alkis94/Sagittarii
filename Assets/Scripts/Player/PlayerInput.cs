// This script handles inputs for the player. It serves two main purposes: 1) wrap up
// inputs so swapping between mobile and standalone is simpler and 2) keeping inputs
// from Update() in sync with FixedUpdate()

using Factories;
using UnityEngine;
using System.Collections;


//We first ensure this script runs before all other player scripts to prevent laggy
//inputs
[DefaultExecutionOrder(-100)]
public class PlayerInput : MonoBehaviour
{
    public GameObject map;

	[HideInInspector] public float horizontal;		//Float that stores horizontal input
	[HideInInspector] public bool jumpHeld;			//Bool that stores jump pressed
	[HideInInspector] public bool jumpPressed;		//Bool that stores jump held

	private bool readyToClear;                              //Bool used to keep input in sync
    //private const float teleportCastTime = 3;
    //private float teleportTimeCasted = 0;

    private BoxCollider2D boxCollider2D;
    [SerializeField]
    private LayerMask collisionMask;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
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

        if (Input.GetButtonDown("Cancel") && !GameState.GamePaused)
        {
            MenuFactory.CreateMenuAndPause(MenuFactory.PauseMenu);
        }

        else if (Input.GetButtonDown("Cancel") && GameState.GamePaused)
        {
            MenuFactory.DestroyMenuAndUnpause();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            var results = Physics2D.OverlapCircle(transform.position, 0.5f, 1 << LayerMask.NameToLayer("Interactables"));
            if(results != null)
            {
                IInteractable interactable = results.gameObject.GetComponent<IInteractable>();
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

        //if(Input.GetKeyUp(KeyCode.F))
        //{
        //    teleportTimeCasted = 0;
        //}

        //if (Input.GetKey(KeyCode.F))
        //{
        //    teleportTimeCasted += Time.deltaTime;
        //    if (teleportCastTime <= teleportTimeCasted)
        //    {
        //        teleportTimeCasted = 0;
        //        if (SceneManager.GetActiveScene().name != "Town")
        //        {
        //            SceneManager.LoadScene("Town");
        //        }
        //    }
        //}
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
