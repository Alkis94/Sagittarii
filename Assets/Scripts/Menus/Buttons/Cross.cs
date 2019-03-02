using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cross : MonoBehaviour
{
    [SerializeField]
    private GameObject thisMenu;

    public void OnCrossClick ()
    {
        thisMenu.SetActive(false);
    }
}
