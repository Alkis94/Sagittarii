using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderData : EnemyData
{

    private void Awake()
    {
        attackFrequencies.Add(3);
    }

    protected override void Start()
    {
        base.Start();
    }


}
