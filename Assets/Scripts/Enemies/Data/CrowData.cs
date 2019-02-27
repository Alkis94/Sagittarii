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

        //behaviour bools
        changingDirections = false;
        attackIsDirectionDependant = false;
        jumpingBehaviour = false;

        //attack stats
        projectileRotations.Add(240);
        projectileRotations.Add(270);
        projectileRotations.Add(300);
        projectileSpeed = 3;
        projectileDestroyDelay = 15f;
        attackFrequency = 3;
        projectileSpawnPositionOffset = new Vector3(0, 0, 0);
    }

    protected override void Start()
    {
        base.Start();
    }

}
