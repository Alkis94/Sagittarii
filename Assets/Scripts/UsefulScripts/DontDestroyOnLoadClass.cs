using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadClass : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
