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

        //behaviour bools
        changingDirections = false;
        attackIsDirectionDependant = true;
        jumpingBehaviour = true;

        //attack stats
        projectileRotations.Add(10);
        projectileRotations.Add(0);
        projectileRotations.Add(350);
        projectileSpeed = 10;
        projectileDestroyDelay = 0.5f;
        attackFrequency = 3;
        projectileSpawnPositionOffset = new Vector3(1, 0, 0);
    }

    protected override void Start()
    {
        base.Start();
    }




}
