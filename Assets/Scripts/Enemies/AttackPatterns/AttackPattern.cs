using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackPattern : MonoBehaviour
{
    [SerializeField]
    protected AttackData attackData;

    abstract public void Attack();
}
