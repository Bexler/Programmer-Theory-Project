using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddEnemyBehavior : EnemyBehavior
{
    protected override void Start()
    {
        health = 5 + waveWhenSpawned;
        speed = 5f * (1 + (waveWhenSpawned / 100) * 5);

        base.Start();
    }
}
