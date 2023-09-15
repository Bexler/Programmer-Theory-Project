using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueTowerBehavior : TowerBehavior
{

    protected override void Start()
    {
        base.Start();
        attackFrequency = 1f;
    }

    private void TestCall()
    {
        
    }

}
