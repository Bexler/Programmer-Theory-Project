using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : MonoBehaviour
{

    private float range;
    private List<GameObject> enemiesInRange = new List<GameObject> ();
    [SerializeField] private GameObject rangeIndicator;
    private bool isEnemyInRange = false;
    private float attackFrequency = 1f;


    private void OnEnable()
    {
        EventManager.Instance.OnEnemyDeath += RemoveEnemyInRange;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnEnemyDeath -= RemoveEnemyInRange;
    }

    // Start is called before the first frame update
    void Start()
    {
        DisableRangeIndicator();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator AttackRoutine()
    {
        while (isEnemyInRange)
        {
            Attack();
            yield return new WaitForSeconds(attackFrequency);
        }
        yield return new WaitForSeconds(attackFrequency);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Found Enemy!");
            AddEnemyInRange(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Lost Enemy!");
            RemoveEnemyInRange(other.gameObject);
        }
    }

    private void AddEnemyInRange(GameObject enemy)
    {
        enemiesInRange.Add(enemy);
        CheckFirstEnemyInRange();
    }

    private void RemoveEnemyInRange(GameObject enemy)
    {
        enemiesInRange.Remove(enemy);
        CheckLastEnemyInRange();
    }

    private void CheckFirstEnemyInRange()
    {
        if(enemiesInRange.Count == 1)
        {
            isEnemyInRange = true;
            StartCoroutine(AttackRoutine());
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

    protected virtual void Attack()
    {
        Debug.Log("Attac!");
        foreach(GameObject enemy in enemiesInRange)
        {
            Destroy(enemy);
        }
    }
}
