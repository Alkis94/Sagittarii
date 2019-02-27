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

        //behaviour bools
        changingDirections = true;
        attackIsDirectionDependant = false;
        jumpingBehaviour = false;

        //attack stats
        projectileSpeed = 5;
        projectileDestroyDelay = 15f;
        attackFrequency = 3;
        projectileSpawnPositionOffset = new Vector3(0, 0, 0);
    }

    protected override void Start()
    {
        base.Start();

        
    }


}
