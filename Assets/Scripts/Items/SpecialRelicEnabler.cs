using UnityEngine;
using System.Collections.Generic;

public class SpecialRelicEnabler : MonoBehaviour
{

    private Dictionary<string, MonoBehaviour> specialRelicDictionery;

    private void OnEnable()
    {
        specialRelicDictionery = new Dictionary<string, MonoBehaviour>();

        foreach(var monoBehaviour in GetComponents<MonoBehaviour>())
        {
            if(monoBehaviour != this)
            {
                specialRelicDictionery.Add(monoBehaviour.GetType().ToString(), monoBehaviour);
            }
        }

        if (ES3.FileExists("Saves/Profile" + SaveProfile.SaveID + "/SpecialItems"))
        {
            foreach (var relic in specialRelicDictionery)
            {
                if (ES3.KeyExists("Special" +relic.Key, "Saves/Profile" + SaveProfile.SaveID + "/SpecialItems"))
                {
                    specialRelicDictionery[relic.Key].enabled = true;
                }
            }

        }

    }

    public void EnableRelic(string relic)
    {
        specialRelicDictionery[relic].enabled = true;
        ES3.Save<bool>("Special" + relic, true, "Saves/Profile" + SaveProfile.SaveID + "/SpecialItems");
    }

 
}
