using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookAtMouse : MonoBehaviour
{
    private BoxCollider2D boxCollider2d;
    private Vector3 MousePosition;

    private void Start()
    {
        boxCollider2d = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (PlayerStats.CurrentHealth > 0)
        {
            if (!GameState.GamePaused)
            {
                MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (transform.position.x < (MousePosition.x - 0.1f) && transform.eulerAngles == new Vector3(0, 180, 0))
                {
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                else if (transform.position.x > (MousePosition.x + 0.1f) && transform.eulerAngles == Vector3.zero)
                {
                    transform.localRotation = Quaternion.Euler(0, 180f, 0);
                }
            }
        }
    }
}
