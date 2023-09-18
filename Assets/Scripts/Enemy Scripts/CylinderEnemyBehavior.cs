using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderEnemyBehavior : EnemyBehavior
{

    //Accelerate when taking damage
    //Can only be slowed down to base speed

    private float baseHealth;

    protected override void Start()
    {
        base.Start();
        baseHealth = health;
    }

    public override void SlowSpeed(float slowStrength)
    {
        base.SlowSpeed(slowStrength);
        if(baseSpeed > speed)
        {
            speed = baseSpeed;
        }
    }

    public override void DamageEnemy(float damage)
    {
        float previousHealth = health;
        base.DamageEnemy(damage);
        if(!isDefeated)
        {
            float healthLoss = previousHealth - health;
            float speedModifier = healthLoss / baseHealth;
            IncreaseSpeed(speedModifier);
        }
    }

    private void IncreaseSpeed(float speedMod)
    {
        speed *= (1+speedMod);
    }

}
