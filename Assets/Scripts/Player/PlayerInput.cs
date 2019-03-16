// This script handles inputs for the player. It serves two main purposes: 1) wrap up
// inputs so swapping between mobile and standalone is simpler and 2) keeping inputs
// from Update() in sync with FixedUpdate()

using Factories;
using UnityEngine;


//We first ensure this script runs before all other player scripts to prevent laggy
//inputs
[DefaultExecutionOrder(-100)]
public class PlayerInput : MonoBehaviour
{

    public GameObject Map;

	[HideInInspector] public float horizontal;		//Float that stores horizontal input
	[HideInInspector] public bool jumpHeld;			//Bool that stores jump pressed
	[HideInInspector] public bool jumpPressed;		//Bool that stores jump held

	bool readyToClear;                              //Bool used to keep input in sync

    void OnEnable()
    {
        PlayerCollision.OnDeath += DisableInput;
    }


    void OnDisable()
    {
        PlayerCollision.OnDeath -= DisableInput;
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

        if(Input.GetKeyDown(KeyCode.M))
        {
            Map.SetActive(!Map.activeInHierarchy);
        }

        ClearInput();
		ProcessInputs();
		horizontal = Mathf.Clamp(horizontal, -1f, 1f);
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

}
