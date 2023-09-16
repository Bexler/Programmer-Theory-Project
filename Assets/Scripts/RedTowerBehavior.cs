using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedTowerBehavior : TowerBehavior
{

    private EnemyBehavior laserTargetScript;
    private float damage = 0.5f;

    protected override void Start()
    {
        base.Start();
        attackFrequency = 5f;
        UpdateTarget();
    }

    private void UpdateTarget()
    {

        int maxHealth = 0;
        foreach (GameObject enemy in enemiesInRange)
        {
            EnemyBehavior enemyBehavior = enemy.GetComponent<EnemyBehavior>();
            if (enemyBehavior.health > maxHealth)
            {
                laserTargetScript = enemyBehavior;
            }
        }
    }

    protected override void Attack()
    {
        if(laserTargetScript == null)
        {
            UpdateTarget();
        }
        Debug.Log("Laser!");
        laserTargetScript.DamageEnemy(damage);

        if (laserTargetScript.isDefeated)
        {
            laserTargetScript = null;
        }
    }

}
