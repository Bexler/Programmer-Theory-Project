using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedTowerBehavior : TowerBehavior
{

    [SerializeField] private GameObject targetEnemy;
    private EnemyBehavior targetEnemyScript;
    private float damage = 0.5f;

    protected override void Start()
    {
        base.Start();
        attackFrequency = 5f;
    }

    private void UpdateTarget()
    {

        float maxHealth = 0;
        foreach (GameObject enemy in enemiesInRange)
        {
            EnemyBehavior enemyBehavior = enemy.GetComponent<EnemyBehavior>();
            float enemyHealth = enemyBehavior.health;

            if (enemyHealth > maxHealth)
            {
                maxHealth = enemyHealth;
                targetEnemy = enemy;
                targetEnemyScript = enemyBehavior;
            }
        }
        if (maxHealth == 0)
        {
            Debug.Log("No enemy found!");
        }
    }

    protected override void Attack()
    {
        if(targetEnemyScript == null)
        {
            //Debug.Log("Update Red Tower target!");
            UpdateTarget();
        }
        //Debug.Log("Laser!");
        if(targetEnemyScript == null)
        {
            Debug.LogError("Attacking nothing!");
        }
        targetEnemyScript.DamageEnemy(damage);

        if (targetEnemyScript.isDefeated)
        {
            targetEnemy = null;
            targetEnemyScript = null;
        }
    }

    protected override void RemoveEnemyInRange(GameObject enemy)
    {
        base.RemoveEnemyInRange(enemy);
        if (enemy == targetEnemy)
        {
            targetEnemy = null;
            targetEnemyScript = null;
            if (isEnemyInRange)
            {
                UpdateTarget();
            }
        }
    }


}
