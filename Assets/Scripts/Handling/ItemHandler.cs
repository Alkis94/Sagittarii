using System.Collections;
using System.Collections.Generic;

public static class ItemHandler 
{

    public static Dictionary<string, bool> ItemDropped = new Dictionary<string, bool>()
    {
        {"DeadBird",false},
        {"ImpFlame",false},
        {"BatWings",false}
    };

    


    public static bool BatWingsNotDropped = true;


    //public bool ImpFlameNotDropped;
    //public bool WolfPawNotDropped;

    public static bool PlayerHasImpFlame = false;
    public static bool PlayerHasBatWings = false;
    public static int WolfPawMultiplier = 1;


}
