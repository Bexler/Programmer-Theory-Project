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
        UpdateTarget();
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
    }

    protected override void Attack()
    {
        if(targetEnemyScript == null)
        {
            UpdateTarget();
        }
        Debug.Log("Laser!");
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
            UpdateTarget();
        }
    }


}
