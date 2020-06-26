using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpecialRelicEnabler : MonoBehaviour
{

    private Dictionary<string, MonoBehaviour> uniqueRelicsDictionery;

    void Start()
    {
        uniqueRelicsDictionery = new Dictionary<string, MonoBehaviour>();

        foreach(var monoBehaviour in GetComponents<MonoBehaviour>())
        {
            if(monoBehaviour != this)
            {
                uniqueRelicsDictionery.Add(monoBehaviour.GetType().ToString(), monoBehaviour);
            }
        }

    }

    public void EnableRelic(string relic)
    {
        uniqueRelicsDictionery[relic].enabled = true;
    }

 
}
