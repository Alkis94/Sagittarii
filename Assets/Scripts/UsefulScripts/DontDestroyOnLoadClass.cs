using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadClass : MonoBehaviour
{
    private static DontDestroyOnLoadClass instance = null;

    void Awake()
    {
        // First we check if there are any other instances conflicting
        if (instance != null && instance != this)
        {
            // If that is the case, we destroy other instances
            Destroy(instance.gameObject);
        }
        else
        {
            // Here we save our singleton instance
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

}
