using UnityEngine;
using System.Collections;

public class ItemHandler : MonoBehaviour
{
    //   # # # # # # # # # # # # 
    //   #                     #
    //   #  SINGLETON CLASS    #
    //   #                     #
    //   # # # # # # # # # # # # 

    public static ItemHandler Instance = null;

    public bool BatWingsNotDropped;
    public bool DeadBirdNotDropped;
    public bool PlayerHasDeadBird;
    //public bool ImpFlameNotDropped;
    //public bool WolfPawNotDropped;
    public bool PlayerHasImpFlame;
    public int WolfPawMultiplier;
    public int ImpFlameMultiplier;

    void Awake()
    {
        // First we check if there are any other instances conflicting
        if (Instance != null && Instance != this)
        {
            // If that is the case, we destroy other instances
            Destroy(gameObject);
        }
        // Here we save our singleton instance
        Instance = this;
    }


    // Use this for initialization
    void Start ()
    {
        //WolfPawNotDropped = true;
        //ImpFlameNotDropped = true;
        DeadBirdNotDropped = true;
        BatWingsNotDropped = true;
        PlayerHasDeadBird = false;
        PlayerHasImpFlame = false;
        WolfPawMultiplier = 1;
        ImpFlameMultiplier = 1;
    }
}
