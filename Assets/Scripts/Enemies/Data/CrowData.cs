using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowData : EnemyData
{

    private void Awake()
    {
        //stats
        health = 10;
        speed = 2;
        delayBeforeFirstAttack = 3;
        attackFrequencies.Add(3);

        //behaviour bools
        changingDirections = false;
        jumpingBehaviour = false;


    }

    protected override void Start()
    {
        base.Start();
    }

}
