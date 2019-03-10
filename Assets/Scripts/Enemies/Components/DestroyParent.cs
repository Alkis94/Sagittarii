using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParent : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponentInChildren<EnemyGotShot>().OnDeath += DestoryThis;
    }

    //private void OnDestroy()
    //{
    //    GetComponentInChildren<EnemyGotShot>().OnDeath -= DestoryThis;
    //}

    private void DestoryThis()
    {
        Destroy(gameObject, 10f);
    }
}
