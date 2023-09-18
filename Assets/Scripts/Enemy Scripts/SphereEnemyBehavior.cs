using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereEnemyBehavior : EnemyBehavior
{

    //switches damage and slow
    //slows now deal damage to spheres
    //damage now slows spheres

    public override void DamageEnemy(float damage)
    {
        base.SlowSpeed(damage);
    }

    public override void SlowSpeed(float slowStrength)
    {
        base.DamageEnemy(slowStrength);
    }

}
