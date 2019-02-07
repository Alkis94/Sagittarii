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

                if (transform.position.x < (MousePosition.x - 0.1f) && transform.localRotation.Equals(Quaternion.Euler(0,180,0)))
                {
                    transform.localRotation = Quaternion.Euler(0,0,0);

                    // We change the transform position based on boxcollifer.offset. This way whenever the sprite flips the collider remains on the same exact position in the world space.
                    // this prevents wall clipping and other bugs.
                    transform.position = new Vector3(transform.position.x - (2 * boxCollider2d.offset.x) + 0.01f, transform.position.y);
                    
                }
                else if (transform.position.x > (MousePosition.x + 0.1f) && transform.localRotation.Equals(Quaternion.Euler(0, 0, 0)))
                {
                    transform.localRotation = Quaternion.Euler(0, 180, 0);
                    //Same as above comment
                    transform.position = new Vector3(transform.position.x + (2 * boxCollider2d.offset.x) - 0.01f, transform.position.y);
                }
            }
        }
    }
}