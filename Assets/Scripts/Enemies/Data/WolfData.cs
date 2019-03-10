using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfData : EnemyData
{

    private void Awake()
    {
        //stats
        health = 10;
        speed = 3;
        delayBeforeFirstAttack = 3;
        attackFrequencies.Add(3);

        //behaviour bools
        changingDirections = false;
        jumpingBehaviour = true;
    }

    protected override void Start()
    {
        base.Start();
    }




}
