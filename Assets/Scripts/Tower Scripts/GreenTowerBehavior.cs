using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenTowerBehavior : TowerBehavior
{
    [SerializeField] private ParticleSystem snowStorm;
    private float slowStrength = 0.5f;


    protected override void Start()
    {
        base.Start();
        attackFrequency = 3f;
        goldCost = 20;
    }

    protected override void Attack()
    {
        //Debug.Log("Slow!");
        foreach (GameObject enemy in enemiesInRange)
        {
            EnemyBehavior enemyBehavior = enemy.GetComponent<EnemyBehavior>();
            enemyBehavior.SlowSpeed(slowStrength);
        }
    }

    public override void EnableTower()
    {
        base.EnableTower();
        snowStorm.Play();
    }

    public override void DisableTower()
    {
        base.DisableTower();
        snowStorm.Stop();
    }


}
