using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemyBehavior : EnemyBehavior
{

    //more health
    //on death spawn smaller enemies

    protected override void Start()
    {
        health = 20 + (3 * waveWhenSpawned);
        speed = 3f * (1 + (waveWhenSpawned/100)*5);
        baseSpeed = speed;
    }

    //Add spawn logic is in GameManager for now


}
