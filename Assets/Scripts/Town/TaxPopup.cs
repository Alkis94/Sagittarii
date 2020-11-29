using UnityEngine;
using System.Collections;

public class TaxPopup : MonoBehaviour
{
    private TaxHandler taxHandler;
    private GuardEnabler guardEnabler;

    private void Start()
    {
        taxHandler = GetComponentInChildren<TaxHandler>(true);
        guardEnabler = FindObjectOfType<GuardEnabler>().GetComponent<GuardEnabler>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !taxHandler.TaxWasPaid && !guardEnabler.TaxTheft)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

}
