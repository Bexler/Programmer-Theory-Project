using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddEnemyBehavior : EnemyBehavior
{
    protected override void Start()
    {
        health = 5f;
        speed = 5f; 

        goldOnDeath = 1;

        base.Start();
    }
}
