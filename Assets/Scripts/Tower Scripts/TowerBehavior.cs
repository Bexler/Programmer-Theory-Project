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

    protected bool isEnemyInRange = false;

    private Coroutine attackRoutine;
    private bool isNewEnemySpawned = false;
    private bool isActive = false;

    protected float attackFrequency { get; set; }

    public int placementBlockers = 0;

    private void OnEnable()
    {
        EnableTower();
    }

    private void OnDisable()
    {
        EventManager.Instance.OnEnemyDeath -= AddDefeatedEnemy;
        EventManager.Instance.OnEnemySpawn -= UpdateEnemiesInRange;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        range = GetComponentInChildren<SphereCollider>().radius;
        //DisableRangeIndicator();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        if(defeatedEnemies.Count > 0)
        {
            RemoveDefeatedEnemiesInRange();
        }
        if (isNewEnemySpawned)
        {
            AddEnemiesInRange();
        }
    }

    IEnumerator AttackRoutine()
    {
        while (isEnemyInRange)
        {
            //Debug.Log("I have been called to attack with frequency " + attackFrequency);
            Attack();
            yield return new WaitForSeconds(1/attackFrequency);
        }
        attackRoutine = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isActive && other.CompareTag("Enemy"))
        {
            //for testing purposes show the amount of towers that the enemy is in range for
            other.GetComponent<EnemyBehavior>().towerTargetCount++;

            //Debug.Log("Found Enemy!");
            AddEnemyInRange(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isActive && other.CompareTag("Enemy"))
        {
            //for testing purposes show the amount of towers that the enemy is in range for
            other.GetComponent<EnemyBehavior>().towerTargetCount--;

            //Debug.Log("Lost Enemy!");
            RemoveEnemyInRange(other.gameObject);
        }
    }

    public void EnableTower()
    {
        Debug.Log("Enable Tower.");
        isActive = true;
        EventManager.Instance.OnEnemyDeath += AddDefeatedEnemy;
        EventManager.Instance.OnEnemySpawn += UpdateEnemiesInRange;
        DisableRangeIndicator();
        AddEnemiesInRange();
    }

    public void DisableTower()
    {
        Debug.Log("Disable Tower.");
        isActive = false;
        EventManager.Instance.OnEnemyDeath -= AddDefeatedEnemy;
        EventManager.Instance.OnEnemySpawn -= UpdateEnemiesInRange;
        EnableRangeIndicator();
        ResetVariables();
    }

    private void ResetVariables()
    {
        enemiesInRange.Clear();
        defeatedEnemies.Clear();
        isEnemyInRange = false;
    }

    private void AddEnemyInRange(GameObject enemy)
    {
        if (!enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Add(enemy);
            CheckFirstEnemyInRange();
        }
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

    private void AddEnemiesInRange()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);

        foreach (Collider col in colliders)
        {
            // Check if the collider belongs to an object you want to detect
            if (col.CompareTag("Enemy") && !col.GetComponent<EnemyBehavior>().isDefeated)
            {
                AddEnemyInRange(col.gameObject);
            }
        }
        isNewEnemySpawned = false;
    }

    private void UpdateEnemiesInRange()
    {
        isNewEnemySpawned = true;
        
    }

    protected void RemoveDefeatedEnemiesInRange()
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
            //StopCoroutine(AttackRoutine());
        }
    }

    public void DisableRangeIndicator()
    {
        rangeIndicator.SetActive(false);
    }

    public void EnableRangeIndicator()
    {
        rangeIndicator.SetActive(true);
    }

    protected abstract void Attack();

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Tower"))
        {
            placementBlockers++;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Tower"))
        {
            placementBlockers--;
        }
    }

    public bool IsTowerBlocked()
    {
        return placementBlockers > 0 ? true : false;
    }
}
