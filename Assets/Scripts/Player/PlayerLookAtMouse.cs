using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookAtMouse : MonoBehaviour
{
    private Vector3 mousePosition;
    private PlayerStats playerStats;


    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (playerStats.CurrentHealth > 0)
        {
            if (GameStateManager.GameState == GameStateEnum.unpaused)
            {
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (transform.position.x < (mousePosition.x - 0.1f) && transform.eulerAngles == new Vector3(0, 180, 0))
                {
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                else if (transform.position.x > (mousePosition.x + 0.1f) && transform.eulerAngles == Vector3.zero)
                {
                    transform.localRotation = Quaternion.Euler(0, 180f, 0);
                }
            }
        }
    }
}
