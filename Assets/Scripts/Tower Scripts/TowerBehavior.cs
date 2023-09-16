using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class TowerBehavior : MonoBehaviour
{

    protected float range;
    [SerializeField] protected List<GameObject> enemiesInRange = new List<GameObject> ();
    protected List<GameObject> defeatedEnemies = new List<GameObject> ();

    [SerializeField] private GameObject rangeIndicator;
    private bool isEnemyInRange = false;
    private Coroutine attackRoutine;

    protected float attackFrequency { get; set; }

    private void OnEnable()
    {
        EventManager.Instance.OnEnemyDeath += AddDefeatedEnemy;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnEnemyDeath -= AddDefeatedEnemy;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        DisableRangeIndicator();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        if(defeatedEnemies.Count > 0)
        {
            UpdateEnemiesInRange();
        }
    }

    IEnumerator AttackRoutine()
    {
        while (isEnemyInRange)
        {
            Debug.Log("I have been called to attack with frequency " + attackFrequency);
            Attack();
            yield return new WaitForSeconds(1/attackFrequency);
        }
        attackRoutine = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //for testing purposes show the amount of towers that the enemy is in range for
            other.GetComponent<EnemyBehavior>().towerTargetCount++;

            Debug.Log("Found Enemy!");
            AddEnemyInRange(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //for testing purposes show the amount of towers that the enemy is in range for
            other.GetComponent<EnemyBehavior>().towerTargetCount--;

            Debug.Log("Lost Enemy!");
            RemoveEnemyInRange(other.gameObject);
        }
    }

    private void AddEnemyInRange(GameObject enemy)
    {
        enemiesInRange.Add(enemy);
        CheckFirstEnemyInRange();
    }

    protected virtual void RemoveEnemyInRange(GameObject enemy)
    {
        enemiesInRange.Remove(enemy);
        CheckLastEnemyInRange();
    }

    private void AddDefeatedEnemy(GameObject enemy)
    {
        defeatedEnemies.Add(enemy);
    }

    protected void UpdateEnemiesInRange()
    {
        for(int i = 0; i < defeatedEnemies.Count; i++)
        {
            RemoveEnemyInRange(defeatedEnemies[i]);
        }
        defeatedEnemies.Clear();
    }

    private void CheckFirstEnemyInRange()
    {
        if(enemiesInRange.Count == 1)
        {
            isEnemyInRange = true;
            StartAttacking();
        }
    }

    private void StartAttacking()
    {
        if(attackRoutine == null)
        {
            attackRoutine = StartCoroutine(AttackRoutine());
        }
    }

    private void CheckLastEnemyInRange()
    {
        if(enemiesInRange.Count == 0)
        {
            isEnemyInRange = false;
        }
    }

    private void DisableRangeIndicator()
    {
        rangeIndicator.SetActive(false);
    }

    private void EnableRangeIndicator()
    {
        rangeIndicator.SetActive(true);
    }

    protected abstract void Attack();
}
