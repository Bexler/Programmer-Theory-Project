using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{



    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Finsh line touchu!");
        if (other.CompareTag("Enemy"))
        {
            
            EnemyBehavior enemyBehaviorScript = other.GetComponent<EnemyBehavior>();
            float enemyRemainingHealth = enemyBehaviorScript.health;

            Debug.Log("Enemy survived with " + enemyRemainingHealth + " health");

            EventManager.Instance.EnemySurvival(enemyRemainingHealth);
            enemyBehaviorScript.DamageEnemy(enemyRemainingHealth);
        }
    }

}
