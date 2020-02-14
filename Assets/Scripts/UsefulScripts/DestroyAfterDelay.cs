using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{

    [SerializeField]
    private float destroyDelay;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyObject", destroyDelay);
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
