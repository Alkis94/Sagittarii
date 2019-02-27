using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackPatern : MonoBehaviour
{
    abstract public void Attack(EnemyData enemyData);
}
