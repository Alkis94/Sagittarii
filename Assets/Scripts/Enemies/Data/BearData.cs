using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearData : EnemyData
{
    private void Awake()
    {
        //stats
        health = 30;
        speed = 2;
        delayBeforeFirstAttack = 3;
        attackFrequencies.Add(8);

        //behaviour bools
        changingDirections = false;
        jumpingBehaviour = false;

        
    }

    protected override void Start()
    {
        base.Start();

        
    }


}
