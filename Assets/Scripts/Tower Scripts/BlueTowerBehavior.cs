using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlueTowerBehavior : TowerBehavior
{

    private int damage = 1;

    protected override void Start()
    {
        base.Start();
        attackFrequency = 1f;
    }

    protected override void Attack()
    {
        //Debug.Log("Attac!");
        foreach (GameObject enemy in enemiesInRange)
        {
            EnemyBehavior enemyBehavior = enemy.GetComponent<EnemyBehavior>();
            enemyBehavior.DamageEnemy(damage);
            
        }
        UpdateEnemiesInRange();
        
    }

    private void TestCall()
    {
        
    }

}
