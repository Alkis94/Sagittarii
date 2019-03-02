using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopOrb : MonoBehaviour
{
    [SerializeField]
    private GameObject orbMenu;

    void Start()
    {
        orbMenu.SetActive(false);
    }


    public void OnOrbClick()
    {
        orbMenu.SetActive(true);
    }


}
