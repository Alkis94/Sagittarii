using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackPatern : MonoBehaviour
{
    [SerializeField]
    protected AttackData attackData;

    abstract public void Attack();
}
