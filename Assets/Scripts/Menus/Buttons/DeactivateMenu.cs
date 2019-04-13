using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject thisMenu;

    public void OnButtonClick ()
    {
        thisMenu.SetActive(false);
    }
}
