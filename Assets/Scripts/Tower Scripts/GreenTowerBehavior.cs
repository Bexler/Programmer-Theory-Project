using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenTowerBehavior : TowerBehavior
{
    private float slowStrength = 0.5f;


    protected override void Start()
    {
        base.Start();
        attackFrequency = 3f;
    }

    protected override void Attack()
    {
        Debug.Log("Slow!");
        foreach (GameObject enemy in enemiesInRange)
        {
            EnemyBehavior enemyBehavior = enemy.GetComponent<EnemyBehavior>();
            enemyBehavior.SlowSpeed(slowStrength);
        }
    }


}
