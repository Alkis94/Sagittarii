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

        //behaviour bools
        changingDirections = false;
        attackIsDirectionDependant = true;
        jumpingBehaviour = false;

        //attack stats
        projectileRotations.Add(0);
        projectileSpeed = 4;
        projectileDestroyDelay = 30f;
        attackFrequency = 5;
        projectileSpawnPositionOffset = new Vector3(0, 0, 0);
    }

    protected override void Start()
    {
        base.Start();

        
    }


}
