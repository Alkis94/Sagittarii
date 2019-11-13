using UnityEngine;
using System.Collections;

public abstract class MoveOnAttackPattern : MonoBehaviour
{
    abstract public void MoveOnAttack(float MoveForceFactor);
}
