using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    public event Action<GameObject> OnEnemyDeath;

    public event Action<float> OnEnemySurvival;

    public event Action OnEnemySpawn;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnemyDeath(GameObject enemy)
    {
        Debug.Log("Enemy Death!");
        OnEnemyDeath?.Invoke(enemy);
    }

    public void EnemySurvival(float enemyHealth)
    {
        Debug.Log("Enemy Survival!");
        OnEnemySurvival?.Invoke(enemyHealth);
    }

    public void EnemySpawn()
    {
        Debug.Log("Enemy Spawn!");
        OnEnemySpawn?.Invoke();
    }
}
