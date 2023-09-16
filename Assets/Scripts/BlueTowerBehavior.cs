using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlueTowerBehavior : TowerBehavior
{

    protected override void Start()
    {
        base.Start();
        attackFrequency = 1f;
    }

    protected override void Attack()
    {
        Debug.Log("Attac!");
        foreach (GameObject enemy in enemiesInRange)
        {
            EnemyBehavior enemyBehavior = enemy.GetComponent<EnemyBehavior>();
            enemyBehavior.DamageEnemy(1);
            
        }
        UpdateEnemiesInRange();
        
    }

    private void TestCall()
    {
        
    }

}
