using UnityEngine;
using System.Collections.Generic;

public class GuardEnabler : MonoBehaviour
{
    private GameObject[] guards = new GameObject[5];
    [SerializeField]
    private TaxHandler taxHandler;
    public bool TaxTheft { get; private set; } = false;

    private void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            guards[i] = transform.GetChild(i).gameObject;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (!taxHandler.TaxWasPaid)
            {

                int i = 0;
                foreach (GameObject guard in guards)
                {
                    TaxTheft = true;
                    guard.GetComponent<GuardBrain>().enabled = true;
                    guard.GetComponent<GuardBrain>().ExtraDinstance = i;
                    guard.GetComponent<Animator>().enabled = true;
                    guard.transform.GetChild(0).gameObject.SetActive(true);
                    i++;
                }
            }
        }
    }

}
