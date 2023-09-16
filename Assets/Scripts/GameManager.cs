using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private GameObject spawnPrefab;
    private float spawnFrequency = 1f;

    private int gold = 0;

    [SerializeField] private GameObject UIManager;
    private MainUIManager UIManagerScript;

    private void OnEnable()
    {
        EventManager.Instance.OnEnemyDeath += AddMoney;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnEnemyDeath -= AddMoney;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
        UIManagerScript = UIManager.GetComponent<MainUIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnFrequency);
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        Instantiate(spawnPrefab, spawnPosition, spawnPrefab.transform.rotation);
    }

    private void AddMoney(GameObject enemy)
    {
        gold++;
        UIManagerScript.UpdateGoldText(gold);
    }
}
