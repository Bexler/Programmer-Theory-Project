using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemyBehavior : EnemyBehavior
{

    //more health
    //on death spawn smaller enemies

    protected override void Start()
    {
        health = 20;
        speed = 3;
        baseSpeed = speed;
    }

    //Add spawn logic is in GameManager for now


}
