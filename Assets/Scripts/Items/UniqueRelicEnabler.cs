using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UniqueRelicEnabler : MonoBehaviour
{

    private Dictionary<string, MonoBehaviour> uniqueRelicsDictionery;

    // Use this for initialization
    void Start()
    {
        uniqueRelicsDictionery = new Dictionary<string, MonoBehaviour>();

        foreach(var monoBehaviour in GetComponents<MonoBehaviour>())
        {
            if(monoBehaviour != this)
            {
                Debug.Log(monoBehaviour.GetType());
                uniqueRelicsDictionery.Add(monoBehaviour.GetType().ToString(), monoBehaviour);
            }
        }

    }

    public void EnableRelic(string relic)
    {
        uniqueRelicsDictionery[relic].enabled = true;
    }

 
}
