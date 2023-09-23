using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereEnemyBehavior : EnemyBehavior
{

    //switches damage and slow
    //slows now deal damage to spheres
    //damage now slows spheres

    protected override void Start()
    {

        health = 10f;

        speed = 5f;

        goldOnDeath = 3;
        base.Start();
    }

    public override void DamageEnemy(float damage)
    {
        base.SlowSpeed(damage);
    }

    public override void SlowSpeed(float slowStrength)
    {
        base.DamageEnemy(slowStrength);
    }

}
