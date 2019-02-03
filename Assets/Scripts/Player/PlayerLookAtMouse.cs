using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookAtMouse : MonoBehaviour
{

    private Vector3 MousePosition;

    void Update()
    {
        if (PlayerStats.CurrentHealth > 0)
        {
            if (!GameState.GamePaused)
            {

                MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (transform.position.x < MousePosition.x)
                {
                    transform.localRotation = Quaternion.Euler(0,0,0);
                }
                else if (transform.position.x > MousePosition.x)
                {
                    transform.localRotation = Quaternion.Euler(0, 180, 0);
                }
            }
        }
    }
}