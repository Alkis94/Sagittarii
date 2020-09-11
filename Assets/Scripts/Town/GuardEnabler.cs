using UnityEngine;
using System.Collections.Generic;

public class GuardEnabler : MonoBehaviour
{
    private GameObject[] guards = new GameObject[5];
    [SerializeField]
    private TaxHandler taxHandler;

    private void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            guards[i] = transform.GetChild(i).gameObject;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!taxHandler.taxWasPaid)
        {
            foreach(GameObject guard in guards)
            {
                guard.GetComponent<GuardBrain>().enabled = true;
                guard.GetComponent<Animator>().enabled = true;
                guard.transform.GetChild(0).gameObject.SetActive(true);

            }
        }
    }

}
