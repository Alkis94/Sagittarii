using UnityEngine;

public class Universal : MonoBehaviour
{
    public static Universal instance = null;

    void Awake()
    {
        // First we check if there are any other instances conflicting
        if (instance != null && instance != this)
        {
            // If that is the case, we destroy other instances
            Destroy(gameObject);
        }
        else
        {
            // Here we save our singleton instance
            instance = this;
        }

        if(instance == this)
        {
            DontDestroyOnLoad(gameObject);
        }
        
    }

}
