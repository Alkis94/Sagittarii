using UnityEngine;
using Factories;
using System.Collections;

[RequireComponent (typeof (PlayerController))]
public class PlayerInput : MonoBehaviour {

    PlayerController controller2D;

	void Start () {
        controller2D = GetComponent<PlayerController> ();
	}

	void Update () {
		Vector2 directionalInput = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
        controller2D.SetDirectionalInput (directionalInput);

        if (Input.GetButtonDown("Cancel") && !GameState.GamePaused)
        {
            MenuFactory.CallPauseMenu();
        }
        else if (Input.GetButtonDown("Cancel") && GameState.GamePaused)
        {
            MenuFactory.DestroyMenuAndUnpause();
        }

        if (Input.GetKeyDown (KeyCode.Space)) {
            controller2D.OnJump ();
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
            controller2D.OnJumpInputUp ();
		}
        
    }
}
