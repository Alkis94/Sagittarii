using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateParentButton: MonoBehaviour
{
    public void OnButtonClick ()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
