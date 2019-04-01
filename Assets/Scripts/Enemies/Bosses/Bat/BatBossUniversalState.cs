using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

public class BatBossUniversalState : State<BatBossBrain>
{
    IEnumerator MultiTargetedAttack(BatBossBrain stateOwner,int attackTimes)
    {
        for(int i = 0; i < attackTimes; i++)
        {
            stateOwner.AttackPatterns[0].Attack();
            yield return new WaitForSeconds(0.25f);
        }
    }

    IEnumerator SpawnSmallBats(BatBossBrain stateOwner, float spanwFrequency)
    {
        while (true)
        {
            yield return new WaitForSeconds(spanwFrequency);
            Object.Instantiate(stateOwner.smallBat, stateOwner.transform.position, Quaternion.identity);
        }
    }
}
