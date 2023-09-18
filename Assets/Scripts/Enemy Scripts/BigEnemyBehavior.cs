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

    public override void DamageEnemy(float damage)
    {
        base.DamageEnemy(damage);
        if (isDefeated)
        {
            EventManager.Instance.SpawnAdds(transform.position);
        }
    }


}
