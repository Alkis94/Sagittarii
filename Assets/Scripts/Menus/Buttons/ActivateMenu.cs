using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject thisMenu;

    public void OnButtonClick()
    {
        thisMenu.SetActive(true);
    }
}
