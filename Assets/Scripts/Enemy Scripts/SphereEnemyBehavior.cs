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
        
        health = 10 + (2 * waveWhenSpawned);
        speed = 5 * (1 + (waveWhenSpawned / 100) * 5);
        baseSpeed = speed;
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
