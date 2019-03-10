using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatData : EnemyData
{
    private void Awake()
    {
        //stats
        health = 10;
        speed = 1;
        delayBeforeFirstAttack = 1;
        changeDirectionFrequency = 3;
        attackFrequencies.Add(4);

        //behaviour bools
        changingDirections = true;
        jumpingBehaviour = false;
    }

    protected override void Start()
    {
        base.Start();

        
    }


}
