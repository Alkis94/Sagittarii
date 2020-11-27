using UnityEngine;
using System.Collections;

public class TaxPopup : MonoBehaviour
{
    private bool poped = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!poped && collision.tag == "Player")
        {
            transform.GetChild(0).gameObject.SetActive(true);
            poped = true;
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
