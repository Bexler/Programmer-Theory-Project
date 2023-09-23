using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlueTowerBehavior : TowerBehavior
{

    [SerializeField] private GameObject shockwave;
    [SerializeField] private float shockwaveSpeed = 10;

    private int damage = 1;
    private bool isShockwaving;

    protected override void Start()
    {
        base.Start();
        attackFrequency = 1f;
        goldCost = 10;
    }

    private void Update()
    {
        if (isShockwaving)
        {
            shockwave.transform.localScale *= (1 + (shockwaveSpeed * Time.deltaTime));
            if(shockwave.transform.localScale.x > 10)
            {
                DisableShockwave();
            }
        }
    }

    protected override void Attack()
    {
        //Debug.Log("Attac!");
        foreach (GameObject enemy in enemiesInRange)
        {
            EnemyBehavior enemyBehavior = enemy.GetComponent<EnemyBehavior>();
            enemyBehavior.DamageEnemy(damage);
            
        }
        RemoveDefeatedEnemiesInRange();
        EnableShockwave();
        
    }

    private void EnableShockwave()
    {
        isShockwaving = true;
        shockwave.SetActive(true);
        shockwave.transform.localScale = Vector3.one;
    }

    private void DisableShockwave()
    {
        isShockwaving = false;
        shockwave.SetActive(false);

    }

}
