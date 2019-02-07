using UnityEngine;
using Factories;
using System.Collections;

[RequireComponent (typeof (PlayerMovementController))]
public class PlayerInput : MonoBehaviour {

    private PlayerMovementController controller2D;

    void OnEnable()
    {
        PlayerCollision.OnDeath += DisableInput;
    }


    void OnDisable()
    {
        PlayerCollision.OnDeath -= DisableInput;
    }


    void Start ()
    {
        controller2D = GetComponent<PlayerMovementController> ();
	}


    void Update()
    {
        controller2D.playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetButtonDown("Cancel") && !GameState.GamePaused)
        {
            MenuFactory.CallPauseMenu();
        }
        else if (Input.GetButtonDown("Cancel") && GameState.GamePaused)
        {
            MenuFactory.DestroyMenuAndUnpause();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            controller2D.OnJump();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            controller2D.OnJumpInputUp();
        }

    }

    private void DisableInput()
    {
        controller2D.playerInput = Vector2.zero;
        enabled = false;
    }
}
