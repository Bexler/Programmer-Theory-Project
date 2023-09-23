using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedTowerBehavior : TowerBehavior
{

    private bool isEnemyTargeted = false;
    [SerializeField] private GameObject targetEnemy;
    private EnemyBehavior targetEnemyScript;
    private float damage = 0.5f;

    [SerializeField] private GameObject laser;

    protected override void Start()
    {
        base.Start();
        attackFrequency = 5f;
        goldCost = 15;
    }

    private void Update()
    {
        if (isEnemyTargeted)
        {
            //set laser position
            Vector3 enemyPos = targetEnemy.transform.position;
            laser.transform.position = (enemyPos + transform.position)/2;

            //set laser length
            float laserLength = (laser.transform.position - transform.position).magnitude;
            Vector3 newScale = laser.transform.localScale;
            newScale.y = laserLength;
            laser.transform.localScale = newScale;

            //set laser rotation
            Vector3 direction = targetEnemy.transform.position - laser.transform.position;
            Quaternion laserRotation = Quaternion.LookRotation(direction);
            laser.transform.rotation = laserRotation * Quaternion.Euler(90, 0, 0);
            
            //laser.transform.rotation = Quaternion.LookRotation(targetEnemy.transform.position - laser.transform.position);
        }
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
                SelectTargetEnemy(enemy);
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
            DeselectTargetEnemy();

        }
    }

    protected override void RemoveEnemyInRange(GameObject enemy)
    {
        base.RemoveEnemyInRange(enemy);
        if (enemy == targetEnemy)
        {
            DeselectTargetEnemy();
            if (isEnemyInRange)
            {
                UpdateTarget();
            }
        }
    }

    private void SelectTargetEnemy(GameObject enemy)
    {
        isEnemyTargeted = true;
        targetEnemy = enemy;
        targetEnemyScript = enemy.GetComponent<EnemyBehavior>();
        laser.SetActive(true);
    }

    private void DeselectTargetEnemy()
    {
        isEnemyTargeted=false;
        targetEnemy = null;
        targetEnemyScript = null;
        laser.SetActive(false);
    }


}
